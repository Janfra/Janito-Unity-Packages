# Changelog

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