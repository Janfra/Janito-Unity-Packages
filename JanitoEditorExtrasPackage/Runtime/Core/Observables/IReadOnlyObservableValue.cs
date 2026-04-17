using System;
using UnityEngine;

namespace Janito.EditorExtras
{
    public interface IReadOnlyObservableValue<T>
    {
        public event Action<T> OnValueChanged;
        public T Value { get; }
    }
}
