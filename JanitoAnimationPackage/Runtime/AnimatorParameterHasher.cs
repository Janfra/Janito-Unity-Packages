using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterHasher", menuName = "Scriptable Objects/Animation/Animator Parameter Hasher")]
    public class AnimatorParameterHasher : ScriptableObject
    {
        [SerializeField]
        private string _parameterName = string.Empty;
        [SerializeField]
        private AnimatorControllerParameterType _type;

        [SerializeField]
        private int? _id;
        public int ID => GetID();
        public string ReadableParameterName => _parameterName;
        public bool IsValid => _id != null;

        public void Initialise(string parameterName, AnimatorControllerParameterType type)
        {
            if (!string.IsNullOrEmpty(_parameterName)) return;

            _parameterName = parameterName;
            _type = type;
            _id = Animator.StringToHash(_parameterName);
        }

        public bool HasParameter(Animator animator)
        {
            if (!IsValid)
            {
                Debug.LogError($"Animator parameter name is empty in AnimatorParameterHasher '{name}'.");
                return false;
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

        private int GetID()
        {
            if (_id is int id)
            {
                return id;
            }
            else
            {
                Debug.LogError($"Animator parameter name is empty in AnimatorParameterHasher '{name}'. Returning ID as -1.");
                return -1;
            }
        }

        private void OnValidate()
        {
            if (_parameterName.Length > 0)
            {
                _id = Animator.StringToHash(_parameterName);
            }
            else
            {
                _id = null;
            }
        }
    }
}