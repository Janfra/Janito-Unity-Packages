using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public abstract class EventChannelListener<T1> : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private string m_DeveloperDescription = $"This is a {typeof(T1)} event channel. It can be used to raise events with one parameter of type {typeof(T1)}.";
#endif

        [SerializeField]
        private EventChannelSO<T1> m_Channel = default;

        public UnityEvent<T1> OnEventRaised;

        private void OnEnable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised += Respond;
            }
        }

        private void OnDisable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised -= Respond;
            }
        }

        private void Respond(T1 arg1)
        {
            OnEventRaised?.Invoke(arg1);
        }
    }

    public abstract class EventChannelListener<T1, T2> : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private string m_DeveloperDescription = $"This is a {typeof(T1)}, {typeof(T2)} event channel. It can be used to raise events with two parameters of type {typeof(T1)}, {typeof(T2)}.";
#endif

        [SerializeField]
        private EventChannelSO<T1, T2> m_Channel = default;

        public UnityEvent<T1, T2> OnEventRaised;

        private void OnEnable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised += Respond;
            }
        }

        private void OnDisable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised -= Respond;
            }
        }

        private void Respond(T1 arg1, T2 arg2)
        {
            OnEventRaised?.Invoke(arg1, arg2);
        }
    }

    public abstract class EventChannelListener<T1, T2, T3> : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private string m_DeveloperDescription = $"This is a {typeof(T1)}, {typeof(T2)}, {typeof(T3)} event channel. It can be used to raise events with three parameters of type {typeof(T1)}, {typeof(T2)}, {typeof(T3)}.";
#endif

        [SerializeField]
        private EventChannelSO<T1, T2, T3> m_Channel = default;

        public UnityEvent<T1, T2, T3> OnEventRaised;

        private void OnEnable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised += Respond;
            }
        }

        private void OnDisable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised -= Respond;
            }
        }

        private void Respond(T1 arg1, T2 arg2, T3 arg3)
        {
            OnEventRaised?.Invoke(arg1, arg2, arg3);
        }
    }
}
