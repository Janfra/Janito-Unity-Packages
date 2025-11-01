using System.Collections.Generic;
using UnityEngine;

namespace Janito.Audio
{
    [CreateAssetMenu(fileName = "AudioSourceModifiersContainerConfiguration", menuName = "Scriptable Objects/Audio/Modifiers Container Configuration")]
    public class AudioSourceModifiersContainerConfiguration : ScriptableObject
    {
        [SerializeField]
        private AudioSourceModifier[] _scriptableModifiers;
        protected List<IAudioSourceModifier> _modifiers;

        public AudioSourceModifiersContainer CreateContainer()
        {
            if (_modifiers == null || _modifiers.Count <= 0)
            {
                return new AudioSourceModifiersContainer(_scriptableModifiers);
            }
            else
            {
                IAudioSourceModifier[] combinedModifiers = new IAudioSourceModifier[_scriptableModifiers.Length + _modifiers.Count];
                _scriptableModifiers.CopyTo(combinedModifiers, 0);
                _modifiers.CopyTo(combinedModifiers, _scriptableModifiers.Length);
                return new AudioSourceModifiersContainer(combinedModifiers);
            }
        }
    }
}
