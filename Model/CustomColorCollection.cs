using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CollectionViewIssue.Model;

internal class CustomReadOnlyColorCollection 
    : IReadOnlyList<NamedColor>,
      INotifyCollectionChanged, 
      INotifyPropertyChanged,
      ISelectedItem<NamedColor>
{
    readonly List<NamedColor> _items = [];
    readonly IComparer<NamedColor> _comparer = NamedColor.Comparer;
    NamedColor _selectedItem;

    public CustomReadOnlyColorCollection()
    {
    }

    #region Properties

    public int Count
    {
        get => _items.Count;
    }

    public NamedColor this[int index]
    {
        get => _items[index];
    }

    public NamedColor SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (!ReferenceEquals(value, _selectedItem))
            {
                _selectedItem = value;
                OnPropertyChanged(SelectedItemChangedEventArgs);
            }
        }
    }

    #endregion Properties

    #region Updates

    internal bool Add(NamedColor item)
    {
        int index = ItemIndex(item);
        if (index < 0)
        {
            index = ~index;
            _items.Insert(index, item);
            OnPropertyChanged(CountChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            return true;
        }
        return false;
    }

    internal bool Remove(NamedColor color)
    {
        int index = ItemIndex(color);
        if (index >= 0)
        {
            _items.RemoveAt(index);
            OnPropertyChanged(CountChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, color, index));
            return true;
        }
        return false;
    }

    internal void Clear()
    {
        if (_items.Count > 0)
        {
            List<NamedColor> removedItems = new List<NamedColor>(_items);
            _items.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, 0));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    #endregion Updates

    #region Item Index

    public int IndexOf(NamedColor item)
    {
        int index = ItemIndex(item);
        if (index >= 0)
        {
            return index;
        }
        return -1;
    }

    int ItemIndex(NamedColor item)
    {
        return _items.BinarySearch(item, _comparer);
    }

    #endregion Item Index

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
    static readonly PropertyChangedEventArgs SelectedItemChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedItem));

    void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    #endregion INotifyPropertyChanged

    #region INotifyCollectionChanged

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    static readonly NotifyCollectionChangedEventArgs ResetNotifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

    void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    #endregion INotifyCollectionChanged

    #region IEnumerable

    public IEnumerator<NamedColor> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }

    #endregion IEnumerable
}
