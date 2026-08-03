param(
    [switch]$RequireNativeAot,
    [string]$ReleaseVersion
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
$manifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
. (Join-Path $PSScriptRoot "snapshot-state.ps1")
. (Join-Path $PSScriptRoot "api-approval.ps1")
. (Join-Path $PSScriptRoot "nuget-package.ps1")
. (Join-Path $PSScriptRoot "api-reference.ps1")

function Assert-ApiReferenceArchitecture {
    $verifyPath = Join-Path $PSScriptRoot "verify-api.ps1"
    $verifyLines = [System.IO.File]::ReadAllLines($verifyPath)
    $verifySource = $verifyLines -join "`n"
    if ($verifyLines.Count -ge 1000) {
        throw "verify-api.ps1 must remain a verifier/orchestrator below 1,000 lines."
    }

    $assemblyParses = [regex]::Matches(
        $verifySource,
        "\bGet-AssemblyApiMemberInventory\b"
    ).Count
    if ($assemblyParses -ne 1) {
        throw "verify-api.ps1 must derive the assembly API inventory exactly once."
    }

    $pipelineCalls = [regex]::Matches(
        $verifySource,
        "\bInvoke-ApiReferenceGeneration\b"
    ).Count
    if ($pipelineCalls -ne 3) {
        throw "verify-api.ps1 must use one shared API generation pipeline for its three outputs."
    }

    foreach ($stage in @(
        "Add-ImplicitApiConstructors",
        "Set-ApiMarkdownStructure",
        "Set-ApiTypeMetadata",
        "Assert-GeneratedApiMemberCompleteness",
        "Assert-ApiHeadingOwnership"
    )) {
        if ($verifySource -match "(?m)^function $stage\b") {
            throw "$stage belongs in the shared API reference module."
        }
    }
}

Assert-ApiReferenceArchitecture

function New-ApiShapeInput {
    param(
        [Parameter(Mandatory = $true)][string]$Root,
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][string]$Source,
        [string[]]$CompilerOptions = @()
    )

    $dllPath = Join-Path $Root "$Name.dll"
    $arguments = @{
        Language = "CSharp"
        OutputAssembly = $dllPath
        TypeDefinition = $Source
    }
    if ($CompilerOptions.Count -gt 0) {
        $arguments.CompilerOptions = $CompilerOptions
    }
    Add-Type @arguments
    $xmlPath = Join-Path $Root "$Name.xml"
    [System.IO.File]::WriteAllText(
        $xmlPath,
        "<doc><assembly><name>$Name</name></assembly><members /></doc>`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    return [PSCustomObject]@{
        Dll = $dllPath
        Xml = $xmlPath
    }
}

