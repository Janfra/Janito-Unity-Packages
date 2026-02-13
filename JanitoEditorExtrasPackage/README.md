# Janito Editor Extras Package

A utility-driven Unity framework designed to enhance the Editor experience through high-performance type discovery, inspector-friendly serialization tools, and a lightweight, memory-safe observables system.

All code is a result of my learning on extending the editor; there may be errors or oversights; I'll fix them as I become aware of them. Please do reach out with any problems or suggestions!

## 🚀 Key Features
- **Observables System**: A pure C# implementation of the observer pattern within the `Janito.EditorExtras.Observables` namespace, exposing events for changes on values and collections.
- **Type-Safe Serialization**: Use `ChildTypeSelectionAttribute` to provide searchable, namespace-aware dropdowns for `[SerializeReference]` fields with full support for interfaces and abstract classes.
- **Fluent Type Discovery**: A high-performance discovery system using Unity’s `TypeCache`. Create readable type constraints for filtering types.
- **Utility Libraries**: Dedicated libraries for pathing (`PathLibrary`), validation (`ValidationLibrary`), and type management (`TypeLibrary`).
- **Memory Safety**: Built-in `IDisposable` support across all observable types to prevent memory leaks.

## 📦 Installation
### Via Unity Package Manager (UPM)
1. Open the **Window > Package Manager** in Unity.
2. Click the **+** icon and select **Add package from git URL...**
3. Paste the following URL: 

```plaintext
https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage
```

### Via Manifest Json
1. Open your project folder and locate the `Packages` folder.
2. Open `manifest.json`.
3. Add the following to the dependencies list:

```json
"com.janito.editorextras": "https://github.com/Janfra/Janito-Unity-Packages.git?path=/JanitoEditorExtrasPackage"
```

## Requirements
- **Unity Version:** 2022.3 or higher.

## 🛠 Usage
### 1. Using Observables
Declare an observable to monitor data changes. Remember to dispose of it to clean up event subscriptions.

```csharp
using Janito.EditorExtras.Observables;

public class MyComponent : MonoBehaviour {
    private ObservableValue<int> m_Health = new ObservableValue<int>();

    void Start() {
        m_Health.OnValueChanged += (newVal) => Debug.Log($"Health is now: {newVal}");
        m_Health.Value = 100;
    }

    void OnDestroy() => m_Health.Dispose();
}
```

### 2. Inspector Type Selection
Expose a searchable dropdown for interfaces or abstract classes assigned via `[SerializeReference]`.

```csharp
using Janito.EditorExtras;

[SerializeReference, ChildTypeSelection(typeof(IMyInterface))]
private IMyInterface m_Logic;

[SerializeReference, ChildTypeSelection(typeof(BaseClass))]
private List<BaseClass> m_ClassList;
```

### 3. Type Discovery
Query the assembly using the `TypeLibrary` and `TypeCriteria` filters.

```csharp
using Janito.EditorExtras.Editor;

// Example: Find all non-abstract classes inheriting from BaseClass
var criteria = new TypeCriteria().ExcludeAbstract();
var childrenTypes = TypeLibrary.GetChildTypes<BaseClass>(criteria);
```

## 📜 Changelog
### - 2026-02-13 (v1.0.4)
- **Added:** Interface support for `ChildTypeSelectionAttribute`.
- **Added:** Fluent API for `TypeCriteria` filtering.
- **Changed [Breaking]:** Renamed `GetCachedEnumerableOfTypeChildren` to `GetChildTypes`.
- **Changed [Breaking]:** Moved `TypeLibrary` and `TypeCriteria` to `Janito.EditorExtras.Editor`.
- **Changed:** `TypeCriteria` decoupled to support `IList<T>` filtering.

### - 2026-02-13 (v1.0.3)
- **Added:** `Janito.EditorExtras.Observables` namespace and core types.
- **Added:** `IDisposable` support for memory management.
- **Internal:** Integrated NUnit test suites for observables.

### [View Full Changelog](./CHANGELOG.md)

## 📄 License
This project is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License.
See the [LICENSE](./LICENSE.md) file for details.