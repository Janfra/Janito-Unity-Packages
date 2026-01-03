# Janito Unity Framework
A suite of Unity Editor extensions and runtime utilities designed to eliminate manual workflow and enforce code safety.

## 📦 Included Packages
<details>
<summary><b>1. Janito Animation Package</b> <i>(Click to expand)</i></summary>

**Eliminate string-based animator management.** This package provides a type-safe bridge between your C# logic and Unity's Animator system.

- **Animator Value Extraction:** Batch-generate AnimatorParameterHasher assets directly from any Animator Controller.
- **Live-Sync UI:** Real-time tracking of Animator changes within the Editor.
- **Listener System:** Frame-by-frame parameter monitoring and velocity-tracking modules.
- **Extension Methods:** Native Animator extensions for high-performance, string-less parameter setting.
</details>

<details>
<summary><b> 2. Janito Editor Extras</b> <i>(Click to expand)</i></summary>

A set of attributes and libraries to enhance the Inspector and enforce fail-fast validation.

- **Serialize Reference Type Selection:** Searchable dropdowns for `[SerializeReference]` fields with `TypeCache` optimization.
- **Fail-Fast Validation:** A `ValidationLibrary` that throws explicit exceptions for invalid configurations.
- **Path Management:** Project-relative pathing utilities for automated asset generation.
- **Build Safety:** All attributes are decorated with `[Conditional("UNITY_EDITOR")]` to ensure zero runtime overhead.
</details>

<details>
<summary><b>3. Janito Timers Package</b> <i>(Click to expand)</i></summary>

**Self-managed runtime timing.** A personalized extension of the *Improved Timers* architecture from [Adam Myhre - Unity Improved Timers](https://github.com/adammyhre/Unity-Improved-Timers), integrated into the Janito ecosystem.

- **Timer Manager**: Fully self-managed timers that handle their own lifecycle with a simple start and forget.
- **ReadOnly Alternatives**: Immutable interface to safely expose timer data without allowing external modification.
- **Extendable Core**: Seamlessly create custom timer types by inheriting from the `BaseTimer` class.
</details>

## 🛠 Installation
Packages can be installed through **Unity Package Manager (UPM)** via Git URLs. Each package includes a guide on how to install the package within its `README` file.

### Via Manifest (Recommended)
For installing several packages at once, follow the following steps:
1. Open your project folder and locate the `Packages` folder.
2. Open `manifest.json`.
3. Add the following items to the `dependencies` list, removing any undesired packages:
```json
    "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoAnimationPackage",
    "com.janito.editorextras": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage",
    "com.janito.timerspackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoTimersPackage"
```

The resulting file, including only the above packages, would look like the following:
```json
{
  "dependencies": {
      "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoAnimationPackage",
      "com.janito.editorextras": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage",
      "com.janito.timerspackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoTimersPackage"
  }
}
```

## 📄 License & Attribution

This framework is a multi-licensed project. Users must comply with the specific license applicable to each package.

*   **Janito Timers Package**: Licensed under the **MIT License** (inherited from the original [Improved Timers](https://github.com/adammyhre/Unity-Improved-Timers) by Adam Myhre).
*   **Other Packages**: Licensed under **Creative Commons Attribution-ShareAlike 4.0 International** (CC BY-SA 4.0).

### Third-Party Credits
The **Janito Timers Package** is a personalised fork of Adam Myhre's work. I have maintained the original license and copyright notices within the package directory as per the legal requirements of the MIT license.

> For full details, see the `LICENSE` file within each package directory.
