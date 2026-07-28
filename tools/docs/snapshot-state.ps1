function Get-SnapshotDirectoryDigest {
    param([Parameter(Mandatory = $true)][string]$Path)

    $lines = Get-ChildItem $Path -File -Recurse -Force |
        Sort-Object FullName |
        ForEach-Object {
            $relativePath = [System.IO.Path]::GetRelativePath(
                $Path,
                $_.FullName
            ).Replace("\", "/")
            "$relativePath $((Get-FileHash $_.FullName -Algorithm SHA256).Hash)"
        }
    $bytes = [System.Text.Encoding]::UTF8.GetBytes(($lines -join "`n"))
    return [Convert]::ToHexString(
        [System.Security.Cryptography.SHA256]::HashData($bytes)
    )
}

function Get-SnapshotArtifactDigest {
    param([Parameter(Mandatory = $true)][string]$Path)

    if (Test-Path $Path -PathType Container) {
        return Get-SnapshotDirectoryDigest -Path $Path
    }
    if (Test-Path $Path -PathType Leaf) {
        return (Get-FileHash $Path -Algorithm SHA256).Hash
    }
    return $null
}

function Write-SnapshotJson {
    param(
        [Parameter(Mandatory = $true)]$Value,
        [Parameter(Mandatory = $true)][string]$Path,
        [switch]$AsArray
    )

    $json = if ($AsArray) {
        $Value | ConvertTo-Json -Depth 20 -AsArray
    } else {
        $Value | ConvertTo-Json -Depth 20
    }
    [System.IO.File]::WriteAllText(
        $Path,
        "$json`n",
        [System.Text.UTF8Encoding]::new($false)
    )
}

function Write-SnapshotJournal {
    param(
        [Parameter(Mandatory = $true)]$Journal,
        [Parameter(Mandatory = $true)][string]$Path
    )

    $temporaryPath = "$Path.tmp"
    Write-SnapshotJson -Value $Journal -Path $temporaryPath
    [System.IO.File]::Move($temporaryPath, $Path, $true)
}

function Assert-SnapshotRelativeTarget {
    param([Parameter(Mandatory = $true)][string]$Target)

    if ([string]::IsNullOrWhiteSpace($Target) -or
        [System.IO.Path]::IsPathRooted($Target) -or
        $Target.Contains("\") -or
        $Target -match "(^|/)\.\.(/|$)" -or
        $Target -notmatch "^(?:docs/api|versions\.json|humanizer-versions\.json|versioned_sidebars/version-\d+\.\d+\.\d+(?:\.\d+)?-sidebars\.json|versioned_docs/version-\d+\.\d+\.\d+(?:\.\d+)?(?:/(?!api(?:/|$)|.*(?:^|/)\.\.(?:/|$)).+)?)$") {
        throw "Snapshot transaction target is unsafe: $Target"
    }
}

function Assert-SnapshotJournalTargets {
    param([Parameter(Mandatory = $true)]$Journal)

    $targets = @($Journal.targets)
    if ($targets.Count -eq 0 -or
        $targets.Count -ne
            @($targets.target | Sort-Object -Unique).Count) {
        throw "Snapshot transaction journal must contain unique targets."
    }
    for ($index = 0; $index -lt $targets.Count; $index++) {
        $item = $targets[$index]
        Assert-SnapshotRelativeTarget -Target $item.target
        if ([int]$item.index -ne $index -or
            $item.staged -ne "staged/$index" -or
            $item.backup -ne "backups/$index") {
            throw "Snapshot transaction journal contains invalid target metadata."
        }
    }
    $manifestItems = @(
        $targets | Where-Object target -eq "humanizer-versions.json"
    )
    if ($manifestItems.Count -gt 1 -or
        ($manifestItems.Count -eq 1 -and
            [int]$manifestItems[0].index -ne $targets.Count - 1)) {
        throw "Snapshot transaction manifest target must be last."
    }
}

function Undo-SnapshotTransaction {
    param(
        [Parameter(Mandatory = $true)][string]$WebsiteRoot,
        [Parameter(Mandatory = $true)]$Journal,
        [Parameter(Mandatory = $true)][string]$TransactionRoot
    )

    Assert-SnapshotJournalTargets -Journal $Journal
    $failures = [System.Collections.Generic.List[string]]::new()
    foreach ($item in @($Journal.targets) |
        Sort-Object index -Descending) {
        Assert-SnapshotRelativeTarget -Target $item.target
        if (-not $item.installing -and -not $item.installed) {
            continue
        }
        try {
            $livePath = Join-Path $WebsiteRoot $item.target
            $liveDigest = Get-SnapshotArtifactDigest -Path $livePath
            if ($liveDigest -notin @(
                    $null,
                    $item.beforeDigest,
                    $item.afterDigest
                )) {
                throw "Live target changed outside the interrupted transaction."
            }
            $backupPath = $null
            if ($item.hadOriginal) {
                if (-not $item.backupReady) {
                    throw "Backup was not persisted."
                }
                $backupPath = Join-Path $TransactionRoot $item.backup
                if (-not (Test-Path $backupPath)) {
                    throw "Backup is missing: $($item.backup)"
                }
                if ((Get-SnapshotArtifactDigest -Path $backupPath) -ne
                    $item.beforeDigest) {
                    throw "Backup has an unexpected digest: $($item.backup)"
                }
            }
            if (Test-Path $livePath) {
                Remove-Item $livePath -Recurse -Force
            }
            if ($item.hadOriginal) {
                New-Item -ItemType Directory `
                    -Path (Split-Path $livePath -Parent) `
                    -Force | Out-Null
                Copy-Item $backupPath $livePath -Recurse
            }
            if ((Get-SnapshotArtifactDigest -Path $livePath) -ne
                $item.beforeDigest) {
                throw "Snapshot rollback produced an unexpected digest."
            }
        } catch {
            $failures.Add("$($item.target): $($_.Exception.Message)")
        }
    }
    if ($failures.Count -gt 0) {
        throw "Snapshot rollback failed; journal and backups were preserved. $($failures -join '; ')"
    }
}

function Repair-SnapshotTransaction {
    param([Parameter(Mandatory = $true)][string]$WebsiteRoot)

    $transactionRoot = Join-Path $WebsiteRoot ".snapshot-transaction"
    if (-not (Test-Path $transactionRoot -PathType Container)) {
        return
    }
    $journalPath = Join-Path $transactionRoot "journal.json"
    if (-not (Test-Path $journalPath -PathType Leaf)) {
        Remove-Item $transactionRoot -Recurse -Force
        Write-Host "Cleaned a pre-journal snapshot transaction."
        return
    }
    $journal = Get-Content -Raw $journalPath | ConvertFrom-Json -Depth 20
    if ($journal.schemaVersion -ne 1) {
        throw "Snapshot transaction journal has an unsupported schema."
    }
    Assert-SnapshotJournalTargets -Journal $journal

    if ($journal.state -eq "committed") {
        foreach ($item in @($journal.targets)) {
            $livePath = Join-Path $WebsiteRoot $item.target
            if ((Get-SnapshotArtifactDigest -Path $livePath) -ne
                $item.afterDigest) {
                throw "Committed snapshot transaction target changed; journal retained: $($item.target)"
            }
        }
        Remove-Item $transactionRoot -Recurse -Force
        Write-Host "Cleaned a committed snapshot transaction."
        return
    }
    if ($journal.state -notin @("preparing", "prepared", "applying")) {
        throw "Snapshot transaction journal has an invalid state."
    }

    Undo-SnapshotTransaction `
        -WebsiteRoot $WebsiteRoot `
        -Journal $journal `
        -TransactionRoot $transactionRoot
    Remove-Item $transactionRoot -Recurse -Force
    Write-Host "Recovered and rolled back a stale snapshot transaction."
}

function Enter-SnapshotMutation {
    param([Parameter(Mandatory = $true)][string]$WebsiteRoot)

    $lockPath = Join-Path $WebsiteRoot ".snapshot-mutation.lock"
    try {
        $stream = [System.IO.File]::Open(
            $lockPath,
            [System.IO.FileMode]::OpenOrCreate,
            [System.IO.FileAccess]::ReadWrite,
            [System.IO.FileShare]::None
        )
    } catch {
        throw "Another snapshot mutation owns the lock: $lockPath"
    }

    try {
        $stream.SetLength(0)
        $payload = [System.Text.Encoding]::UTF8.GetBytes(
            "$(@{ pid = $PID; startedUtc = [DateTime]::UtcNow.ToString('O') } | ConvertTo-Json -Compress)`n"
        )
        $stream.Write($payload, 0, $payload.Length)
        $stream.Flush($true)
        Repair-SnapshotTransaction -WebsiteRoot $WebsiteRoot
        return $stream
    } catch {
        $stream.Dispose()
        throw
    }
}

