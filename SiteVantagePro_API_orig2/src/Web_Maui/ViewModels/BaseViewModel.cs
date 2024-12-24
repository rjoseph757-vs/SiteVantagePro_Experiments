using CommunityToolkit.Mvvm.ComponentModel;

namespace Web_Maui.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    public bool _isBusy = false;

    [ObservableProperty]
    public string _title = string.Empty;

}
