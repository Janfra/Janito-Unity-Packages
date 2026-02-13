# Changelog

## [1.0.3] - 2026-02-13
#### Generic Observables & Unit Tests

### Added
- **Observables System**: Introduced the `Janito.EditorExtras.Observables` namespace, containing generic types that trigger events upon data changes.
- **Observable Value**: Added `ObservableValue<T>` to monitor and notify of changes on an individual field.
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
