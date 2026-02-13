using System;

namespace Janito.EditorExtras.Observables
{
    public sealed class ObservableValue<T> : IDisposable
    {
        public event Action<T> OnValueChanged;
        public T Value 
        {
            get { return m_Value; }
            set { ChangeValue(value); }
        }

        private T m_Value;

        public ObservableValue(T value = default) 
        {
            m_Value = value;
        }

        public void ChangeWithoutNotify(T value) => m_Value = value;

        public void UnsubscribeAll()
        {
            OnValueChanged = null;
        }

        public void Dispose()
        {
            UnsubscribeAll();
            ChangeWithoutNotify(default);
        }

        private void ChangeValue(T value)
        {
            m_Value = value;
            OnValueChanged?.Invoke(m_Value);
        }

        public static implicit operator T(ObservableValue<T> observable) => observable.Value;
        public static explicit operator ObservableValue<T>(T value) => new(value);
    }
}
