# Changelog

## [1.0.1] - 2025-12-16
### Animator Parameter Hasher Update
- Replaces the old process of updating the ID of the parameter name on enabling with a new process performed on 
- Adds a property to verify that an ID is contained within an Animator Parameter Hasher
- Adds logging when using an invalid Animator Parameter Hasher

## [1.0.0] - 2025-11-01
### First Release
- Includes an Animator Modifier Component that wraps Unity's Animator component to expose ways of modifying the animator
- Includes Animator Parameter Hasher to define which parameter in the animator will be modified
- Includes Animator Parameter Events for Bool and Trigger animator parameter types to apply a predefined value to the animator