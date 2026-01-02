# Changelog

## [Unreleased]
### Added
- **Validation Library**: A set of utility functions to validate references, throwing an exception on condition failure.
- **Path Library**: A set of utility functions to retrieve folder paths that are relative to the project. Wraps around Unity's libraries to open the file explorer and retrieve relative path information.

### Changed
- **Renamed Library**: Renamed `EditorExtrasLibrary` to `TypeLibrary` to better reflect the contents and purpose of the library.
- **Folder Restructured**: Created subfolders for Drawers and Libraries scripts, moved relevant files to the new folders.

### Changed
- **Restructured Runtime Folder**: Created an attributes folder to contain all runtime available attributes, moving all existing attributes inside.

## [1.0.1]
#### Array & List Support

### Added
- **Array Support on Type Selection**: Type selection now supports arrays and lists elements, displaying the dropdown field for each element.
- **Fail-Fast Type Selection Validation**: Explicit validation within `ChildTypeSelectionAttribute` that throws an `ArgumentException` if the requested base type inherits from Unity Engine Object class.

### Fixed
- **Incorrect Invalid Type**: The filter of invalid types had a typo and was removing all types since they weren't inhering from Unity Object class. Now it correctly instead removes classes inheriting from Unity Object class.

## [1.0.0] - 2025-12-30
#### First Release

###  Added   
- **Type Selection Attribute**: New attribute to expose a dropdown field on the Inspector for assigning types to `[SerializeReference]` fields.
- **Type Discovery Library**: Set of utility functions to retrieve and filter child types, integrated with the `TypeCriteria` system.
- **TypeCriteria Structure**: Support for bidirectional constraints (e.g., `Abstract` vs `NotAbstract`) when filtering types.
- **Fail-Fast Configuration Validation**: Explicit validation within `TypeCriteria` that throws an `ArgumentException` if contradictory requirements (including and excluding the same flag) are provided during construction.