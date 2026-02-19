using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Janito.Animations
{
    public abstract class AnimatorParameterEvent : ScriptableObject
    {
        [SerializeField]
        private AnimatorParameterHasher m_Parameter;
        public AnimatorParameterHasher Parameter => m_Parameter;

        /// <summary>
        /// Initialises newly created event with the specified animator parameter, if it has not already been initialised. Reinitialisation is not allowed and will be ignored with a warning.
        /// </summary>
        /// <remarks>If the event has already been initialised with a parameter, subsequent calls to this
        /// method are ignored and a warning is logged. The event can only be initialised once.</remarks>
        /// <param name="parameter">The animator parameter to associate with this event. Must not be null.</param>
        public void Initialise(AnimatorParameterHasher parameter)
        {
            if (m_Parameter != null)
            {
                Debug.LogWarning($"{nameof(AnimatorParameterEvent)} '{name}' is already initialised with parameter '{m_Parameter.name}'. Reinitialisation with new parameter name '{parameter.name}' is not allowed. Ignoring new parameter.", this);
                return;
            }

            m_Parameter = parameter;
        }

        public abstract void ApplyEventValue(AnimatorModifierComponent modifierComponent);

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                if (m_Parameter == null)
                {
                    Debug.LogWarning($"Animator Parameter is null on {nameof(AnimatorParameterEvent)} ScriptableObject: {name}", this);
                }
            }
        }
#endif
    }
}