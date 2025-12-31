# Changelog

## [1.0.2] - 2025-12-31
### Added
- **Animator Value Extraction Window**: Added an Editor Window to automatically create an `AnimatorParameterHasher` for each requested parameter of the provided `AnimatorController`.
- **Extension Methods For Animator**: Added extension methods to the Unity Animator to be able to directly utilise `AnimatorParameterHasher` without requiring an `AnimatorModifierComponent`.
- **Animator Parameter Listener System**: Added `AnimatorParameterValueListener` to allow for frame-by-frame or custom parameter monitoring via `AnimatorModifierComponent`.
- **Automatic Class-Detection System**: Added automatically exposing new valid classes added for the `AnimatorParameterValueListener` in the `AnimatorModifierComponent`.
- **Animator Parameter Velocity Listener**: Added `AnimatorParameterVelocityListener` utilising the `AnimatorParameterValueListener` system to set a float parameter each frame to match the squared linear velocity of a Rigidbody component.
- **Package Dependency**: Added `Janito Editor Extras` as a required dependency to provide Inspector attributes and core functions for the `Value Extraction Window`.

### Changed
- **Automatic Component Fetching**: Added automatically setting the Animator reference on the `AnimatorModifierComponent` on validation, removing the need of setting it on being added to a game object. 

### Internal
- **Changelog Standardization**: Updated format to include bolded identifiers, specific, accurate names for better readability and clearer referencing of systems and objects and standardized sectioning (Added, Changed, Internal) across all versions. Previous entries have been retrospectively updated to follow these changes, in some cases rewritten entirely.

## [1.0.1] - 2025-12-16
#### Animator Parameter Hasher Update

### Added
- **Parameter Hashing Validation**: Added a `IsValid` property to verify that a valid hash, referred to as `ID`, is contained within the `AnimatorParameterHasher`.

### Changed
- **Invalid Logging**: Added invalid logs inside the console when attempting to utilise an invalid `AnimatorParameterHasher`.
- **Hashed Cached On Validation**: Hash is now generated and stored on validation as the parameter name is set instead of when the asset is enabled.

## [1.0.0] - 2025-11-01
#### First Release

## Added
- **Animator Modifier Component**: Added `AnimatorModifierComponent`, a wrapper that provides methods for modifying Animator states via code or Unity events.
- **Animator Parameter Hashing**: Added `AnimatorParameterHasher` assets to handle parameter hashing in the Editor. Replaces string-based access with reusable hashes to improve performance and prevent "parameter not found" errors.
- **Parameter Event System**: Added `AnimatorParameterEvent` assets to trigger predefined animator changes automatically. Fully compatible with the new hashing system and `AnimatorModifierComponent`.
- **Bool Parameter Event**: Added `AnimatorBoolParameterEvent` utilising the `AnimatorParameterEvent` system.
- **Trigger Parameter Event**: Added `AnimatorTriggerParameterEvent` utilising the `AnimatorParameterEvent` system.