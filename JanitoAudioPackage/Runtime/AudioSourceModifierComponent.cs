using UnityEngine;

namespace Janito.Audio
{
    public class AudioSourceModifierComponent : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private bool _isLocked = false;

        private void Awake()
        {
            if (_audioSource == null)
            {
                throw new System.NullReferenceException($"AudioSourceModifierComponent in {name} requires a reference to an AudioSource component.");
            }
        }

        public void PlayWithRandomPitch(float minPitch, float maxPitch)
        {
            if (!CanPlay())
            {
                return;
            }

            _audioSource.pitch = Random.Range(minPitch, maxPitch);
            _audioSource.Play();
        }

        public void PlayWithRandomPitch(AudioPitchRandomizer pitchRandomizer)
        {
            if (!CanPlay())
            {
                return;
            }

            pitchRandomizer?.SetRandomPitch(_audioSource);
            _audioSource.Play();
        }

        public void ApplyModifiers(AudioSourceModifiersContainerConfiguration modifiersContainerConfiguration)
        {
            AudioSourceModifiersContainer modifiersContainer = modifiersContainerConfiguration.CreateContainer();
            modifiersContainer.ApplyModifiers(_audioSource);
        }

        public void ApplyModifiers(AudioSourceModifiersContainer modifiersContainer)
        {
            modifiersContainer?.ApplyModifiers(_audioSource);
        }

        public void ApplyAudioSourceModification(AudioSourceModifier modifier)
        {
            modifier?.ApplyAudioSourceModification(_audioSource);
        }

        public void ApplyAudioSourceModification(IAudioSourceModifier modifier)
        {
            modifier?.ApplyAudioSourceModification(_audioSource);
        }

        public void SetAudioLock(bool isLocked)
        {
            _isLocked = isLocked;
        }

        public bool CanPlay()
        {
            return !_isLocked;
        }
    }
}
