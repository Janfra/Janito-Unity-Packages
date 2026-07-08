# Janito Prototyping Package
A collection of wrappers and classes to improve the iteration speed when prototyping an idea. 

## 📦 Installation
### Via Unity Package Manager (UPM)
1. Open the **Window > Package Manager** in Unity.
2. Click the **+** icon and select **Add package from git URL...**
3. Paste the following URL: `https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoPrototypingPackage`

### Via Manifest Json
1. Open your project folder and locate the `Packages` folder.
2. Open `manifest.json`.
3. Add the following to the dependencies list:
```json
        "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoPrototypingPackage"
```
4. Ensure a comma is placed at the end of the previous line if not at the end of the list.

In the case of only having this package and its dependency, the file should look like the following example:
```json
{
    "dependencies": {
        "com.janito.animationpackage": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoPrototypingPackage",
        "com.janito.editorextras": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage"
    }
}
```

## Dependencies
This package requires **Janito Editor Extras** to be installed for Inspector attributes and UI logic.
*   **Git URL:** `https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage`