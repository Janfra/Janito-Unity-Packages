using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    [CreateAssetMenu(fileName = "Void Event Channel", menuName = "Scriptable Objects/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
#if UNITY_EDITOR
        [Header("Developement Only")]
        [SerializeField]
        [TextArea]
        private string m_DeveloperDescription = "This is a void event channel. It can be used to raise events without any parameters.";

        [Space]
        [SerializeField]
        private bool m_DebugLogEnabled = false;
#endif

        private event UnityAction m_OnEventRaised;
        public event UnityAction OnEventRaised
        {
            add
            {
                TryLog($"{value.Method?.DeclaringType}.{value.Method?.Name} subscribed to event in {name}.");
                m_OnEventRaised += value;
            }
            remove
            {
                TryLog($"{value.Method?.DeclaringType}.{value.Method?.Name} unsubscribed from event in {name}.");
                m_OnEventRaised -= value;
            }
        }

        [Button(ButtonExecutionModes.PlayMode)]
        public void RaiseEvent()
        {
            TryLog($"{name} raised an event.");
            m_OnEventRaised?.Invoke();
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        private void TryLog(string message)
        {
            #if UNITY_EDITOR
            if (m_DebugLogEnabled)
            {
                this.LogInDevelopment(message);
            }
            #endif
        }
    }
}
