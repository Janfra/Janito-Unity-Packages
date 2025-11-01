using UnityEngine;

namespace Janito.Audio
{
    public interface IAudioSourceModifier
    {
        public abstract void ApplyAudioSourceModification(AudioSource audioSource);
    }
}
