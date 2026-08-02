Here is a checklist you should tick through before submitting a pull request: 
 - [ ] Implementation is clean
 - [ ] Code adheres to the existing coding standards; e.g. no curlies for one-line blocks, no redundant empty lines between methods or code blocks, spaces rather than tabs, etc.
 - [ ] No Code Analysis warnings
 - [ ] There is proper unit test coverage
 - [ ] If the code is copied from StackOverflow (or a blog or OSS) full disclosure is included. That includes required license files and/or file headers explaining where the code came from with proper attribution
 - [ ] There are very few or no comments (because comments shouldn't be needed if you write clean code)
 - [ ] Xml documentation is added/updated for the addition/change
 - [ ] Your PR is (re)based on top of the latest commits from the `main` branch (more info below)
 - [ ] Link to the issue(s) you're fixing from your PR description. Use `fixes #<the issue number>`
 - [ ] Readme is updated if you change an existing feature or add a new one
 - [ ] Run the applicable validation from `AGENTS.md`; documentation changes include the documentation gates

## Terminal evidence (merge owner)

 - [ ] This PR body matches the current template, contains no stale draft instructions, and the PR is ready, not draft
 - [ ] Exact evidence pair: `baseSha=<base>` / `headSha=<head>`
 - [ ] Thermos correctness, breakage, security, and developer-experience review ran on that exact pair: `<evidence>`
 - [ ] Thermos code-quality and maintainability review ran on that exact pair: `<evidence>`
 - [ ] Real findings were fixed and both reviews reran on the replacement head, or no findings were reported
 - [ ] Applicable tests, format, build, package, documentation, browser, security, and platform gates pass on that exact pair: `<evidence>`
 - [ ] `compound-engineering:ce-babysit-pr` covered the exact pair through the current-head reviewer lifecycle, CI, base movement, and a quiet settle; terminal clean evidence: `<evidence>`
 - [ ] If either SHA changed, both Thermos reviews, every applicable check, and babysitting reran; stale evidence was removed
 - [ ] All actionable review threads are resolved; any `needs-human` item pauses merge
 - [ ] The head is current and mergeable, and terminal hosted CI and ruleset checks are green
 - [ ] Security: Codex Security proof-of-concept or attack-path closure passes, or N/A: `<evidence>`
 - [ ] Docs/UI: desktop, mobile, light, dark, accessibility, links, and version snapshots pass, or N/A: `<evidence>`
 - [ ] Localization/source generator: applicable locale, schema, generator, and runtime matrices pass with no partial or English fallback, or N/A: `<evidence>`
 - [ ] Merge owner will merge only the exact approved head
