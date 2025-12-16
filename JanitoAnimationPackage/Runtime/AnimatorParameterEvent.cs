using UnityEngine;

namespace Janito.Animations
{
    public abstract class AnimatorParameterEvent : ScriptableObject
    {
        [SerializeField]
        private AnimatorParameterHasher _parameter;
        public AnimatorParameterHasher Parameter => _parameter;

        public abstract void ApplyEventValue(AnimatorModifierComponent modifierComponent);

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                if (_parameter == null)
                {
                    Debug.LogWarning($"Animator Parameter is null on AnimatorParameterEvent ScriptableObject: {name}", this);
                }
            }
        }
#endif
    }
}