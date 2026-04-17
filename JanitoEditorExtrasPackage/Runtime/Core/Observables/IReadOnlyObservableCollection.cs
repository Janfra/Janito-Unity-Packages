using System;
using System.Collections.Generic;
using UnityEngine;

namespace Janito.EditorExtras
{
    public interface IReadOnlyObservableCollection<TItem>
    {
        public event Action OnCleared;
        public event Action<TItem> OnItemAdded;
        public event Action<TItem> OnItemRemoved;

        public IReadOnlyCollection<TItem> ReadOnlyCollection { get; }
    }
}
