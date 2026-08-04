using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        public event UnityAction<T> OnEventRaised;
        public void RaiseEvent(T value)
        {
            OnEventRaised?.Invoke(value);
        }
    }

    public abstract class EventChannelSO<T1, T2> : ScriptableObject
    {
        public event UnityAction<T1, T2> OnEventRaised;
        public void RaiseEvent(T1 value1, T2 value2)
        {
            OnEventRaised?.Invoke(value1, value2);
        }
    }

    public abstract class EventChannelSO<T1, T2, T3> : ScriptableObject
    {
        public event UnityAction<T1, T2, T3> OnEventRaised;
        public void RaiseEvent(T1 value1, T2 value2, T3 value3)
        {
            OnEventRaised?.Invoke(value1, value2, value3);
        }
    }
}
