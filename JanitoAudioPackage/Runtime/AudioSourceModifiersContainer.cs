using System;
using System.Collections.Generic;
using UnityEngine;

namespace Janito.Audio
{
    public sealed class AudioSourceModifiersContainer
    {
        public IReadOnlyList<IAudioSourceModifier> Modifiers { get; }

        public AudioSourceModifiersContainer(IAudioSourceModifier[] modifiers)
        {
            IAudioSourceModifier[] safe = modifiers ?? Array.Empty<IAudioSourceModifier>();
            IAudioSourceModifier[] copy = safe.Clone() as IAudioSourceModifier[];
            Modifiers = Array.AsReadOnly(copy);
        }

        public void ApplyModifiers(AudioSource audioSource)
        {
            foreach (IAudioSourceModifier modifier in Modifiers)
            {
                modifier?.ApplyAudioSourceModification(audioSource);
            }
        }

        public void ApplyModifiers(AudioSourceModifierComponent component) 
        {
            foreach (IAudioSourceModifier modifier in Modifiers)
            {
                component?.ApplyAudioSourceModification(modifier);
            }
        }
    }
}
