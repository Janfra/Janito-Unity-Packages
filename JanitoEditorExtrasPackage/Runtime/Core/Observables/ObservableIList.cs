using System.Collections.Generic;

namespace Janito.EditorExtras.Observables
{
    public class ObservableIList<TList, TItem> : ObservableCollection<TList, TItem>, IReadOnlyList<TItem>
        where TList : IList<TItem>, IReadOnlyCollection<TItem>, IReadOnlyList<TItem>, new() 
    {
        public ObservableIList(TList collection) : base(collection) { }

        public IReadOnlyList<TItem> ReadOnlyList => InternalCollection;

        /// <summary>
        /// Wrapper property for <c>InternalCollection</c> as <c>IList</c> interface to avoid ambiguity in between <c>IReadOnlyList</c> and <c>IList</c> during access.
        /// <see cref="ObservableCollection{TCollection, TItem}.InternalCollection"/>
        /// </summary>
        private IList<TItem> _internalList => InternalCollection;

        public TItem this[int index]
        {
            get { return _internalList[index]; }
            set { ChangeValueAt(index, value); }
        } 

        private void ChangeValueAt(int index, TItem value)
        {
            TItem initialValue = _internalList[index];
            if (initialValue != null)
            {
                NotifyRemoveChange(initialValue);
            }

            _internalList[index] = value;
            NotifyAddChange(value);
        }
    }
}
