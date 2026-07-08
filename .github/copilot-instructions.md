# Git Commit Instructions

When generating commit messages for this repository, you must strictly follow the Conventional Commits specification.

## Formatting Rules
- Structure the message exactly as: `<type>(<optional-scope>): <description>`
- The `<type>` and `<optional-scope>` MUST be completely lowercase.
- The `<description>` should start with a capital letter (Sentence case).
- Use the imperative, present tense for the description (e.g., "Add feature", NOT "Added feature" or "Adds feature").
- Do not end the description line with a period.
- Keep the summary line under 50 characters.
- Extended description body MUST encase referenced source code names with backticks.

## Allowed Types
Only use one of the following structural types based on the code changes:
- **feat**: A new feature for the user
- **fix**: A bug fix for the user
- **docs**: Changes to the documentation
- **style**: Formatting, missing semi-colons, etc. (no production code changes)
- **refactor**: Refactoring production code (neither fixes a bug nor adds a feature)
- **perf**: Code changes that improve performance
- **test**: Adding missing tests or correcting existing tests
- **chore**: Updating build tasks, package manager configs, etc.
- **ci**: Changes to CI configuration files and scripts

## Unity Multi-Package Repository Rules

This repository functions as a multi-package repository containing isolated Unity packages. Adhere strictly to the following Unity-specific context when generating commits:

### 1. Scope Determination
- The `<scope>` inside the Conventional Commit must match the name of the package folder being modified unless stated otherwise (e.g., if files inside `/JanitoExampleNamePackage` change, the scope MUST be `example-name`).
- If changes cross multiple packages simultaneously, use `deps` or `multi` as the scope, or split into multiple concise bullet points in the extended description body.

#### Scope Abbreviations
When files in a package folder change, use these exact abbreviated scopes to save character space when folder name matches:
- `/JanitoEditorExtrasPackage` -> use scope: `editor`
- `/JanitoAnimationPackage` -> use scope: `animation`
- `/JanitoPrototypingPackage` -> use scope: `proto`
- `/JanitoTimersPackage` -> use scope: `timers`

### 2. Meta File Handling
- Unity `.meta` files are critical. If a `.meta` file is part of the staging area, do not ignore it. 
- If a commit consists *only* of adding or moving `.meta` files, use the `style` or `chore` type (e.g., `style(proto): Regenerate missing asset meta files`).

### 3. Assembly Definitions (.asmdef)
- If an `.asmdef` or `.asmref` file is modified, the commit type must be `chore` or `refactor`. 
- Ensure the description mentions whether a dependency was added, removed, or a platform target changed.

### 4. Package Manifests (package.json)
- Updates to a package's `package.json` must use the `chore` or `release` type.
- Always try to state the version shift if visible in the diff (e.g., `chore(editor): Bump package version to 2.1.0`).

### 5. Code Style Language
- For changes within C# scripts, align the description verbs with Unity-centric operations where applicable (e.g., "Implement Awake cycle", "Optimize Update loop", "Expose serialized field").
