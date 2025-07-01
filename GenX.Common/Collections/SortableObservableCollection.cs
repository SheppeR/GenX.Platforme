using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GenX.Network.Packets.FriendsDatas;

namespace GenX.Common.Collections;

public class SortedObservableCollection<T>(IComparer<T> comparer) : ObservableCollection<T>
{
    private readonly IComparer<T> _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);

        if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        {
            Sort();
        }
    }

    public void Sort()
    {
        var items = Items.ToList();
        items.Sort(_comparer);
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(item);
        }

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}

public class FriendResultComparer : IComparer<FriendDatas>
{
    public int Compare(FriendDatas? x, FriendDatas? y)
    {
        if (x == null || y == null)
        {
            return x == null ? y == null ? 0 : -1 : 1;
        }

        if (x.IsAccepted != y.IsAccepted)
        {
            return x.IsAccepted ? 1 : -1;
        }

        var xStatusPriority = x.Status == 3 ? 0 : 1;
        var yStatusPriority = y.Status == 3 ? 0 : 1;
        if (xStatusPriority != yStatusPriority)
        {
            return xStatusPriority.CompareTo(yStatusPriority);
        }

        var xPseudo = x.Pseudo ?? string.Empty;
        var yPseudo = y.Pseudo ?? string.Empty;
        return string.Compare(xPseudo, yPseudo, StringComparison.OrdinalIgnoreCase);
    }
}