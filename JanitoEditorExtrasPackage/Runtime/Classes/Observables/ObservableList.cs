using System.Collections.Generic;

namespace Janito.EditorExtras.Observables
{
    public class ObservableList<TItem> : ObservableIList<List<TItem>, TItem>
    {
        protected ObservableList(List<TItem> collection) : base(collection) { }
    }
}
