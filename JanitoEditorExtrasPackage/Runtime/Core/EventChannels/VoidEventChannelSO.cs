using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    [CreateAssetMenu(fileName = "Void Event Channel", menuName = "Scriptable Objects/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event UnityAction OnEventRaised;

        [Button(ButtonExecutionModes.PlayMode)]
        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
