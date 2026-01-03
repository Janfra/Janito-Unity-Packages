using UnityEngine;

namespace Janito.Animations
{
    public abstract class AnimatorParameterEvent : ScriptableObject
    {
        [SerializeField]
        private AnimatorParameterHasher m_Parameter;
        public AnimatorParameterHasher Parameter => m_Parameter;

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