function New-CheckedConversionApiInput {
    param([Parameter(Mandatory = $true)][string]$Root)

    $name = "CheckedConversionFixture"
    $projectRoot = Join-Path $Root $name
    New-Item -ItemType Directory -Path $projectRoot | Out-Null
    $projectPath = Join-Path $projectRoot "$name.csproj"
    [System.IO.File]::WriteAllText(
        $projectPath,
        @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>CS1591</NoWarn>
  </PropertyGroup>
</Project>
"@,
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        (Join-Path $projectRoot "Source.cs"),
        @"
namespace CheckedConversionFixture;

public readonly struct CheckedToken
{
    public static explicit operator CheckedToken(int value) => default;
    /// <summary>Converts an integer in a checked context.</summary>
    public static explicit operator checked CheckedToken(int value) => default;
}

public class Generic<T> { }
"@,
        [System.Text.UTF8Encoding]::new($false)
    )
    Invoke-DocsCheckedCommand `
        -FilePath "dotnet" `
        -ArgumentList @(
            "build", $projectPath,
            "--configuration", "Release",
            "--nologo",
            "--verbosity", "quiet"
        ) `
        -WorkingDirectory $Root
    $outputRoot = Join-Path $projectRoot "bin/Release/net8.0"
    return [PSCustomObject]@{
        Dll = Join-Path $outputRoot "$name.dll"
        Xml = Join-Path $outputRoot "$name.xml"
    }
}

function Assert-ApiPageCollision {
    param(
        [Parameter(Mandatory = $true)][string]$Root,
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][string]$Source,
        [Parameter(Mandatory = $true)][string]$ExpectedPage,
        [Parameter(Mandatory = $true)][string[]]$ExpectedTypeIds
    )

    $apiInput = New-ApiShapeInput `
        -Root $Root `
        -Name $Name `
        -Source $Source
    $collisionFailed = $false
    try {
        Get-AssemblyApiMemberInventory `
            -AssemblyPath $apiInput.Dll | Out-Null
    } catch {
        $collisionMessage = $_.Exception.Message
        $collisionFailed = $collisionMessage.Contains(
            "canonical page $ExpectedPage",
            [System.StringComparison]::Ordinal
        )
        foreach ($typeId in $ExpectedTypeIds) {
            $collisionFailed = $collisionFailed -and
                $collisionMessage.Contains(
                    $typeId,
                    [System.StringComparison]::Ordinal
                )
        }
    }
    if (-not $collisionFailed) {
        throw "A canonical API page collision did not fail closed."
    }
}

function Assert-AssemblyApiShapes {
    $fixtureRoot = Join-Path (
        [System.IO.Path]::GetTempPath()
    ) "humanizer-assembly-api-shapes-$([guid]::NewGuid().ToString('N'))"
    New-Item -ItemType Directory -Path $fixtureRoot | Out-Null
    try {
        Assert-ApiPageCollision `
            -Root $fixtureRoot `
            -Name "CollisionFixture" `
            -ExpectedPage "CollisionFixture.Same_T_" `
            -ExpectedTypeIds @(
                'T:CollisionFixture.Same`1',
                "T:CollisionFixture.Same_T_"
            ) `
            -Source @"
namespace CollisionFixture;

public class Same<T> { }
public class Same_T_ { }
"@
        Assert-ApiPageCollision `
            -Root $fixtureRoot `
            -Name "CaseCollisionFixture" `
            -ExpectedPage "CaseCollisionFixture.Same" `
            -ExpectedTypeIds @(
                "T:CaseCollisionFixture.Same",
                "T:CaseCollisionFixture.same"
            ) `
            -Source @"
namespace CaseCollisionFixture;

public class Same { }
public class same { }
"@

        $checkedInput = New-CheckedConversionApiInput -Root $fixtureRoot
        $checkedConversionId = (
            "M:CheckedConversionFixture.CheckedToken." +
            "op_CheckedExplicit(System.Int32)" +
            "~CheckedConversionFixture.CheckedToken"
        )
        $compilerDocumentation = [xml](Get-Content -Raw $checkedInput.Xml)
        $compilerDocumentationIds = @(
            $compilerDocumentation.doc.members.member |
                ForEach-Object name
        )
        if ($checkedConversionId -notin $compilerDocumentationIds) {
            throw "The compiler omitted the checked conversion return type."
        }
        $checkedInventory = Get-AssemblyApiMemberInventory `
            -AssemblyPath $checkedInput.Dll
        if ($checkedConversionId -notin @($checkedInventory.Records.Id)) {
            throw "The assembly inventory omitted the checked conversion return type."
        }
        $originalCheckedXml = Get-Content -Raw $checkedInput.Xml
        $checkedOutput = Join-Path $fixtureRoot "checked"
        $checkedLinks = Join-Path $fixtureRoot "checked-links.txt"
        New-Item -ItemType Directory -Path $checkedOutput | Out-Null
        Invoke-ApiReferenceGeneration `
            -ApiInput $checkedInput `
            -OutputPath $checkedOutput `
            -LinksPath $checkedLinks `
            -AssemblyInventory $checkedInventory `
            -RepositoryRoot $repoRoot `
            -VersionLabel "checked conversion fixture" `
            -ConfigurationFilePath (Join-Path $PSScriptRoot "api-reference-v4.json")
        if ($checkedConversionId -notin @(
            (Get-ApiLinkRecords -LinksPath $checkedLinks).Id
        )) {
            throw "Generated API links omitted the checked conversion return type."
        }
        $checkedPage = Get-Content -Raw (
            Join-Path $checkedOutput "CheckedConversionFixture.CheckedToken.md"
        )
        if (-not $checkedPage.Contains(
                "Converts an integer in a checked context",
                [System.StringComparison]::Ordinal
            )) {
            throw "Generated API omitted the checked conversion summary."
        }
        if ((Get-Content -Raw $checkedInput.Xml) -ne $originalCheckedXml) {
            throw "Checked conversion generation mutated the compiler XML."
        }

        $checkedConversionAlias = $checkedConversionId.Substring(
            0,
            $checkedConversionId.LastIndexOf("~")
        )
        $conflictingXml = Join-Path $fixtureRoot "checked-conflict.xml"
        $conflictingDocument = [System.Xml.XmlDocument]::new()
        $conflictingDocument.PreserveWhitespace = $true
        $conflictingDocument.Load($checkedInput.Xml)
        $canonicalNodes = @(
            $conflictingDocument.SelectNodes('/doc/members/member') |
                Where-Object {
                    [string]::Equals(
                        $_.GetAttribute("name"),
                        $checkedConversionId,
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($canonicalNodes.Count -ne 1) {
            throw "The checked conversion fixture has no unique canonical XML member."
        }
        $canonicalNode = $canonicalNodes[0]
        $conflictingNode = $canonicalNode.CloneNode($true)
        $conflictingNode.SetAttribute("name", $checkedConversionAlias)
        $conflictingMembers = $conflictingDocument.SelectSingleNode('/doc/members')
        [void]$conflictingMembers.AppendChild($conflictingNode)
        [void]$conflictingMembers.AppendChild(
            $conflictingDocument.CreateWhitespace("`n    ")
        )
        $conflictingDocument.Save($conflictingXml)
        $conflictFailed = $false
        try {
            New-DefaultDocumentationApiInput `
                -ApiInput ([PSCustomObject]@{
                    Dll = $checkedInput.Dll
                    Xml = $conflictingXml
                }) `
                -ExpectedRecords $checkedInventory.Records | Out-Null
        } catch {
            $conflictFailed = $_.Exception.Message.Contains(
                "Checked conversion XML alias already exists: $checkedConversionAlias",
                [System.StringComparison]::Ordinal
            )
        }
        if (-not $conflictFailed) {
            throw "A conflicting checked conversion XML alias did not fail closed."
        }

        $ambiguousFailed = $false
        try {
            New-DefaultDocumentationApiInput `
                -ApiInput $checkedInput `
                -ExpectedRecords @(
                    [PSCustomObject]@{ Id = $checkedConversionId },
                    [PSCustomObject]@{
                        Id = "$checkedConversionAlias~System.Object"
                    }
                ) | Out-Null
        } catch {
            $ambiguousFailed = $_.Exception.Message.Contains(
                "matched 2 assembly-derived IDs",
                [System.StringComparison]::Ordinal
            )
        }
        if (-not $ambiguousFailed) {
            throw "An ambiguous checked conversion XML alias did not fail closed."
        }

        $uncheckedOutput = Join-Path $fixtureRoot "checked-without-config"
        $uncheckedLinks = Join-Path $fixtureRoot "checked-without-config-links.txt"
        New-Item -ItemType Directory -Path $uncheckedOutput | Out-Null
        Invoke-ApiReferenceGeneration `
            -ApiInput $checkedInput `
            -OutputPath $uncheckedOutput `
            -LinksPath $uncheckedLinks `
            -AssemblyInventory $checkedInventory `
            -RepositoryRoot $repoRoot `
            -VersionLabel "checked conversion fixture without configuration"
        $uncheckedPage = Get-Content -Raw (
            Join-Path $uncheckedOutput "CheckedConversionFixture.CheckedToken.md"
        )
        if (-not $uncheckedPage.Contains(
                "Converts an integer in a checked context",
                [System.StringComparison]::Ordinal
            )) {
            throw "Unconfigured API generation omitted the checked conversion summary."
        }

        $apiInput = New-ApiShapeInput `
            -Root $fixtureRoot `
            -Name "Fixture" `
            -Source @"
namespace Fixture;

public delegate string Formatter(string input);
public delegate TResult Converter<in TInput, out TResult>(TInput value);

public readonly struct Token
{
    public static explicit operator Token(string value) => default;
    public static implicit operator string(Token value) => "";
}

public interface IContract
{
    string Name { get; }
    string this[int index] { get; }
    void Run();
}

public sealed class Implementation : IContract
{
    string IContract.Name => "";
    string IContract.this[int index] => "";
    void IContract.Run() { }
}

public interface IGenericContract<T>
{
    string Name { get; }
    void Run(T value);
}

public sealed class GenericImplementation : IGenericContract<int>
{
    string IGenericContract<int>.Name => "";
    void IGenericContract<int>.Run(int value) { }
}

internal interface IHiddenGenericContract<T>
{
    void Run(T value);
}

public sealed class HiddenGenericImplementation : IHiddenGenericContract<int>
{
    void IHiddenGenericContract<int>.Run(int value) { }
}

public class GenericContainer<TOuter>
{
    public class Nested { }
    public class Inner<TInner> { }
    public delegate TResult Project<TResult>(TOuter input);
}

public static class NestedGenericConsumer
{
    public static void Use(GenericContainer<int>.Inner<string> value) { }
}

public interface IAccessShapes
{
    protected void ProtectedDefault() { }
    void PublicDefault() { }
    static abstract string Transform(int value);
}

public class Container
{
    protected class Hidden
    {
        public class Nested
        {
            public void Run() { }
        }
    }
}

public class AccessorLikeMethods
{
    public void get_Value() { }
    public void set_Value(int value) { }
    public void add_Changed(System.Action value) { }
    public void remove_Changed(System.Action value) { }
    public void raise_Changed() { }
}
"@
        $inventory = Get-AssemblyApiMemberInventory `
            -AssemblyPath $apiInput.Dll
        $records = @{}
        foreach ($record in $inventory.Records) {
            $records[$record.Id] = $record
        }
        $expectedRecords = [ordered]@{
            "T:Fixture.Formatter" = @("public", "Type", "Fixture.Formatter")
            "T:Fixture.Converter``2" = @("public", "Type", "Fixture.Converter_TInput_TResult_")
            "M:Fixture.Token.op_Explicit(System.String)~Fixture.Token" = @("public", "Operator", "Fixture.Token")
            "M:Fixture.Token.op_Implicit(Fixture.Token)~System.String" = @("public", "Operator", "Fixture.Token")
            "M:Fixture.IAccessShapes.ProtectedDefault" = @("protected", "Method", "Fixture.IAccessShapes")
            "M:Fixture.IAccessShapes.PublicDefault" = @("public", "Method", "Fixture.IAccessShapes")
            "M:Fixture.IAccessShapes.Transform(System.Int32)" = @("public", "Method", "Fixture.IAccessShapes")
            "P:Fixture.GenericImplementation.Fixture#IGenericContract{System#Int32}#Name" = @("public", "Property", "Fixture.GenericImplementation")
            "M:Fixture.GenericImplementation.Fixture#IGenericContract{System#Int32}#Run(System.Int32)" = @("public", "Method", "Fixture.GenericImplementation")
            "T:Fixture.GenericContainer``1.Nested" = @("public", "Type", "Fixture.GenericContainer_TOuter_.Nested")
            "T:Fixture.GenericContainer``1.Inner``1" = @("public", "Type", "Fixture.GenericContainer_TOuter_.Inner_TInner_")
            "T:Fixture.GenericContainer``1.Project``1" = @("public", "Type", "Fixture.GenericContainer_TOuter_.Project_TResult_")
            "M:Fixture.NestedGenericConsumer.Use(Fixture.GenericContainer{System.Int32}.Inner{System.String})" = @("public", "Method", "Fixture.NestedGenericConsumer")
            "T:Fixture.Container.Hidden" = @("protected", "Type", "Fixture.Container.Hidden")
            "T:Fixture.Container.Hidden.Nested" = @("protected", "Type", "Fixture.Container.Hidden.Nested")
            "M:Fixture.Container.Hidden.Nested.Run" = @("protected", "Method", "Fixture.Container.Hidden.Nested")
            "M:Fixture.AccessorLikeMethods.get_Value" = @("public", "Method", "Fixture.AccessorLikeMethods")
            "M:Fixture.AccessorLikeMethods.set_Value(System.Int32)" = @("public", "Method", "Fixture.AccessorLikeMethods")
            "M:Fixture.AccessorLikeMethods.add_Changed(System.Action)" = @("public", "Method", "Fixture.AccessorLikeMethods")
            "M:Fixture.AccessorLikeMethods.remove_Changed(System.Action)" = @("public", "Method", "Fixture.AccessorLikeMethods")
            "M:Fixture.AccessorLikeMethods.raise_Changed" = @("public", "Method", "Fixture.AccessorLikeMethods")
            "P:Fixture.Implementation.Fixture#IContract#Name" = @("public", "Property", "Fixture.Implementation")
            "P:Fixture.Implementation.Fixture#IContract#Item(System.Int32)" = @("public", "Property", "Fixture.Implementation")
            "M:Fixture.Implementation.Fixture#IContract#Run" = @("public", "Method", "Fixture.Implementation")
        }
        foreach ($id in $expectedRecords.Keys) {
            if (-not $records.ContainsKey($id)) {
                throw "The assembly inventory omitted $id."
            }
            $expected = $expectedRecords[$id]
            if ($records[$id].Access -ne $expected[0] -or
                $records[$id].Kind -ne $expected[1] -or
                $records[$id].PageName -ne $expected[2]) {
                throw "The assembly inventory misclassified $id."
            }
        }
        $hiddenImplementationRecords = @(
            $inventory.Records |
                Where-Object {
                    $_.TypeId -eq "Fixture.HiddenGenericImplementation" -and
                    $_.Kind -notin @("Type", "Constructor")
                }
        )
        if ($hiddenImplementationRecords.Count -ne 0) {
            throw "The assembly inventory exposed an inaccessible generic interface."
        }

        $apiOutput = Join-Path $fixtureRoot "api"
        $publicOutput = Join-Path $fixtureRoot "public"
        $apiLinks = Join-Path $fixtureRoot "api-links.txt"
        $publicLinks = Join-Path $fixtureRoot "public-links.txt"
        New-Item -ItemType Directory -Path $apiOutput | Out-Null
        New-Item -ItemType Directory -Path $publicOutput | Out-Null
        $configurationPath = Join-Path $PSScriptRoot "api-reference-v4.json"
        Invoke-ApiReferenceGeneration `
            -ApiInput $apiInput `
            -OutputPath $apiOutput `
            -LinksPath $apiLinks `
            -AssemblyInventory $inventory `
            -RepositoryRoot $repoRoot `
            -VersionLabel "assembly fixture" `
            -ConfigurationFilePath $configurationPath
        Invoke-ApiReferenceGeneration `
            -ApiInput $apiInput `
            -OutputPath $publicOutput `
            -LinksPath $publicLinks `
            -AssemblyInventory $inventory `
            -RepositoryRoot $repoRoot `
            -VersionLabel "public assembly fixture" `
            -ConfigurationFilePath $configurationPath `
            -AccessModifiers "Public"
        Assert-GeneratedApiLinks `
            -OutputPath $apiOutput `
            -LinksPath $apiLinks `
            -VersionLabel "assembly fixture"
        Assert-GeneratedApiLinks `
            -OutputPath $publicOutput `
            -LinksPath $publicLinks `
            -VersionLabel "public assembly fixture"
        Assert-ApiAccessCoverage `
            -ApiLinksPath $apiLinks `
            -PublicLinksPath $publicLinks `
            -AssemblyInventory $inventory `
            -VersionLabel "assembly fixture"

        $formatterPage = Get-Content -Raw (
            Join-Path $apiOutput "Fixture.Formatter.md"
        )
        $converterPage = Get-Content -Raw (
            Join-Path $apiOutput "Fixture.Converter_TInput_TResult_.md"
        )
        $nestedPage = Get-Content -Raw (
            Join-Path $apiOutput "Fixture.GenericContainer_TOuter_.Nested.md"
        )
        $innerPage = Get-Content -Raw (
            Join-Path $apiOutput "Fixture.GenericContainer_TOuter_.Inner_TInner_.md"
        )
        $projectPage = Get-Content -Raw (
            Join-Path $apiOutput "Fixture.GenericContainer_TOuter_.Project_TResult_.md"
        )
        $namespacePage = Get-Content -Raw (Join-Path $apiOutput "Fixture.md")
        $identityChecks = [ordered]@{
            "formatter title" = $formatterPage.StartsWith(
                "---`ntitle: 'Fixture.Formatter'",
                [System.StringComparison]::Ordinal
            )
            "formatter heading" = $formatterPage -match (
                "(?m)^## Formatter Delegate$"
            )
            "generic delegate title" = $converterPage.StartsWith(
                "---`ntitle: 'Fixture.Converter<TInput,TResult>'",
                [System.StringComparison]::Ordinal
            )
            "generic delegate heading" = $converterPage -match (
                "(?m)^## Converter\\<TInput,TResult\\> Delegate$"
            )
            "nested title" = $nestedPage.StartsWith(
                "---`ntitle: 'Fixture.GenericContainer<TOuter>.Nested'",
                [System.StringComparison]::Ordinal
            )
            "nested generic title" = $innerPage.StartsWith(
                "---`ntitle: 'Fixture.GenericContainer<TOuter>.Inner<TInner>'",
                [System.StringComparison]::Ordinal
            )
            "nested delegate title" = $projectPage.StartsWith(
                "---`ntitle: 'Fixture.GenericContainer<TOuter>.Project<TResult>'",
                [System.StringComparison]::Ordinal
            )
            "nested delegate heading" = $projectPage -match (
                "(?m)^## GenericContainer\\<TOuter\\>\.Project\\<TResult\\> Delegate$"
            )
            "delegate namespace entry" = $namespacePage.Contains(
                "[Formatter](Fixture.Formatter.md",
                [System.StringComparison]::Ordinal
            )
            "generic delegate namespace entry" = $namespacePage.Contains(
                "[Converter&lt;TInput,TResult&gt;](Fixture.Converter_TInput_TResult_.md",
                [System.StringComparison]::Ordinal
            )
            "nested delegate namespace entry" = $namespacePage.Contains(
                "[GenericContainer&lt;TOuter&gt;.Project&lt;TResult&gt;](Fixture.GenericContainer_TOuter_.Project_TResult_.md",
                [System.StringComparison]::Ordinal
            )
        }
        foreach ($identityCheck in $identityChecks.GetEnumerator()) {
            if (-not $identityCheck.Value) {
                throw "The API generator did not canonicalize the $($identityCheck.Key)."
            }
        }

        $publicIds = @(
            Get-ApiLinkRecords -LinksPath $publicLinks |
                ForEach-Object Id
        )
        foreach ($protectedId in @(
            "M:Fixture.IAccessShapes.ProtectedDefault",
            "T:Fixture.Container.Hidden",
            "T:Fixture.Container.Hidden.Nested",
            "M:Fixture.Container.Hidden.Nested.Run"
        )) {
            if ($protectedId -in $publicIds) {
                throw "Public API generation included protected ID $protectedId."
            }
        }

        $originalLinks = Get-Content -Raw $apiLinks
        $mutatedLinks = $originalLinks.Replace(
            "M:Fixture.IAccessShapes.PublicDefault|",
            "M:Fixture.IAccessShapes.ReplacedDefault|",
            [System.StringComparison]::Ordinal
        )
        if ($mutatedLinks -eq $originalLinks) {
            throw "The exact-member fixture could not replace its method ID."
        }
        [System.IO.File]::WriteAllText(
            $apiLinks,
            $mutatedLinks,
            [System.Text.UTF8Encoding]::new($false)
        )
        $wrongIdFailed = $false
        try {
            Assert-GeneratedApiMemberCompleteness `
                -ExpectedRecords (Select-AssemblyApiRecords $inventory) `
                -LinksPath $apiLinks `
                -VersionLabel "same-kind mutation"
        } catch {
            $wrongIdFailed = $_.Exception.Message -match (
                "missing M:Fixture\.IAccessShapes\.PublicDefault"
            ) -and $_.Exception.Message -match (
                "unexpected M:Fixture\.IAccessShapes\.ReplacedDefault"
            )
        }
        if (-not $wrongIdFailed) {
            throw "A wrong same-kind API member ID unexpectedly passed."
        }

        $eventInput = New-ApiShapeInput `
            -Root $fixtureRoot `
            -Name "EventFixture" `
            -Source @"
namespace EventFixture;

public interface IEvents
{
    event System.EventHandler Changed;
}

public sealed class Implementation : IEvents
{
    event System.EventHandler IEvents.Changed { add { } remove { } }
}
"@
        $eventInventory = Get-AssemblyApiMemberInventory `
            -AssemblyPath $eventInput.Dll
        $eventOutput = Join-Path $fixtureRoot "event"
        $eventLinks = Join-Path $fixtureRoot "event-links.txt"
        New-Item -ItemType Directory -Path $eventOutput | Out-Null
        $eventFailed = $false
        try {
            Invoke-ApiReferenceGeneration `
                -ApiInput $eventInput `
                -OutputPath $eventOutput `
                -LinksPath $eventLinks `
                -AssemblyInventory $eventInventory `
                -RepositoryRoot $repoRoot `
                -VersionLabel "explicit event fixture" `
                -ConfigurationFilePath $configurationPath
        } catch {
            $eventFailed = $_.Exception.Message -match (
                "missing E:EventFixture\.Implementation\.EventFixture#IEvents#Changed"
            )
        }
        if (-not $eventFailed) {
            throw "DefaultDocumentation's explicit-event blind spot did not fail closed."
        }

        $pointerInput = New-ApiShapeInput `
            -Root $fixtureRoot `
            -Name "PointerFixture" `
            -CompilerOptions "/unsafe" `
            -Source @"
namespace PointerFixture;

public static unsafe class PointerShape
{
    public static int Invoke(
        delegate* unmanaged[Cdecl]<int, int> callback,
        int value) => callback(value);
}
"@
        $pointerFailed = $false
        try {
            Get-AssemblyApiMemberInventory -AssemblyPath $pointerInput.Dll
        } catch {
            $pointerFailed = $_.Exception.Message -match (
                "function-pointer documentation IDs are not supported"
            )
        }
        if (-not $pointerFailed) {
            throw "An unsupported function-pointer API did not fail closed."
        }
    } finally {
        Remove-Item $fixtureRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Assert-AssemblyApiShapes

function Assert-VersionedExampleDefaults {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$ExamplesRoot
    )

    [xml]$props = Get-Content -Raw (
        Join-Path $ExamplesRoot "Directory.Build.props"
    )
    $groups = @(
        $props.SelectNodes(
            "/Project/PropertyGroup[@Label='HumanizerDocumentationSnapshot']"
        )
    )
    if ($groups.Count -ne 1) {
        throw "Version $($Entry.version) must declare one example-default group."
    }

    $versionNodes = @($groups[0].SelectNodes("HumanizerPackageVersion"))
    $expectedVersionCondition =
        "'`$(HumanizerProject)' == '' and " +
        "'`$(HumanizerPackageVersion)' == ''"
    if ($versionNodes.Count -ne 1 -or
        $versionNodes[0].InnerText -ne $Entry.source.packageVersion -or
        $versionNodes[0].GetAttribute("Condition") -ne
            $expectedVersionCondition) {
        throw "Version $($Entry.version) does not default to its exact package."
    }

    $excludedAssetsProperty = $Entry.PSObject.Properties[
        "exampleExcludedAssets"
    ]
    $expectedExcludedAssets = if ($null -eq $excludedAssetsProperty) {
        ""
    } else {
        @($excludedAssetsProperty.Value) -join ";"
    }
    $excludedAssetNodes = @(
        $groups[0].SelectNodes("HumanizerExampleExcludeAssets")
    )
    if (($expectedExcludedAssets -eq "" -and
            $excludedAssetNodes.Count -ne 0) -or
        ($expectedExcludedAssets -ne "" -and
            ($excludedAssetNodes.Count -ne 1 -or
                $excludedAssetNodes[0].InnerText -ne
                    $expectedExcludedAssets -or
                $excludedAssetNodes[0].GetAttribute("Condition") -ne
                    ("'`$(HumanizerProject)' == '' and " +
                        "'`$(HumanizerPackageVersion)' == " +
                        "'$($Entry.source.packageVersion)' and " +
                        "'`$(HumanizerExampleExcludeAssets)' == ''")))) {
        throw "Version $($Entry.version) has incorrect example asset defaults."
    }
}

$fingerprintTempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-fingerprint-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $fingerprintTempRoot | Out-Null
try {
    $staleReferencePath = Join-Path $fingerprintTempRoot "locale-yaml-reference.mdx"
    $referenceText = Get-Content -Raw (
        Join-Path $repoRoot "website/docs/contributing/locale-yaml-reference.mdx"
    )
    $staleReferenceText = [regex]::Replace(
        $referenceText,
        "(?m)^(src/Humanizer\.SourceGenerators/Common/CanonicalLocaleAuthoring\.cs )[0-9a-f]{64}$",
        ('${1}' + ('0' * 64))
    )
    [System.IO.File]::WriteAllText(
        $staleReferencePath,
        $staleReferenceText,
        [System.Text.UTF8Encoding]::new($false)
    )
    $fingerprintOutput = & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "generate-language-coverage.ps1"
    ) `
        -Check `
        -ReferencePath $staleReferencePath `
        -ProjectPath (Join-Path $fingerprintTempRoot "must-not-build.csproj") 2>&1 |
        Out-String
    if ($LASTEXITCODE -eq 0) {
        throw "Stale locale reference fingerprint unexpectedly passed."
    }
    if ($fingerprintOutput -notmatch "CanonicalLocaleAuthoring\.cs" -or
        $fingerprintOutput -notmatch "Re-review") {
        throw "Stale locale reference fingerprint did not fail with the required review guidance."
    }
} finally {
    Remove-Item $fingerprintTempRoot -Recurse -Force
}

& (Join-Path $PSScriptRoot "verify-manifest.ps1")
$fixtureManifest = Get-Content -Raw $manifestPath |
    ConvertFrom-Json -Depth 20
$fixtureLatest = @($fixtureManifest.versions | Where-Object latestStable)
$candidateVersion = if ([string]::IsNullOrWhiteSpace($ReleaseVersion)) {
    if ($fixtureLatest.Count -ne 1) {
        throw "The release fixture requires exactly one latest stable version."
    }
    [string]$fixtureLatest[0].version
} else {
    $ReleaseVersion
}
$candidateEntries = @(
    $fixtureManifest.versions |
        Where-Object version -eq $candidateVersion
)
$priorPublishedVersions = @(
    $fixtureManifest.versions |
        Where-Object {
            $_.version -ne "current" -and
            $_.published -and
            $_.version -ne $candidateVersion
        } |
        Sort-Object {
            [System.Management.Automation.SemanticVersion]::Parse(
                $_.version
            )
        } -Descending |
        ForEach-Object version
)
if ($candidateEntries.Count -ne 1 -or
    -not $candidateEntries[0].published -or
    $priorPublishedVersions.Count -eq 0) {
    throw "The release fixture requires one published candidate and at least one prior published version."
}
$candidateEntry = $candidateEntries[0]
$candidateSemanticVersion =
    [System.Management.Automation.SemanticVersion]::Parse($candidateVersion)
$previousLatestEntry = if ($candidateEntry.latestStable) {
    @(
        $fixtureManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.published -and
                $_.version -ne $candidateVersion -and
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                ) -lt $candidateSemanticVersion
            } |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending
    )[0]
} else {
    $fixtureLatest[0]
}
if ($null -eq $previousLatestEntry) {
    throw "The release fixture requires a default predecessor."
}
$previousLatestVersion = [string]$previousLatestEntry.version
$canPromoteCandidate =
    $candidateSemanticVersion -gt
        [System.Management.Automation.SemanticVersion]::Parse(
            $previousLatestVersion
        )

foreach ($publishedEntry in @(
    $fixtureManifest.versions |
        Where-Object { $_.version -ne "current" -and $_.published }
)) {
    Assert-VersionedExampleDefaults `
        -Entry $publishedEntry `
        -ExamplesRoot (Join-Path $repoRoot (
            "website/versioned_docs/version-$($publishedEntry.version)/_examples"
        ))
}

& (Join-Path $PSScriptRoot "build.ps1") `
    -Version $candidateVersion `
    -ValidateOnly
$aotParameters = @{
    RequireNativeAot = $RequireNativeAot
}
if (-not [string]::IsNullOrWhiteSpace($ReleaseVersion)) {
    $aotParameters.Version = @($ReleaseVersion, "current")
}
& (Join-Path $PSScriptRoot "verify-aot.ps1") @aotParameters

$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $tempRoot | Out-Null
try {
    $currentRestoreOutput = & dotnet restore (
        Join-Path $repoRoot "website/docs/_examples/quick-start/QuickStart.csproj"
    ) `
        --artifacts-path (Join-Path $tempRoot "current-without-input") `
        --nologo 2>&1 | Out-String
    if ($LASTEXITCODE -eq 0 -or
        $currentRestoreOutput -notmatch
            "Set HumanizerProject or HumanizerPackageVersion") {
        throw "Canonical current examples did not require an explicit input."
    }

    $invalidManifestPath = Join-Path $tempRoot "invalid-manifest.json"
    '{"schemaVersion":1,"versions":[]}' | Set-Content $invalidManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "build.ps1") -ValidateOnly -ManifestPath $invalidManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Empty manifest validation unexpectedly passed."
    }

    $duplicateRouteManifestPath = Join-Path $tempRoot "duplicate-route-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    ($manifest.versions | Where-Object version -eq "2.10.1").route = "2.11.10"
    $manifest | ConvertTo-Json -Depth 20 | Set-Content $duplicateRouteManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $duplicateRouteManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Duplicate route validation unexpectedly passed."
    }

    $missingPreviewManifestPath = Join-Path $tempRoot "missing-preview-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    $manifest.versions = @(
        $manifest.versions |
            Where-Object version -ne "current"
    )
    $manifest | ConvertTo-Json -Depth 20 |
        Set-Content $missingPreviewManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $missingPreviewManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Missing preview manifest validation unexpectedly passed."
    }

    $missingTfmManifestPath = Join-Path $tempRoot "missing-tfm-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    ($manifest.versions | Where-Object version -eq "2.10.1").referenceTfm = "net-missing"
    $manifest | ConvertTo-Json -Depth 20 | Set-Content $missingTfmManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-api.ps1") `
        -Version "2.10.1" `
        -Smoke `
        -ManifestPath $missingTfmManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Missing DLL/XML input validation unexpectedly passed."
    }

    $badExampleRoot = Join-Path $tempRoot "bad-example"
    Copy-Item `
        (Join-Path $repoRoot (
            "website/versioned_docs/version-$previousLatestVersion/_examples/quick-start"
        )) `
        $badExampleRoot `
        -Recurse
    foreach ($buildFile in @(
        "Directory.Build.props",
        "Directory.Build.targets"
    )) {
        Copy-Item `
            (Join-Path $repoRoot (
                "website/versioned_docs/version-$previousLatestVersion/_examples/$buildFile"
            )) `
            $badExampleRoot
    }
    $badExampleTargets = Join-Path $badExampleRoot "Directory.Build.targets"
    $badExampleText = (Get-Content -Raw $badExampleTargets).Replace(
        'Version="$(HumanizerPackageVersion)"',
        "Version=`"$candidateVersion`""
    )
    [System.IO.File]::WriteAllText(
        $badExampleTargets,
        $badExampleText,
        [System.Text.UTF8Encoding]::new($false)
    )
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version $previousLatestVersion `
        -ExamplesRoot $badExampleRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "An example restored against the wrong package version."
    }

    $mixedPackageGraphFailed = $false
    try {
        Assert-DocsHumanizerPackageGraph `
            -Entry $candidateEntry `
            -Libraries ([PSCustomObject]@{
                "Humanizer/$candidateVersion" =
                    [PSCustomObject]@{ type = "package" }
                "Humanizer.Core/$previousLatestVersion" =
                    [PSCustomObject]@{ type = "package" }
            }) `
            -Context "Synthetic example"
    } catch {
        $mixedPackageGraphFailed = $true
    }
    if (-not $mixedPackageGraphFailed) {
        throw "A mixed Humanizer package graph unexpectedly passed."
    }

    $extractPaths = [System.Collections.Generic.List[string]]::new()
    $extractedDllHashes = [System.Collections.Generic.List[string]]::new()
    foreach ($iteration in 1..2) {
        Use-DocsNuGetPackage `
            -Entry $candidateEntry `
            -RepoRoot $repoRoot `
            -Action {
            param($package)

            [void]$extractPaths.Add($package.ExtractPath)
            $dllPath = Join-Path (
                $package.ExtractPath
            ) "lib/$($candidateEntry.referenceTfm)/Humanizer.dll"
            [void]$extractedDllHashes.Add(
                (Get-FileHash $dllPath -Algorithm SHA256).Hash
            )
            if ($iteration -eq 1) {
                Set-Content $dllPath "tampered extraction"
            }
        }
        if (Test-Path $extractPaths[$extractPaths.Count - 1]) {
            throw "A verified package extraction was not removed after use."
        }
    }
    if ($extractPaths[0] -eq $extractPaths[1] -or
        $extractedDllHashes[0] -ne $extractedDllHashes[1]) {
        throw "Verified package extraction was reused instead of recreated."
    }

    $memberCoverageRoot = Join-Path $tempRoot "api-member-coverage"
    Copy-Item `
        (Join-Path $repoRoot "website/docs/api") `
        $memberCoverageRoot `
        -Recurse
    $defaultFormatterPath = Join-Path (
        $memberCoverageRoot
    ) "Humanizer.DefaultFormatter.md"
    $memberLinksPath = Join-Path $tempRoot "api-member-links.txt"
    $publicLinksPath = Join-Path $tempRoot "public-api-links.txt"
    [System.IO.File]::WriteAllText(
        $memberLinksPath,
        "./`nT:Humanizer.DefaultFormatter|Humanizer.DefaultFormatter.md|DefaultFormatter`nP:Humanizer.DefaultFormatter.Culture|Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.Culture|Culture`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $publicLinksPath,
        "./`nT:Humanizer.DefaultFormatter|Humanizer.DefaultFormatter.md|DefaultFormatter`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    Assert-GeneratedApiLinks `
        -OutputPath $memberCoverageRoot `
        -LinksPath $memberLinksPath `
        -VersionLabel "current baseline"
    $memberCoverageInventory = [PSCustomObject]@{
        Records = @(
            [PSCustomObject]@{
                Access = "public"
                Id = "T:Humanizer.DefaultFormatter"
            },
            [PSCustomObject]@{
                Access = "protected"
                Id = "P:Humanizer.DefaultFormatter.Culture"
            }
        )
    }
    Assert-ApiAccessCoverage `
        -ApiLinksPath $memberLinksPath `
        -PublicLinksPath $publicLinksPath `
        -AssemblyInventory $memberCoverageInventory `
        -VersionLabel "current baseline"
    $publicOnlyFailed = $false
    try {
            Assert-ApiAccessCoverage `
                -ApiLinksPath $publicLinksPath `
                -PublicLinksPath $publicLinksPath `
                -AssemblyInventory $memberCoverageInventory `
                -VersionLabel "public-only"
    } catch {
        $publicOnlyFailed = $true
    }
    if (-not $publicOnlyFailed) {
        throw "Public-only API generation unexpectedly passed access coverage."
    }
    $defaultFormatter = Get-Content -Raw $defaultFormatterPath
    $cultureAnchor = "<a name='Humanizer.DefaultFormatter.Culture'></a>`n"
    $withoutCulture = $defaultFormatter.Replace(
        $cultureAnchor,
        ""
    )
    if ($withoutCulture -eq $defaultFormatter) {
        throw "The API member-coverage fixture is missing the Culture anchor."
    }
    [System.IO.File]::WriteAllText(
        $defaultFormatterPath,
        $withoutCulture,
        [System.Text.UTF8Encoding]::new($false)
    )
    $memberCoverageFailed = $false
    try {
        Assert-GeneratedApiLinks `
            -OutputPath $memberCoverageRoot `
            -LinksPath $memberLinksPath `
            -VersionLabel "current"
    } catch {
        $memberCoverageFailed = $true
    }
    if (-not $memberCoverageFailed) {
        throw "An omitted non-representative API member unexpectedly passed."
    }

    $preJournalRoot = Join-Path $tempRoot "pre-journal-website"
    New-Item -ItemType Directory -Path $preJournalRoot | Out-Null
    $preJournalFailed = $false
    try {
        Invoke-SnapshotTransaction `
            -WebsiteRoot $preJournalRoot `
            -Changes @(
                [PSCustomObject]@{
                    Target = "not-an-allowed-target"
                    Source = (Join-Path $tempRoot "missing-source")
                }
            )
    } catch {
        $preJournalFailed = $true
    }
    if (-not $preJournalFailed -or
        (Test-Path (Join-Path $preJournalRoot ".snapshot-transaction"))) {
        throw "Pre-journal snapshot failure left stale transaction state."
    }

    $journalInvariantRoot = Join-Path $tempRoot "journal-invariant-website"
    New-Item -ItemType Directory -Path $journalInvariantRoot | Out-Null
    $journalInvariantVersions = Join-Path $journalInvariantRoot "versions.json"
    $journalInvariantManifest = Join-Path (
        $journalInvariantRoot
    ) "humanizer-versions.json"
    $journalSourceOne = Join-Path $tempRoot "journal-source-one.json"
    $journalSourceTwo = Join-Path $tempRoot "journal-source-two.json"
    [System.IO.File]::WriteAllText(
        $journalInvariantVersions,
        "[`"$candidateVersion`"]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalInvariantManifest,
        "{`"schemaVersion`":1}`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalSourceOne,
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalSourceTwo,
        "{} `n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $journalInvariantVersionsHash = (
        Get-FileHash $journalInvariantVersions -Algorithm SHA256
    ).Hash
    $journalInvariantManifestHash = (
        Get-FileHash $journalInvariantManifest -Algorithm SHA256
    ).Hash
    foreach ($case in @(
        [PSCustomObject]@{
            Name = "duplicate target"
            ExpectedError = "unique targets"
            Changes = @(
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceTwo
                }
            )
        },
        [PSCustomObject]@{
            Name = "manifest before another target"
            ExpectedError = "manifest target must be last"
            Changes = @(
                [PSCustomObject]@{
                    Target = "humanizer-versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceTwo
                }
            )
        }
    )) {
        $journalInvariantFailed = $false
        try {
            Invoke-SnapshotTransaction `
                -WebsiteRoot $journalInvariantRoot `
                -Changes $case.Changes
        } catch {
            $journalInvariantFailed =
                $_.Exception.Message -match $case.ExpectedError
        }
        if (-not $journalInvariantFailed -or
            (Get-FileHash $journalInvariantVersions -Algorithm SHA256).Hash -ne
                $journalInvariantVersionsHash -or
            (Get-FileHash $journalInvariantManifest -Algorithm SHA256).Hash -ne
                $journalInvariantManifestHash -or
            (Test-Path (
                Join-Path $journalInvariantRoot ".snapshot-transaction"
            ))) {
            throw "Pre-journal $($case.Name) rejection was not fail-closed."
        }
    }

    $validationRollbackFailed = $false
    try {
        Invoke-SnapshotTransaction `
            -WebsiteRoot $journalInvariantRoot `
            -Changes @(
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "humanizer-versions.json"
                    Source = $journalSourceTwo
                }
            ) `
            -Validate {
                throw "Synthetic post-install validation failure."
            }
    } catch {
        $validationRollbackFailed =
            $_.Exception.Message -match
                "Synthetic post-install validation failure"
    }
    if (-not $validationRollbackFailed -or
        (Get-FileHash $journalInvariantVersions -Algorithm SHA256).Hash -ne
            $journalInvariantVersionsHash -or
        (Get-FileHash $journalInvariantManifest -Algorithm SHA256).Hash -ne
            $journalInvariantManifestHash -or
        (Test-Path (
            Join-Path $journalInvariantRoot ".snapshot-transaction"
        ))) {
        throw "Post-install validation failure was not rolled back atomically."
    }

    $snapshotPath = Join-Path $repoRoot (
        "website/versioned_docs/version-$candidateVersion"
    )
    $sidebarsPath = Join-Path $repoRoot (
        "website/versioned_sidebars/version-$candidateVersion-sidebars.json"
    )
    $versionsPath = Join-Path $repoRoot "website/versions.json"
    $snapshotDigest = Get-SnapshotDirectoryDigest $snapshotPath
    $sidebarsHash = (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash
    $versionsHash = (Get-FileHash $versionsPath -Algorithm SHA256).Hash

    foreach ($rejectedArguments in @(
        @("-Version", $candidateVersion),
        @("-Version", $candidateVersion, "-CorrectPage", "api/Humanizer.StringHumanizeExtensions.md"),
        @("-All")
    )) {
        & pwsh -NoProfile -File (
            Join-Path $PSScriptRoot "snapshot.ps1"
        ) @rejectedArguments *> $null
        if ($LASTEXITCODE -eq 0) {
            throw "Rejected snapshot operation unexpectedly passed: $($rejectedArguments -join ' ')"
        }
    }
    if ((Get-SnapshotDirectoryDigest $snapshotPath) -ne $snapshotDigest -or
        (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash -ne $sidebarsHash -or
        (Get-FileHash $versionsPath -Algorithm SHA256).Hash -ne $versionsHash) {
        throw "A rejected snapshot operation changed immutable output."
    }

    $isolatedWebsiteRoot = Join-Path $tempRoot "website"
    New-Item -ItemType Directory -Path $isolatedWebsiteRoot | Out-Null
    foreach ($path in @(
        "docs",
        "version-overrides",
        "versioned_docs",
        "versioned_sidebars"
    )) {
        Copy-Item `
            (Join-Path $repoRoot "website/$path") `
            $isolatedWebsiteRoot `
            -Recurse
    }
    foreach ($path in @(
        "humanizer-versions.json",
        "scenario-api-contract.json",
        "sidebars.json",
        "versions.json"
    )) {
        Copy-Item `
            (Join-Path $repoRoot "website/$path") `
            $isolatedWebsiteRoot
    }
    $isolatedManifestPath = Join-Path (
        $isolatedWebsiteRoot
    ) "humanizer-versions.json"

    $global:HumanizerSnapshotLockProbeRoot = $isolatedWebsiteRoot
    $global:HumanizerSnapshotLockProbeResult = $null
    $lockBreakpoint = Set-PSBreakpoint `
        -Command Assert-FrozenSnapshot `
        -Action {
            $lockPath = Join-Path (
                $global:HumanizerSnapshotLockProbeRoot
            ) ".snapshot-mutation.lock"
            try {
                $probe = [System.IO.File]::Open(
                    $lockPath,
                    [System.IO.FileMode]::OpenOrCreate,
                    [System.IO.FileAccess]::ReadWrite,
                    [System.IO.FileShare]::None
                )
                $probe.Dispose()
                $global:HumanizerSnapshotLockProbeResult = "released"
            } catch [System.IO.IOException] {
                $global:HumanizerSnapshotLockProbeResult = "held"
            } catch {
                $global:HumanizerSnapshotLockProbeResult =
                    "error:$($_.Exception.GetType().FullName)"
            }
        }
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
        $lockProbeResult = $global:HumanizerSnapshotLockProbeResult
    } finally {
        Remove-PSBreakpoint $lockBreakpoint
        Remove-Variable `
            HumanizerSnapshotLockProbeRoot `
            -Scope Global `
            -ErrorAction SilentlyContinue
        Remove-Variable `
            HumanizerSnapshotLockProbeResult `
            -Scope Global `
            -ErrorAction SilentlyContinue
    }
    if ($lockProbeResult -ne "held") {
        throw "Snapshot check did not retain the global lock: $lockProbeResult"
    }

    $isolatedFixtureManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    $identicalOverlayFixture = @(
        foreach ($entry in $isolatedFixtureManifest.versions) {
            if ([string]::IsNullOrWhiteSpace(
                $entry.compatibilityOverlay
            )) {
                continue
            }
            $overlayRoot = Join-Path (
                $isolatedWebsiteRoot
            ) $entry.compatibilityOverlay
            $overlay = Get-Content -Raw (
                Join-Path $overlayRoot "overlay.json"
            ) | ConvertFrom-Json -Depth 20
            foreach ($path in @($overlay.replacements)) {
                if (Test-Path (
                    Join-Path $isolatedWebsiteRoot "docs/$path"
                ) -PathType Leaf) {
                    [PSCustomObject]@{
                        Entry = $entry
                        Path = $path
                        Root = $overlayRoot
                    }
                }
            }
        }
    )[0]
    if ($null -eq $identicalOverlayFixture) {
        throw "The documentation fixture requires one authored overlay replacement."
    }
    $identicalOverlayRelativePath = $identicalOverlayFixture.Path
    $identicalOverlayPath = Join-Path (
        $identicalOverlayFixture.Root
    ) $identicalOverlayRelativePath
    Copy-Item (
        Join-Path $isolatedWebsiteRoot "docs/$identicalOverlayRelativePath"
    ) $identicalOverlayPath -Force
    & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "verify-manifest.ps1"
    ) `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "A byte-identical overlay unexpectedly passed."
    }
    Copy-Item (
        Join-Path (
            Join-Path $repoRoot (
                "website/$($identicalOverlayFixture.Entry.compatibilityOverlay)"
            )
        ) $identicalOverlayRelativePath
    ) $identicalOverlayPath -Force

    $oldestPublishedVersion = $priorPublishedVersions[-1]
    $deletedPublishedPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_docs/version-$oldestPublishedVersion/start/overview.mdx"
    Remove-Item $deletedPublishedPath -Force
    & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "snapshot.ps1"
    ) `
        -All `
        -Check `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "A deleted published snapshot file unexpectedly passed."
    }
    Copy-Item (
        Join-Path $repoRoot (
            "website/versioned_docs/version-$oldestPublishedVersion/start/overview.mdx"
        )
    ) $deletedPublishedPath

    $transactionRoot = Join-Path $isolatedWebsiteRoot ".snapshot-transaction"
    New-Item -ItemType Directory `
        -Path (Join-Path $transactionRoot "backups") `
        -Force | Out-Null
    $versionsBeforeDigest = (Get-FileHash (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) -Algorithm SHA256).Hash
    Copy-Item (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) (Join-Path $transactionRoot "backups/0")
    [System.IO.File]::WriteAllText(
        (Join-Path $isolatedWebsiteRoot "versions.json"),
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $versionsAfterDigest = (Get-FileHash (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) -Algorithm SHA256).Hash
    Write-SnapshotJson `
        -Value ([PSCustomObject]@{
            schemaVersion = 1
            state = "applying"
            targets = @(
                [PSCustomObject]@{
                    index = 0
                    target = "versions.json"
                    staged = "staged/0"
                    backup = "backups/0"
                    hadOriginal = $true
                    backupReady = $true
                    installing = $true
                    installed = $false
                    beforeDigest = $versionsBeforeDigest
                    afterDigest = $versionsAfterDigest
                }
            )
        }) `
        -Path (Join-Path $transactionRoot "journal.json")
    [System.IO.File]::WriteAllText(
        (Join-Path $isolatedWebsiteRoot ".snapshot-mutation.lock"),
        '{"pid":-1,"startedUtc":"stale"}',
        [System.Text.UTF8Encoding]::new($false)
    )
    $recoveryFailedForExpectedReason = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version "not-declared" `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $recoveryFailedForExpectedReason =
            $_.Exception.Message -match "not declared exactly once"
    }
    if (-not $recoveryFailedForExpectedReason -or
        (Get-Content -Raw (Join-Path $isolatedWebsiteRoot "versions.json")) -eq
            "[]`n" -or
        (Test-Path $transactionRoot)) {
        throw "A stale snapshot transaction was not recovered before validation."
    }

    New-Item -ItemType Directory `
        -Path (Join-Path $transactionRoot "backups") `
        -Force | Out-Null
    $restartableVersionsPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versions.json"
    $restartableBeforeDigest = (
        Get-FileHash $restartableVersionsPath -Algorithm SHA256
    ).Hash
    Copy-Item `
        $restartableVersionsPath `
        (Join-Path $transactionRoot "backups/0")
    $restartableAfterSource = Join-Path (
        $tempRoot
    ) "restartable-after-versions.json"
    $restartableAbsentSource = Join-Path (
        $tempRoot
    ) "restartable-after-sidebar.json"
    [System.IO.File]::WriteAllText(
        $restartableAfterSource,
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $restartableAbsentSource,
        "{} `n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $restartableAbsentTarget = (
        "versioned_sidebars/version-9.9.9-sidebars.json"
    )
    Write-SnapshotJson `
        -Value ([PSCustomObject]@{
            schemaVersion = 1
            state = "applying"
            targets = @(
                [PSCustomObject]@{
                    index = 0
                    target = "versions.json"
                    staged = "staged/0"
                    backup = "backups/0"
                    hadOriginal = $true
                    backupReady = $true
                    installing = $false
                    installed = $true
                    beforeDigest = $restartableBeforeDigest
                    afterDigest = (
                        Get-FileHash `
                            $restartableAfterSource `
                            -Algorithm SHA256
                    ).Hash
                },
                [PSCustomObject]@{
                    index = 1
                    target = $restartableAbsentTarget
                    staged = "staged/1"
                    backup = "backups/1"
                    hadOriginal = $false
                    backupReady = $false
                    installing = $false
                    installed = $true
                    beforeDigest = $null
                    afterDigest = (
                        Get-FileHash `
                            $restartableAbsentSource `
                            -Algorithm SHA256
                    ).Hash
                }
            )
        }) `
        -Path (Join-Path $transactionRoot "journal.json")
    $restartableLock = Enter-SnapshotMutation `
        -WebsiteRoot $isolatedWebsiteRoot
    try {
        if ((Get-FileHash (
                Join-Path $isolatedWebsiteRoot "versions.json"
            ) -Algorithm SHA256).Hash -ne $restartableBeforeDigest -or
            (Test-Path (
                Join-Path $isolatedWebsiteRoot $restartableAbsentTarget
            )) -or
            (Test-Path $transactionRoot)) {
            throw "Restarted rollback did not preserve the restored state."
        }
    } finally {
        Exit-SnapshotMutation -Lock $restartableLock
    }

    $isolatedCurrentApi = Join-Path $isolatedWebsiteRoot "docs/api"
    $currentApiDigest = Get-SnapshotDirectoryDigest $isolatedCurrentApi
    Add-Content (
        Join-Path $isolatedCurrentApi "index.md"
    ) "`n<!-- tampered current API -->"
    $currentCheckFailed = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version current `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $currentCheckFailed = $true
    }
    if (-not $currentCheckFailed) {
        throw "A tampered current API unexpectedly passed."
    }
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version current `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ((Get-SnapshotDirectoryDigest $isolatedCurrentApi) -ne
        $currentApiDigest) {
        throw "Current API refresh did not restore the deterministic tree."
    }
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version current `
        -Check `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null

    $currentLanding = Get-Content -Raw (
        Join-Path $isolatedCurrentApi "index.md"
    )
    if ($currentLanding -notmatch "(?m)^## Namespaces$" -or
        -not $currentLanding.Contains(
            "[StringHumanizeExtensions](Humanizer.StringHumanizeExtensions.md",
            [System.StringComparison]::Ordinal
        )) {
        throw "Current Humanizer 4 API landing is missing its generated index."
    }
    $v4ReleaseRoot = Join-Path $tempRoot "v4-release-source"
    $v4ReleaseRebuildRoot = Join-Path $tempRoot "v4-release-rebuild"
    $v4ReleaseSource = Join-Path $v4ReleaseRoot "api"
    $v4ReleaseRebuild = Join-Path $v4ReleaseRebuildRoot "api"
    New-Item -ItemType Directory -Path $v4ReleaseRoot | Out-Null
    New-Item -ItemType Directory -Path $v4ReleaseRebuildRoot | Out-Null
    Copy-Item $isolatedCurrentApi $v4ReleaseSource -Recurse
    Copy-Item $isolatedCurrentApi $v4ReleaseRebuild -Recurse
    $v4ReleaseEntry = [PSCustomObject]@{
        version = "4.0.0"
        apiPackage = "Humanizer"
        referenceTfm = "net10.0"
        source = [PSCustomObject]@{
            packageVersion = "4.0.0"
        }
    }
    Add-ApiLanding -ApiRoot $v4ReleaseSource -Entry $v4ReleaseEntry
    Add-ApiLanding -ApiRoot $v4ReleaseRebuild -Entry $v4ReleaseEntry
    $v4ReleaseDigest = Get-SnapshotDirectoryDigest $v4ReleaseSource
    if ($v4ReleaseDigest -ne
        (Get-SnapshotDirectoryDigest $v4ReleaseRebuild)) {
        throw "Humanizer 4 release API landing is not deterministic."
    }
    $v4ReleaseTarget = "versioned_docs/version-4.0.0"
    Invoke-SnapshotTransaction `
        -WebsiteRoot $isolatedWebsiteRoot `
        -Changes @(
            [PSCustomObject]@{
                Target = $v4ReleaseTarget
                Source = $v4ReleaseRoot
            }
        )
    $v4InstalledApi = Join-Path (
        Join-Path $isolatedWebsiteRoot $v4ReleaseTarget
    ) "api"
    if ((Get-SnapshotDirectoryDigest $v4InstalledApi) -ne
        $v4ReleaseDigest -or
        (Get-Content -Raw (Join-Path $v4InstalledApi "index.md")) -notmatch
            "(?m)^## Namespaces$") {
        throw "Humanizer 4 release round trip lost the generated API index."
    }
    Remove-Item (
        Join-Path $isolatedWebsiteRoot $v4ReleaseTarget
    ) -Recurse -Force

    $isolatedManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    $isolatedCandidateEntries = @(
        $isolatedManifest.versions |
            Where-Object version -eq $candidateVersion
    )
    $priorVersions = @(
        $isolatedManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.published -and
                $_.version -ne $candidateVersion
            } |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending |
            ForEach-Object version
    )
    if ($isolatedCandidateEntries.Count -ne 1 -or
        -not $isolatedCandidateEntries[0].published -or
        $priorVersions.Count -eq 0 -or
        $previousLatestVersion -notin $priorVersions) {
        throw "The isolated release fixture has invalid publication state."
    }
    $priorState = @{}
    foreach ($priorVersion in $priorVersions) {
        $priorEntry = @(
            $isolatedManifest.versions |
                Where-Object version -eq $priorVersion
        )[0]
        $priorState[$priorVersion] = [PSCustomObject]@{
            ManifestSnapshot = $priorEntry.immutability.snapshotSha256
            ManifestSidebar = $priorEntry.immutability.sidebarSha256
            LiveSnapshot = Get-SnapshotDirectoryDigest (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_docs/version-$priorVersion"
                )
            )
            LiveSidebar = (Get-FileHash (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_sidebars/version-$priorVersion-sidebars.json"
                )
            ) -Algorithm SHA256).Hash
        }
    }
    $releaseSnapshotPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_docs/version-$candidateVersion"
    $releaseSidebarPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_sidebars/version-$candidateVersion-sidebars.json"
    $releaseSnapshotDigest = Get-SnapshotDirectoryDigest $releaseSnapshotPath
    $releaseApiLandingPath = Join-Path $releaseSnapshotPath "api/index.md"
    $releaseApiLandingHash = (
        Get-FileHash $releaseApiLandingPath -Algorithm SHA256
    ).Hash
    $releaseSidebarHash = (
        Get-FileHash $releaseSidebarPath -Algorithm SHA256
    ).Hash
    $canonicalDocsRoot = Join-Path $tempRoot "canonical-docs"
    Copy-Item `
        (Join-Path $isolatedWebsiteRoot "docs") `
        $canonicalDocsRoot `
        -Recurse
    Remove-Item (
        Join-Path $isolatedWebsiteRoot "docs"
    ) -Recurse -Force
    Copy-Item $releaseSnapshotPath (
        Join-Path $isolatedWebsiteRoot "docs"
    ) -Recurse
    Copy-Item (
        Join-Path $canonicalDocsRoot "_examples/Directory.Build.props"
    ) (
        Join-Path $isolatedWebsiteRoot "docs/_examples/Directory.Build.props"
    ) -Force
    Remove-Item (
        Join-Path $isolatedWebsiteRoot "docs/api"
    ) -Recurse -Force
    Copy-Item $releaseSidebarPath (
        Join-Path $isolatedWebsiteRoot "sidebars.json"
    ) -Force
    $releaseOverlayRoot = Join-Path (
        $isolatedWebsiteRoot
    ) $isolatedCandidateEntries[0].compatibilityOverlay
    $releaseOverlay = Get-Content -Raw (
        Join-Path $releaseOverlayRoot "overlay.json"
    ) | ConvertFrom-Json -Depth 20
    foreach ($path in @(
        $releaseOverlay.replacements + $releaseOverlay.exclusions
    )) {
        $destination = Join-Path (
            Join-Path $isolatedWebsiteRoot "docs"
        ) $path
        New-Item -ItemType Directory `
            -Path (Split-Path $destination -Parent) `
            -Force | Out-Null
        Copy-Item (Join-Path $canonicalDocsRoot $path) $destination -Force
    }
    foreach ($priorEntry in (
        $isolatedManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.version -ne $candidateVersion
            }
    )) {
        $priorEntry.latestStable = $false
        $priorEntry.route = $priorEntry.version
        $priorEntry.label = $priorEntry.version
    }
    $previousLatest = @(
        $isolatedManifest.versions |
            Where-Object version -eq $previousLatestVersion
    )[0]
    $previousLatest.latestStable = $true
    $previousLatest.route = ""
    $previousLatest.label = "$previousLatestVersion (latest)"
    $future = @(
        $isolatedManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0]
    $future.published = $false
    $future.latestStable = $false
    $future.route = $candidateVersion
    $future.label = $candidateVersion
    $future.PSObject.Properties.Remove("immutability")
    Write-SnapshotJson -Value $isolatedManifest -Path $isolatedManifestPath
    Write-SnapshotJson `
        -Value $priorVersions `
        -Path (Join-Path $isolatedWebsiteRoot "versions.json") `
        -AsArray
    Remove-Item (
        Join-Path $isolatedWebsiteRoot (
            "versioned_docs/version-$candidateVersion"
        )
    ) -Recurse -Force
    Remove-Item (
        Join-Path $isolatedWebsiteRoot (
            "versioned_sidebars/version-$candidateVersion-sidebars.json"
        )
    ) -Force

    $assertReleaseRoundTrip = {
        param(
            [Parameter(Mandatory = $true)]$ReleasedManifest,
            [Parameter(Mandatory = $true)][string]$Mode
        )

        $releasedCandidates = @(
            $ReleasedManifest.versions |
                Where-Object version -eq $candidateVersion
        )
        if ($releasedCandidates.Count -ne 1) {
            throw "$Mode release did not retain exactly one candidate."
        }
        $releasedCandidate = $releasedCandidates[0]
        Assert-VersionedExampleDefaults `
            -Entry $releasedCandidate `
            -ExamplesRoot (Join-Path $isolatedWebsiteRoot (
                "versioned_docs/version-$candidateVersion/_examples"
            ))
        if ($releasedCandidate.immutability.snapshotSha256 -ne
                $releaseSnapshotDigest -or
            $releasedCandidate.immutability.sidebarSha256 -ne
                $releaseSidebarHash -or
            (Get-SnapshotDirectoryDigest (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_docs/version-$candidateVersion"
                )
            )) -ne $releaseSnapshotDigest -or
            (Get-FileHash (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_docs/version-$candidateVersion/api/index.md"
                )
            ) -Algorithm SHA256).Hash -ne $releaseApiLandingHash -or
            (Get-FileHash (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_sidebars/version-$candidateVersion-sidebars.json"
                )
            ) -Algorithm SHA256).Hash -ne $releaseSidebarHash) {
            throw "$Mode release round trip changed immutable $candidateVersion state."
        }
        $releasedApiLanding = Get-Content -Raw (
            Join-Path $isolatedWebsiteRoot (
                "versioned_docs/version-$candidateVersion/api/index.md"
            )
        )
        $candidateMajor = [System.Management.Automation.SemanticVersion]::Parse(
            $candidateVersion
        ).Major
        if (($candidateMajor -ge 4 -and
                $releasedApiLanding -notmatch "(?m)^## Namespaces$") -or
            ($candidateMajor -lt 4 -and
                $releasedApiLanding -match "(?m)^## Namespaces$")) {
            throw "$Mode release emitted the wrong API landing for $candidateVersion."
        }
        foreach ($priorVersion in $priorVersions) {
            $releasedPrior = @(
                $ReleasedManifest.versions |
                    Where-Object version -eq $priorVersion
            )[0]
            $before = $priorState[$priorVersion]
            if ($releasedPrior.immutability.snapshotSha256 -ne
                    $before.ManifestSnapshot -or
                $releasedPrior.immutability.sidebarSha256 -ne
                    $before.ManifestSidebar -or
                (Get-SnapshotDirectoryDigest (
                    Join-Path $isolatedWebsiteRoot (
                        "versioned_docs/version-$priorVersion"
                    )
                )) -ne $before.LiveSnapshot -or
                (Get-FileHash (
                    Join-Path $isolatedWebsiteRoot (
                        "versioned_sidebars/version-$priorVersion-sidebars.json"
                    )
                ) -Algorithm SHA256).Hash -ne $before.LiveSidebar) {
                throw "$Mode release changed prior immutable state: $priorVersion"
            }
        }
    }

    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version $candidateVersion `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    $nonDefaultManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    & $assertReleaseRoundTrip `
        -ReleasedManifest $nonDefaultManifest `
        -Mode "Non-default"
    $nonDefaultCandidate = @(
        $nonDefaultManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0]
    $nonDefaultLatest = @(
        $nonDefaultManifest.versions |
            Where-Object version -eq $previousLatestVersion
    )[0]
    if (-not $nonDefaultCandidate.published -or
        $nonDefaultCandidate.latestStable -or
        $nonDefaultCandidate.route -ne $candidateVersion -or
        $nonDefaultCandidate.label -ne $candidateVersion -or
        -not $nonDefaultLatest.latestStable -or
        $nonDefaultLatest.route -ne "" -or
        $nonDefaultLatest.label -ne "$previousLatestVersion (latest)") {
        throw "Non-default publication changed latest-version routing."
    }

    if ($canPromoteCandidate) {
        $nonDefaultCandidate.published = $false
        $nonDefaultCandidate.PSObject.Properties.Remove("immutability")
        Write-SnapshotJson `
            -Value $nonDefaultManifest `
            -Path $isolatedManifestPath
        Write-SnapshotJson `
            -Value $priorVersions `
            -Path (Join-Path $isolatedWebsiteRoot "versions.json") `
            -AsArray
        Remove-Item (
            Join-Path $isolatedWebsiteRoot (
                "versioned_docs/version-$candidateVersion"
            )
        ) -Recurse -Force
        Remove-Item (
            Join-Path $isolatedWebsiteRoot (
                "versioned_sidebars/version-$candidateVersion-sidebars.json"
            )
        ) -Force
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -PromoteLatest `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
        $releasedManifest = Get-Content -Raw $isolatedManifestPath |
            ConvertFrom-Json -Depth 20
        & $assertReleaseRoundTrip `
            -ReleasedManifest $releasedManifest `
            -Mode "Promoted"
        $promotedCandidate = @(
            $releasedManifest.versions |
                Where-Object version -eq $candidateVersion
        )[0]
        $demotedLatest = @(
            $releasedManifest.versions |
                Where-Object version -eq $previousLatestVersion
        )[0]
        if (-not $promotedCandidate.latestStable -or
            $promotedCandidate.route -ne "" -or
            $promotedCandidate.label -ne "$candidateVersion (latest)" -or
            $demotedLatest.latestStable -or
            $demotedLatest.route -ne $previousLatestVersion -or
            $demotedLatest.label -ne $previousLatestVersion) {
            throw "Promoted publication did not update latest-version routing."
        }
    } else {
        $releasedManifest = $nonDefaultManifest
    }

    $correctionPath = "start/overview.mdx"
    $correctionSourceRoot = if (
        $correctionPath -in @($releaseOverlay.replacements)
    ) {
        $releaseOverlayRoot
    } else {
        Join-Path $isolatedWebsiteRoot "docs"
    }
    $isolatedCorrectionPage = Join-Path $correctionSourceRoot $correctionPath
    $isolatedSnapshotPage = Join-Path $releaseSnapshotPath $correctionPath
    if (-not (Test-Path $isolatedCorrectionPage -PathType Leaf) -or
        -not (Test-Path $isolatedSnapshotPage -PathType Leaf)) {
        throw "The release fixture requires the authored overview page."
    }
    $snapshotPageHash = (
        Get-FileHash $isolatedSnapshotPage -Algorithm SHA256
    ).Hash
    $snapshotTreeDigest = Get-SnapshotDirectoryDigest $releaseSnapshotPath
    $manifestHash = (Get-FileHash $isolatedManifestPath -Algorithm SHA256).Hash
    $recordedDigest = @(
        $releasedManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0].immutability.snapshotSha256
    Add-Content `
        $isolatedCorrectionPage `
        "`n[broken correction](./missing-correction.mdx)"
    $correctionFailed = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -CorrectPage $correctionPath `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $correctionFailed = $true
    }
    $afterFailedManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    if (-not $correctionFailed -or
        (Get-FileHash $isolatedSnapshotPage -Algorithm SHA256).Hash -ne
            $snapshotPageHash -or
        (Get-SnapshotDirectoryDigest $releaseSnapshotPath) -ne
            $snapshotTreeDigest -or
        (Get-FileHash $isolatedManifestPath -Algorithm SHA256).Hash -ne
            $manifestHash -or
        @(
            $afterFailedManifest.versions |
                Where-Object version -eq $candidateVersion
        )[0].immutability.snapshotSha256 -ne $recordedDigest) {
        throw "Failed staged correction changed content or immutable digest."
    }
} finally {
    Remove-Item $tempRoot -Recurse -Force
}

Write-Host "Documentation manifest, snapshot immutability, idempotence, and failure checks passed."
