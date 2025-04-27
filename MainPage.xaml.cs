namespace CollectionViewIssue
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new Model.MainViewModel();
            InitializeComponent();
        }
    }
}
