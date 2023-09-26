using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Services;

namespace NetMapper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        ViewModelBase content; 

        public MainWindowViewModel()
        {            
            Content = VMServices.DriveListViewModel = new DriveListViewModel();
        }
    }
}