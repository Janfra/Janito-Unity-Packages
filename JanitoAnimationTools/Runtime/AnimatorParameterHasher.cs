using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterHasher", menuName = "Scriptable Objects/Animation/Animator Parameter Hasher")]
    public class AnimatorParameterHasher : ScriptableObject
    {
        [SerializeField]
        private string _parameterName;
        [SerializeField]
        private AnimatorControllerParameterType _type;

        public int ID => _id;
        private int _id;
        private bool _isInitialised = false;

        private void OnEnable()
        {
            Initialise();
        }

        public void Initialise(bool isForced = false)
        {
            if (!_isInitialised || isForced)
            {
                if (_parameterName.Length > 0)
                {
                    _id = Animator.StringToHash(_parameterName);
                }
    #if UNITY_EDITOR
                else if (Application.isPlaying)
                {
                    throw new System.NullReferenceException($"Parameter name for Animator is null or empty. Please set a value before usage for {name}");
                }
    #endif
            }
        }

        public bool HasParameter(Animator animator)
        {
            if (!_isInitialised)
            {
                Initialise();
            }

            foreach (var parameter in animator.parameters)
            {
                if (parameter.nameHash == ID)
                {
                    if (parameter.type != _type)
                    {
                        Debug.LogWarning($"Animator parameter '{_parameterName}' type mismatch. Expected: {_type}, Found: {parameter.type} in Animator '{animator.runtimeAnimatorController.name}'");
                    }

                    return true;
                }
            }
            return false;
        }
    }
}