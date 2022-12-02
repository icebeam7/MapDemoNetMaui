using CommunityToolkit.Mvvm.ComponentModel;

namespace MapDemo.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title;
    }
}
