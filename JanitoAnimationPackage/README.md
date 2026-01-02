# Janito Animation Package
A robust Unity framework designed to eliminate string-based animator management. This package provides high-performance hashing systems, automated asset generation, and a reactive listener framework to streamline your animation pipeline.

## 🚀 Key Features
- **Type-Safe Hashing:** Use `AnimatorParameterHasher` assets to replace error-prone strings with high-performance integer hashes.
- **Value Extraction Window:** Automatically scan any `AnimatorController` and batch-generate hasher assets for all parameters.
- **Live Syncing:** The editor UI tracks changes to your Animator Controllers in real-time, ensuring your tools stay in sync with your assets.
- **Listener System:** Extend functionality with `AnimatorParameterValueListener` to monitor or modify parameters frame-by-frame (e.g., matching Rigidbody velocity to a Float parameter).
- **Extension Methods:** Native Unity `Animator` extensions allow you to use hashes directly without mandatory wrapper components.

## 📦 Installation
### Via Unity Package Manager (UPM)
1. Open the **Window > Package Manager** in Unity.
2. Click the **+** icon and select **Add package from git URL...**
3. Paste the following URL: `https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoAnimationPackage`

### Via Manifest Json
1. Open your project folder and locate the `Packages` folder.
2. Open `manifest.json`.
3. Add the following to the dependencies list:
```json
        "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoAnimationPackage"
```
4. Ensure a comma is placed at the end of the previous line if not at the end of the list.

In the case of only having this package, the file should look like the following example:
```json
{
    "dependencies": {
        "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage"
    }
}
```

## Dependencies
This package requires **Janito Editor Extras** to be installed for Inspector attributes and UI logic.
*   **Git URL:** `https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage`

## 🛠 Usage
### 1. Extracting Parameters
1. Go to **Tools > Janito > Animator Value Extraction Window**. 
2. Drag or select an `AnimatorController` into the **Animator Controller** field.
3. Select the parameters you wish to generate. 
4. *Optional* — Change the destination path for the generated objects using the `...` button.  
5. Click **Create Selected Objects**. This generates `.asset` files for each parameter in your `Assets` root (or selected destination).

### 2. Using Hashes in Code
```csharp
[SerializeField] private AnimatorParameterHasher m_WalkHash;
private Animator m_Animator;

void Start() {
    m_Animator = GetComponent<Animator>();
    // Use the native extension method
    m_Animator.SetFloat(m_WalkHash, 1.0f); 
}
```

### 3. The Modifier Component
Attach the `AnimatorModifierComponent` to any GameObject. It automatically adds and fetches the Animator reference. It allows you to:
1. Fire off `AnimatorParameterEvent` assets via Unity Events or code.
2. Add `AnimatorParameterValueListener` via the Inspector.
3. Modify animator parameters using `AnimatorParameterHasher`.

## 📜 Changelog
### - 2026-01-02
#### Animator Value Extraction Window Improvements
- **Added:** Value Extraction Window Scalability (scrolling/headers) for large controllers.
- **Added:** Parameter Type Labels (Float, Int, Bool, Trigger) for visual context.
- **Added:** Live Animator Syncing to track controller changes in real-time.
- **Fixed:** Window State Persistence during layout changes or recompilations.
### - 2025-12-31
- **Added:** Automatic Class-Detection for the Listener System.
- **Added:** AnimatorParameterVelocityListener implementation.
- **Added:** Extension methods for the native Unity Animator.

### [View Full Changelog](./CHANGELOG.md)

## 📄 License
This project is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License.
See the [LICENSE](./LICENSE.md) file for details.
