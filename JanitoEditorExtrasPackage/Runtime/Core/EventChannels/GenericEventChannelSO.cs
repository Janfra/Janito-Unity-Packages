using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public abstract class EventChannelSO<T1> : ScriptableObject
    {
#if UNITY_EDITOR
        [Header("Developement Only")]
#pragma warning disable CS0414
        [SerializeField]
        [TextArea]
        private string m_DeveloperDescription = $"This is a {typeof(T1)} event channel. It can be used to raise events with one parameter of type {typeof(T1)}.";
#pragma warning restore CS0414

        [Space]
        [SerializeField]
        private bool m_DebugLogEnabled = false;
#endif

        private event UnityAction<T1> m_OnEventRaised;
        public event UnityAction<T1> OnEventRaised
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

        public void RaiseEvent(T1 value)
        {
            TryLog($"{name} raised an event with value: {value}");
            m_OnEventRaised?.Invoke(value);
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

    public abstract class EventChannelSO<T1, T2> : ScriptableObject
    {
#if UNITY_EDITOR
        [Header("Developement Only")]
#pragma warning disable CS0414
        [SerializeField]
        [TextArea]
        private string m_DeveloperDescription = $"This is a {typeof(T1)}, {typeof(T2)} event channel. It can be used to raise events with two parameters of type {typeof(T1)}, {typeof(T2)}.";
#pragma warning restore CS0414

        [Space]
        [SerializeField]
        private bool m_DebugLogEnabled = false;
#endif

        private event UnityAction<T1, T2> m_OnEventRaised;
        public event UnityAction<T1, T2> OnEventRaised
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

        public void RaiseEvent(T1 value1, T2 value2)
        {
            TryLog($"{name} raised an event with values: {value1}, {value2}");
            m_OnEventRaised?.Invoke(value1, value2);
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

    public abstract class EventChannelSO<T1, T2, T3> : ScriptableObject
    {
#if UNITY_EDITOR
        [Header("Developement Only")]
#pragma warning disable CS0414
        [SerializeField]
        [TextArea]
        private string m_DeveloperDescription = $"This is a {typeof(T1)}, {typeof(T2)}, {typeof(T3)} event channel. It can be used to raise events with three parameters of type {typeof(T1)}, {typeof(T2)}, {typeof(T3)}.";
#pragma warning restore CS0414

        [Space]
        [SerializeField]
        private bool m_DebugLogEnabled = false;
#endif

        private event UnityAction<T1, T2, T3> m_OnEventRaised;
        public event UnityAction<T1, T2, T3> OnEventRaised
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

        public void RaiseEvent(T1 value1, T2 value2, T3 value3)
        {
            TryLog($"{name} raised an event with values: {value1}, {value2}, {value3}");
            m_OnEventRaised?.Invoke(value1, value2, value3);
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
