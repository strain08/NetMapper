using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Services;

namespace NetMapper.ViewModels
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