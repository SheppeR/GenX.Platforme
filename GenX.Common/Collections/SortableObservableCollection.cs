using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Threading;

namespace GenX.Common.Collections;

public class DispatcherSortedObservableCollection<T>(IComparer<T> comparer) : ObservableCollection<T>
{
    private readonly IComparer<T> _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

    public new void Add(T item)
    {
        if (_dispatcher.CheckAccess())
        {
            base.Add(item);
            Sort();
        }
        else
        {
            _dispatcher.Invoke(() => base.Add(item));
            _dispatcher.Invoke(Sort);
        }
    }

    public new bool Remove(T item)
    {
        if (_dispatcher.CheckAccess())
        {
            return base.Remove(item);
        }

        return _dispatcher.Invoke(() => base.Remove(item));
    }

    public new void Clear()
    {
        if (_dispatcher.CheckAccess())
        {
            base.Clear();
        }
        else
        {
            _dispatcher.Invoke(base.Clear);
        }
    }

    public void Sort()
    {
        var items = this.ToList();
        items.Sort(_comparer);

        base.ClearItems();
        foreach (var item in items)
        {
            base.Add(item);
        }

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}