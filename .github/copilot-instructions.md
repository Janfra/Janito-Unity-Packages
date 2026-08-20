# Git Commit Instructions

When generating commit messages for this repository, you must strictly follow the Conventional Commits 1.0.0 specification.

## 1. Formatting Rules
- Structure the message exactly as: `<type>(<optional-scope>): <description>`
- The `<type>`, `<optional-scope>`, and `<description>` MUST be completely lowercase (except for literal code tokens or class names).
- Use the imperative, present tense for the description (e.g., "add feature", NOT "added feature" or "adds feature").
- Do not end the description line with a period or full stop.
- The entire title line must never exceed **50 characters**.
- Extended description body MUST encase referenced source code tokens or file names with backticks (e.g. `TypeLibrary`).

### A. Strict Type Overrides (File-Based Priority)
Before evaluating the content of a file diff, you MUST look at the file extensions and paths. Apply these strict type overrides regardless of what features are being discussed inside the text:
- **Changelog Files**: If the changes are strictly within `CHANGELOG.md` or a release notes file, the type MUST be `docs(changelog)`. It must NEVER be `feat` or `fix`, even if the text describes a new feature or bug fix.
- **Documentation Only**: If the diff only contains `.md`, `.txt`, or user manual files, the type MUST be `docs`.
- **Meta Files Only**: If the diff contains only Unity `.meta` files with no accompanying script changes, the type MUST be `chore` or `style`.

## 2. Permitted Structural Types
Only use the following structural types in the title:
- `feat`: A new feature or public API extension.
- `fix`: A bug fix or workflow layout correction.
- `docs`: Documentation-only updates, including CHANGELOG.md or README.md changes.
- `style`: Formatting, missing semicolons, or cosmetic adjustments (no code changes).
- `refactor`: Restructuring code layout without changing external behaviour or adding features.
- `perf`: Code changes aimed strictly at improving performance or reducing heap allocations.
- `test`: Adding or modifying automated unit test passes.
- `chore`: Maintenance tasks, assembly definitions, or package manifest updates.

## 3. Breaking Changes and Major Version Bumps
A breaking change indicates API-breaking modifications that require a Major version bump. You must strictly signal breaking changes using these two conventions:

### A. Title Line Signalling (The Exclamation Rule)
- Append an exclamation mark (`!`) directly between the scope closing parenthesis and the colon divider (e.g., `feat(editor)!: drop support for legacy IMGUI systems`).
- **Length Constraint**: The title line—including the exclamation mark—must still strictly abide by the **50-character limit**. Keep the description phrasing exceptionally concise if a breaking change is detected.

### B. Body Footer Signalling (The Footnote Rule)
- If a breaking change occurs, the extended description body MUST terminate with a dedicated footer block starting exactly with the uppercase token `BREAKING CHANGE:`.
- Follow the token with a space and a clear description explaining what was broken, what structural paths changed, and how developers should migrate their code (e.g., `BREAKING CHANGE: The TypeLibrary.GetFieldType method has been completely removed in favour of TypeLibrary.GetCoreType.`).
- Encase any referenced class, method, or file names within this footer inside backticks.

## 4. Unity Multi-Package Repository Rules
This repository functions as a multi-package repository containing isolated Unity packages. Adhere strictly to the following Unity-specific context when generating commits:

### A. Scope Determination
- The `<scope>` inside the Conventional Commit must match the name of the package folder being modified, stripping out any standard project prefixes like `Janito`.
- Transform folder names to lowercase dash-separated strings (e.g., if files inside `/JanitoExampleNamePackage` change, the scope MUST be `example-name`).
- If changes cross multiple packages simultaneously, use `deps` or `multi` as the scope, or split into multiple concise bullet points in the extended description body.

### B. Scope Abbreviations
When files in a package folder change, use these exact abbreviated scopes to save character space when a folder name matches:
- `/JanitoEditorExtrasPackage` -> use scope: `editor`
- `/JanitoAnimationPackage` -> use scope: `animation`
- `/JanitoPrototypingPackage` -> use scope: `proto`
- `/JanitoTimersPackage` -> use scope: `timers`

### C. Meta File Handling
- Unity `.meta` files are critical. If a `.meta` file is part of the staging area, do not ignore it. 
- If a commit consists *only* of adding or moving `.meta` files, use the `style` or `chore` type (e.g., `style(proto): regenerate missing asset meta files`).

### D. Assembly Definitions (.asmdef)
- If an `.asmdef` or `.asmref` file is modified, the commit type must be `chore` or `refactor`. 
- Ensure the description mentions whether a dependency was added, removed, or a platform target changed.

### E. Package Manifests (package.json)
- Updates to a package's `package.json` must use the `chore` or `release` type.
- Always try to state the version shift if visible in the diff (e.g., `chore(editor): bump package version to 2.1.0`).

## 5. Body Content Requirements
- Separate the title line from the body block using exactly one blank line.
- The body must provide a clear explanation of *why* the change was made and *what* it achieves structurally.
- Detail individual file or utility modifications using a clean, punchy bulleted list.
- **Spelling Style**: Always write descriptions using **UK English spelling conventions** (e.g., utilise "sanitisation", "initialise", "behaviour", and "dialogue").

## 6. Code Style Language
- For changes within C# scripts, align the description verbs with Unity-centric operations where applicable (e.g., "implement awake cycle", "optimise update loop", "expose serialized field").
