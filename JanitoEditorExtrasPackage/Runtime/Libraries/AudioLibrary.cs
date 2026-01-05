using UnityEngine;

namespace Janito.EditorExtras.Audio
{
    public static class AudioLibrary
    {
        public static float GetTimeFromSamples(AudioClip clip, float sampleNumber)
        {
            return sampleNumber / clip.frequency;
        }

        public static int GetSampleAtDuration(AudioClip clip, float duration) 
        {
            return (int)duration * clip.frequency;
        }
    }
}
