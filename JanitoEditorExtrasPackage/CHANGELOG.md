# Changelog

## Unreleased - 2026-07-08
### Added
- **Button Attribute**: Added attribute `ButtonAttribute` to draw a button in the inspector that executes the target parameterless method on click. Buttons can be set to only be valid in certain application modes.
- **Log Alternative**: Added set of functions in `LogLibrary` to allow for prefix logging with objects that do not derive from `UnityEngine.Object`.

### Internal
- **Conditional Attributes**: Replaced conditional compilation with conditional attributes for readability.

## [1.0.5] - 2026-05-14
### Added
- **Read Only Inspector**: Added attribute `ReadOnlyAttribute` to disable editing a property in the inspector, allowing to expose information in the inspector for debugging purposes without the risk of accidental editing.
- **Log Library**: Added a set of extension methods to `UnityEngine.Object` to prefix logged messages with the type name for easier identification. Includes equivalent alternatives that only log in editor and development builds.
- **Singletons System**: Introduced the `Janito.EditorExtras.Singletons` namespace, containing generic types for defining singleton classes.
- **Lazy Singleton**: Added `LazySingleton<TDerived>` to automatically generate the singleton instance if no valid instance is already set.
- **Mono Singleton**: Added `MonoSingleton<T>` for `Component` types in order to assign the `Instance` reference on `Awake()` or in the case of not having a valid reference, generate a `GameObject` with the required component. It can be configured to persist in between scenes.
 
## [1.0.4] - 2026-02-13
### Added
- **Interface Support**: Enabled interfaces support for `ChildTypeSelectionAttribute`, allowing interfaces to be used as base types for selection.
- **Fluent Type Criteria**: Introduced a fluent API for `TypeCriteria` via extension methods, removing the need for manual bitwise operations.

### Changed
- **Namespace Reorganisation [Breaking]**: Moved `TypeLibrary` and `TypeCriteria` into the `Janito.EditorExtras.Editor` namespace.
- **Type Criteria Moved [Breaking]**: Moved `TypeCriteria` to be outside of `TypeLibrary`, contained within `Janito.EditorExtras.Editor` namespace. 
- **API Renaming [Breaking]**: Renamed `GetCachedEnumerableOfTypeChildren` to `GetChildTypes` to provide a more concise and readable entry point.
- **IList Filtering**: Refactored `TypeCriteria` to accept `IList<T>` instead of specific `List<T>` implementations, improving decoupling.

### Internal
- **Deprecation Path**: Marked `GetEnumerableOfTypeChildren` as `[Obsolete]` with explicit compiler warnings directing users toward the `GetChildTypes` functions.

## [1.0.3] - 2026-02-13
#### Generic Observables & Unit Tests

### Added
- **Observables System**: Introduced the `Janito.EditorExtras.Observables` namespace, containing generic types that trigger events upon data changes.
- **Observable Value**: Added `ObservableValue<T>` to monitor and notify of changes on an individual values.
- **Observable Collection**: Added `ObservableCollection<TCollection, TItem>` to monitor and notify of changes on collections.
- **Observable IList**: Added `ObservableIList<TList, TItem>` (extending `ObservableCollection`) to support generic `IList<T>` types.
- **Observable List**: Added `ObservableList<TItem>` (extending `ObservableIList`) specifically for `List<T>` types.
- **Lifecycle Management**: Implemented `IDisposable` and `UnsubscribeAll()` across observable types to prevent memory leaks and simplify event cleanup.

### Internal
- **Unit Testing**: Implemented NUnit-based tests (`ObservableValueTests`, `ObservableCollectionTests`, and `ObservableIListTests`) to ensure that code regression does not occur.

## [1.0.2] - 2026-01-03
### Added
- **Type Caching**: Integrated `TypeCache` into `TypeLibrary` for high-performance type discovery and filtering via `TypeCriteria`. *Note: Old functions still exist, but are not used internally anymore.*
- **Criteria Filter**: Added a method to filter arbitrary type lists based on constraints defined in `TypeCriteria`.
- **Validation Library**: Added a set of utility functions within `ValidationLibrary` to validate object references, providing explicit exception handling on condition failure.
- **Path Library**: Added a set of utility functions within `PathLibrary` to retrieve project-relative folder paths, wrapping Unity's file explorer APIs for easier asset pathing.
- **Namespace Display**: Type options in `ChildTypeSelectionAttribute` now include namespaces to distinguish between duplicate class names in different scopes.

### Changed
- **Library Renaming [Breaking]**: Renamed `EditorExtrasLibrary` to `TypeLibrary` to more accurately reflect its purpose of handling type discovery and filtering.
- **Project Restructuring [Breaking]**: Reorganized the Editor and Runtime folders into subfolders (Drawers, Libraries, Attributes) for better maintainability.
- **Performance Optimization**: Updated `ChildTypeSelectionAttribute` to utilize cached discovery methods from `TypeLibrary`.
- **Type Selection Overhaul**: Updated `ChildTypeSelectionAttribute` with a case-insensitive search bar, namespace display for duplicate types, and a refined UI layout.
- **Attribute Stripping**: Decorated `ChildTypeSelectionAttribute` with `[Conditional("UNITY_EDITOR")]` to ensure it is stripped from runtime builds.

## [1.0.1] - 2025-12-31
#### Array & List Support

### Added
- **Collection Support**: Added support for Array and List elements to `ChildTypeSelectionAttribute`, providing a dropdown field for each individual element.
- **Strict Base Type Validation**: Added fail-fast validation to throw an `ArgumentException` if the requested base type inherits from `UnityEngine.Object` (as `[SerializeReference]` only supports non-Unity objects).

### Fixed
- **Type Filter Logic**: Fixed a typo in the invalid type filter that caused all types to be removed. It now correctly excludes only classes inheriting from `UnityEngine.Object`.

## [1.0.0] - 2025-12-30
#### First Release

###  Added   
- **Type Selection Attribute**: Added attribute `ChildTypeSelectionAttribute` to expose a dropdown field on the Inspector for assigning types to `[SerializeReference]` fields.
- **TypeCriteria Structure**: Added support for bidirectional constraints (e.g., `Abstract` vs `NotAbstract`) when filtering types.
- **Type Discovery Library**: Added a set of utility functions within `EditorExtrasLibrary` to retrieve and filter child types, integrated with the `TypeCriteria` system.
- **Fail-Fast Configuration Validation**: Added explicit validation within `TypeCriteria` that throws an `ArgumentException` if contradictory requirements (including and excluding the same flag) are provided during construction.
