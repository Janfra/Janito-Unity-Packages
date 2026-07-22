# Changelog

## [1.0.1] - 2026-07-22
### Fix
- **Missing Context**: Added check to avoid `_isLogging` not existing during compilation, leading to missing context failing builds.

## [1.0.0] - 2026-07-08
#### First Release

> **Note:** This package was created and tested utilising Unity Input System v1.19.0.

### Added
**Namespace:** `Janito.Prototyping.Input`

- **BaseInputManager**: A generic wrapper for Unity's auto-generated Input Actions class. Provides type safety and customisation while mimicking some of the `PlayerInput` functionality.
    - *Note*: Restricted to one component per `GameObject`. However, multiple distinct implementations can coexist on the same object due to the generic base.
- **Cursor Awake Settings**: Cursor state is configurable within `BaseInputManager` setting them on `Awake`.
- **Custom Enabling**: Logic in `BaseInputManager` to optionally synchronise the Input Action Map state with the `GameObject` enable/disable cycle.
- **BaseInputBroadcaster**: An abstract wrapper class for `InputAction` that expects implementations to expose Unity Events for `Started`, `Performed`, and `Canceled` states.
- **InputBroadcaster**: A non-generic implementation of `BaseInputBroadcaster` that triggers Unity Events without passing a value.
- **Generic InputBroadcaster**: A generic implementation of `BaseInputBroadcaster` that invokes Unity Events with the value read from the input (based on the specified generic type).
- **Individual Action Control**: Enabling/disabling logic for individual `InputActions` within `BaseInputBroadcaster`, including an option to persist state across `GameObject` re-activation.
- **Generated Unity Events**: Integrated `BaseInputBroadcaster` into `BaseInputManager` to automatically expose Unity Events for all actions in an `InputActionAsset`. 
    - *Note*: Events are generated during Editor, not at runtime.
