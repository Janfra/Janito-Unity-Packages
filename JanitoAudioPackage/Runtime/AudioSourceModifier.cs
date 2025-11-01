using UnityEngine;

namespace Janito.Audio
{
    [CreateAssetMenu(fileName = "AudioSourceModifier", menuName = "Scriptable Objects/Audio/Audio Source Modifier")]
    public abstract class AudioSourceModifier : ScriptableObject, IAudioSourceModifier
    {
        public abstract void ApplyAudioSourceModification(AudioSource audioSource);
    }
}