function Exit-SnapshotMutation {
    param([Parameter(Mandatory = $true)]$Lock)

    $Lock.Dispose()
}

function Invoke-SnapshotTransaction {
    param(
        [Parameter(Mandatory = $true)][string]$WebsiteRoot,
        [Parameter(Mandatory = $true)][object[]]$Changes,
        [scriptblock]$Validate
    )

    $transactionRoot = Join-Path $WebsiteRoot ".snapshot-transaction"
    if (Test-Path $transactionRoot) {
        throw "A snapshot transaction is already present: $transactionRoot"
    }
    New-Item -ItemType Directory -Path $transactionRoot | Out-Null
    $journalPath = Join-Path $transactionRoot "journal.json"
    $journalTargets = [System.Collections.Generic.List[object]]::new()

    try {
        for ($index = 0; $index -lt $Changes.Count; $index++) {
            $change = $Changes[$index]
            Assert-SnapshotRelativeTarget -Target $change.Target
            if (-not (Test-Path $change.Source)) {
                throw "Snapshot transaction source is missing: $($change.Source)"
            }
            $stagedRelative = "staged/$index"
            $backupRelative = "backups/$index"
            $journalTargets.Add([PSCustomObject]@{
                index = $index
                target = $change.Target
                staged = $stagedRelative
                backup = $backupRelative
                hadOriginal = (Test-Path (Join-Path $WebsiteRoot $change.Target))
                backupReady = $false
                installing = $false
                installed = $false
                beforeDigest = Get-SnapshotArtifactDigest -Path (
                    Join-Path $WebsiteRoot $change.Target
                )
                afterDigest = Get-SnapshotArtifactDigest -Path $change.Source
            })
        }
        $journal = [PSCustomObject]@{
            schemaVersion = 1
            state = "preparing"
            targets = @($journalTargets)
        }
        Assert-SnapshotJournalTargets -Journal $journal
        Write-SnapshotJournal -Journal $journal -Path $journalPath

        foreach ($item in @($journal.targets)) {
            $change = $Changes[$item.index]
            $stagedPath = Join-Path $transactionRoot $item.staged
            New-Item -ItemType Directory `
                -Path (Split-Path $stagedPath -Parent) `
                -Force | Out-Null
            Copy-Item $change.Source $stagedPath -Recurse
            if ($item.hadOriginal) {
                $backupPath = Join-Path $transactionRoot $item.backup
                New-Item -ItemType Directory `
                    -Path (Split-Path $backupPath -Parent) `
                    -Force | Out-Null
                Copy-Item (
                    Join-Path $WebsiteRoot $item.target
                ) $backupPath -Recurse
                $item.backupReady = $true
                Write-SnapshotJournal -Journal $journal -Path $journalPath
            }
        }
        $journal.state = "prepared"
        Write-SnapshotJournal -Journal $journal -Path $journalPath

        foreach ($item in @($journal.targets)) {
            $journal.state = "applying"
            $item.installing = $true
            Write-SnapshotJournal -Journal $journal -Path $journalPath
            $livePath = Join-Path $WebsiteRoot $item.target
            $currentExists = Test-Path $livePath
            $currentDigest = Get-SnapshotArtifactDigest -Path $livePath
            if ($currentExists -ne [bool]$item.hadOriginal -or
                $currentDigest -ne $item.beforeDigest) {
                throw "Snapshot transaction target changed after preparation: $($item.target)"
            }
            if (Test-Path $livePath) {
                Remove-Item $livePath -Recurse -Force
            }
            New-Item -ItemType Directory `
                -Path (Split-Path $livePath -Parent) `
                -Force | Out-Null
            Move-Item (
                Join-Path $transactionRoot $item.staged
            ) $livePath
            $item.installing = $false
            $item.installed = $true
            Write-SnapshotJournal -Journal $journal -Path $journalPath
        }

        if ($Validate) {
            & $Validate
        }
        foreach ($item in @($journal.targets)) {
            $livePath = Join-Path $WebsiteRoot $item.target
            if ((Get-SnapshotArtifactDigest -Path $livePath) -ne
                $item.afterDigest) {
                throw "Snapshot transaction produced an unexpected digest: $($item.target)"
            }
        }
        $journal.state = "committed"
        Write-SnapshotJournal -Journal $journal -Path $journalPath
        Remove-Item $transactionRoot -Recurse -Force
    } catch {
        $originalError = $_
        try {
            if (Test-Path $journalPath -PathType Leaf) {
                $rollbackJournal = Get-Content -Raw $journalPath |
                    ConvertFrom-Json -Depth 20
                Undo-SnapshotTransaction `
                    -WebsiteRoot $WebsiteRoot `
                    -Journal $rollbackJournal `
                    -TransactionRoot $transactionRoot
                Remove-Item $transactionRoot -Recurse -Force
            } elseif (Test-Path $transactionRoot -PathType Container) {
                Remove-Item $transactionRoot -Recurse -Force
            }
        } catch {
            throw "$($originalError.Exception.Message) $($_.Exception.Message)"
        }
        throw $originalError
    }
}
