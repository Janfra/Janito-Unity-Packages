using UnityEngine;

namespace Janito.Audio
{
    [CreateAssetMenu(fileName = "AudioPitchRandomizer", menuName = "Scriptable Objects/Audio/Audio Pitch Randomizer")]
    public class AudioPitchRandomizer : AudioSourceModifier
    {
        [SerializeField, Range(-3, 3)]
        private float _minPitch = 1.0f;
        public float MinPitch => _minPitch;

        [SerializeField, Range(-3, 3)]
        private float _maxPitch = 1.0f;
        public float MaxPitch => _maxPitch;

        public override void ApplyAudioSourceModification(AudioSource audioSource)
        {
            SetRandomPitch(audioSource);
        }

        public void SetRandomPitch(AudioSource audioSource)
        {
            audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        }

        private void OnValidate()
        {
            RangeUtils.ValidateRangeOrder(ref _minPitch, ref _maxPitch);
        }
    }
}
