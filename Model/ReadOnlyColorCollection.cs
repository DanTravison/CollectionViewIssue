namespace CollectionViewIssue.Model;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

/// <summary>
/// Provides a read-only <see cref="NamedColor"/> collection based on <see cref="ReadOnlyCollection{T}"/>
/// </summary>
internal class ReadOnlyColorCollection 
    : ReadOnlyCollection<NamedColor>, 
      INotifyPropertyChanged, 
      INotifyCollectionChanged,
      ISelectedItem<NamedColor>
{
    readonly OrderedList<NamedColor> _items;
    NamedColor _selectedItem;

    public ReadOnlyColorCollection(IEnumerable<NamedColor> colors) 
        : base(new OrderedList<NamedColor>(NamedColor.Comparer, colors))
    {
        _items = (OrderedList<NamedColor>)Items;
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

    #region Updates

    internal void Add(NamedColor item)
    {
        int index = _items.AddItem(item);
        if (index >= 0)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }
    }

    internal void Remove(NamedColor color)
    {
        int index = _items.RemoveItem(color);
        if (index >= 0)
        {
            OnPropertyChanged(CountPropertyChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, color, index));
        }
    }

    internal void Clear()
    {
        if (_items.Count > 0)
        {
            List<NamedColor> removedItems = new List<NamedColor>(_items);
            _items.Clear();
            OnPropertyChanged(CountPropertyChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, 0));
            OnCollectionChanged(ResetNotifyCollectionChangedEventArgs);
        }
    }

    #endregion Updates

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
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
}
