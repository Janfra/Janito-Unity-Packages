# Changelog

## [1.0.0] - 2025-12-30
#### First Release

###  Added   
- **Type Selection Attribute**: New attribute to expose a dropdown field on the Inspector for assigning types to `[SerializeReference]` fields.
- **Type Discovery Library**: Set of utility functions to retrieve and filter child types, integrated with the `TypeCriteria` system.
- **TypeCriteria Structure**: Support for bidirectional constraints (e.g., `Abstract` vs `NotAbstract`) when filtering types.
- **Fail-Fast Configuration Validation**: Explicit validation within `TypeCriteria` that throws an `ArgumentException` if contradictory requirements (including and excluding the same flag) are provided during construction.