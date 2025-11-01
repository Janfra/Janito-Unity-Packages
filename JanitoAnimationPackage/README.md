# Audio Package
My animation reusable tools, components and scriptable objects! Learning about creating and using custom packages. 
This package was transferred from the `Turning-Point` repository and converted into a Unity package format.

License URL: https://creativecommons.org/licenses/by-sa/4.0/

## Includes
- `AudioSourceModifierComponent`: Wrapper for Unity `AudioSource` that provides additional functionality in the form of being able to lock the component to not allow playing any new sounds and support for the audio source modifiers.

- `IAudioSourceModifier`: Interface for all audio source modifiers intended to be used with the `AudioSourceModifierComponent`. When applied, they change the audio source component in some way.

- `AudioSourceModifier`: Base class for scriptable objects that modify the audio source component. Useful for immutable changes. Implements the `IAudioSourceModifier` interface.

- `AudioPitchRandomizer`: Defines the range in which a value will be randomly picked to set the audio source pitch to when applied.

- `AudioVolumeRandomizer`: Defines the range in which a value will be randomly picked to set audio source volume to when applied.

- `AudioSourceModifiersContainer`: Contains an immutable list of `IAudioSourceModifier` to apply to the audio source. Guarantees the order and length of the list will be maintained once created, however the referenced values are not guaranteed to be immutable themselves. 

- `AudioSourceModifiersContainerConfiguration`: Scriptable object capable of creating `AudioSourceModifiersContainer`. Provides a way to define a container of modifiers. It does not cache the container, creating a new one each time.