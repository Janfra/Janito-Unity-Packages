using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    [CreateAssetMenu(fileName = "Void Event Channel", menuName = "Scriptable Objects/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField]
        private string m_DeveloperDescription = "This is a void event channel. It can be used to raise events without any parameters.";
#endif

        public event UnityAction OnEventRaised;

        [Button(ButtonExecutionModes.PlayMode)]
        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
