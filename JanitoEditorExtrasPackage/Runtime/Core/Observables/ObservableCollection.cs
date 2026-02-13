using System;
using System.Collections;
using System.Collections.Generic;

namespace Janito.EditorExtras.Observables
{
    public class ObservableCollection<TCollection, TItem> : ICollection<TItem>, IReadOnlyCollection<TItem>, IDisposable
        where TCollection : ICollection<TItem>, IReadOnlyCollection<TItem>, new()
    {
        public event Action OnCleared;
        public event Action<TItem> OnItemAdded;
        public event Action<TItem> OnItemRemoved;

        public bool IsReadOnly => m_Collection.IsReadOnly;
        public IReadOnlyCollection<TItem> ReadOnlyCollection => m_Collection;
        public int Count => ReadOnlyCollection.Count;

        protected TCollection InternalCollection => m_Collection;
        private TCollection m_Collection;

        public ObservableCollection(TCollection collection = default) => m_Collection = collection ?? new();

        public void Add(TItem item)
        {
            m_Collection.Add(item);
            NotifyAddChange(item);
        }

        public bool Remove(TItem item)
        {
            var wasRemoved = m_Collection.Remove(item);
            if (wasRemoved)
            {
                NotifyRemoveChange(item);
            }

            return wasRemoved;
        }

        public void Clear()
        {
            if (Count == 0) return;

            m_Collection.Clear();
            OnCleared?.Invoke();
        }

        public void UnsubscribeAll()
        {
            OnCleared = null;
            OnItemAdded = null;
            OnItemRemoved = null;
            OnUnsubscribeAll();
        }

        public void AddWithoutNotify(TItem item) => m_Collection.Add(item);
        public bool RemoveWithoutNotify(TItem item) => m_Collection.Remove(item);
        public bool Contains(TItem item) => m_Collection.Contains(item);
        public void CopyTo(TItem[] array, int arrayIndex) => m_Collection.CopyTo(array, arrayIndex);
        public IEnumerator<TItem> GetEnumerator() => m_Collection.GetEnumerator();
        public void Dispose()
        {
            UnsubscribeAll();
            m_Collection.Clear();
            OnDispose();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        /// <summary>
        /// Executed when <c>Dispose</c> method is executed. 
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// Executed when <c>UnsubscribeAll</c> or <c>Dispose</c> is executed.
        /// </summary>
        protected virtual void OnUnsubscribeAll() { }
        protected void NotifyAddChange(TItem item) => OnItemAdded?.Invoke(item);
        protected void NotifyRemoveChange(TItem item) => OnItemRemoved?.Invoke(item);
    }
}
