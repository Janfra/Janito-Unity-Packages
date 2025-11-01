using UnityEngine;

namespace Janito.Audio
{
    [CreateAssetMenu(fileName = "AudioVolumeRandomizer", menuName = "Scriptable Objects/Audio/Audio Volume Randomizer")]
    public class AudioVolumeRandomizer : AudioSourceModifier
    {
        [SerializeField, Range(0.0f, 1.0f)]
        private float _minVolume = 1.0f;
        public float MinVolume => _minVolume;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _maxVolume = 1.0f;
        public float MaxVolume => _maxVolume;

        public override void ApplyAudioSourceModification(AudioSource audioSource)
        {
            SetRandomVolume(audioSource);
        }

        public void SetRandomVolume(AudioSource audioSource)
        {
            audioSource.volume = Random.Range(_minVolume, _maxVolume);
        }

        private void OnValidate()
        {
            RangeUtils.ValidateRangeOrder(ref _minVolume, ref _maxVolume);
        }
    }
}
