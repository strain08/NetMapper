using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Services;

namespace NetDriveManager.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        ViewModelBase content;

        [ObservableProperty]
        DriveListViewModel driveListViewModel;



        public MainWindowViewModel()
        {            
            VMServices.DriveListViewModel = new DriveListViewModel();
            Content = driveListViewModel = VMServices.DriveListViewModel;            
        }
    }
}