using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public abstract class EventChannelListener<TEvent, T1> : MonoBehaviour
        where TEvent : EventChannelSO<T1>
    {
        [CreateButton]
        [SerializeField]
        private TEvent m_Channel = default;

        [Space]
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

    public abstract class BaseEventChannelListener<T1> : MonoBehaviour
    {
        [SerializeField]
        private EventChannelSO<T1> m_Channel = default;

        [Space]
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

    public abstract class EventChannelListener<TEvent, T1, T2> : MonoBehaviour
    where TEvent : EventChannelSO<T1, T2>
    {
        [CreateButton]
        [SerializeField]
        private TEvent m_Channel = default;

        [Space]
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

    public abstract class BaseEventChannelListener<T1, T2> : MonoBehaviour
    {
        [SerializeField]
        private EventChannelSO<T1, T2> m_Channel = default;

        [Space]
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

    public abstract class EventChannelListener<TEvent, T1, T2, T3> : MonoBehaviour
where TEvent : EventChannelSO<T1, T2, T3>
    {
        [CreateButton]
        [SerializeField]
        private TEvent m_Channel = default;

        [Space]
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

    public abstract class BaseEventChannelListener<T1, T2, T3> : MonoBehaviour
    {
        [SerializeField]
        private EventChannelSO<T1, T2, T3> m_Channel = default;

        [Space]
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
