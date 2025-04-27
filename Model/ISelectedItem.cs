namespace CollectionViewIssue.Model;

using System.ComponentModel;

internal interface ISelectedItem<T> : INotifyPropertyChanged
{
    T SelectedItem { get; set; }
}
