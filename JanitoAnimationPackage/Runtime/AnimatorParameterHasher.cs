using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterHasher", menuName = "Scriptable Objects/Animation/Animator Parameter Hasher")]
    public class AnimatorParameterHasher : ScriptableObject
    {
        [SerializeField]
        private string m_ParameterName = string.Empty;
        [SerializeField]
        private AnimatorControllerParameterType m_Type;

        [SerializeField]
        private int? m_ID;
        public int ID => GetID();
        public string ReadableParameterName => m_ParameterName;
        public bool IsValid => m_ID != null;
        public AnimatorControllerParameterType Type => m_Type;

        public void Initialise(string parameterName, AnimatorControllerParameterType type)
        {
            if (!string.IsNullOrEmpty(m_ParameterName)) return;

            m_ParameterName = parameterName;
            m_Type = type;
            m_ID = Animator.StringToHash(m_ParameterName);
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
                    if (parameter.type != m_Type)
                    {
                        Debug.LogWarning($"Animator parameter '{m_ParameterName}' type mismatch. Expected: {m_Type}, Found: {parameter.type} in Animator '{animator.runtimeAnimatorController.name}'");
                    }

                    return true;
                }
            }
            return false;
        }

        private int GetID()
        {
            if (m_ID is int id)
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
            if (m_ParameterName.Length > 0)
            {
                m_ID = Animator.StringToHash(m_ParameterName);
            }
            else
            {
                m_ID = null;
            }
        }
    }
}