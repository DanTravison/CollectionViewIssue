namespace CollectionViewIssue.Model;

internal class MainViewModel
{
    public MainViewModel()
    {
        Source = new ReadOnlyColorCollection(NamedColor.All);
        Source.PropertyChanged += OnSourcePropertyChanged;
        MoveToCommand = new(MoveToDestination);

        Destination = new CustomReadOnlyColorCollection();
        Destination.PropertyChanged += OnDestinationPropertyChanged;
        MoveFromCommand = new(MoveToSource);
    }

    #region Event Handlers

    private void OnDestinationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ISelectedItem<NamedColor>.SelectedItem))
        {
            UpdateCommand(MoveFromCommand, Destination, Source);
        }
    }

    private void OnSourcePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ISelectedItem<NamedColor>.SelectedItem))
        {
            UpdateCommand(MoveToCommand, Source, Destination);
        }
    }

    void UpdateCommand(Command command, ISelectedItem<NamedColor> source, ISelectedItem<NamedColor> destination)
    {
        if (source.SelectedItem != null)
        {
            destination.SelectedItem = null;
            command.IsEnabled = true;
        }
        else
        {
            command.IsEnabled = false;
        }
    }

    #endregion Event Handlers

    #region Commands

    public ReadOnlyColorCollection Source
    {
        get;
    }

    public CustomReadOnlyColorCollection Destination
    {
        get;
    }

    public Model.Command MoveToCommand
    {
        get;
    }

    public Command MoveFromCommand
    {
        get;
    }

    void MoveToSource()
    {
        if (Destination.SelectedItem != null)
        {
            NamedColor namedColor = Destination.SelectedItem;
            Destination.Remove(namedColor);
            Destination.SelectedItem = null;
            Source.Add(namedColor);
        }
    }   

    void MoveToDestination()
    {
        if (Source.SelectedItem != null)
        {
            NamedColor namedColor = Source.SelectedItem;
            Source.Remove(namedColor);
            Source.SelectedItem = null;
            Destination.Add(namedColor);
        }
    }

    #endregion Commands
}
