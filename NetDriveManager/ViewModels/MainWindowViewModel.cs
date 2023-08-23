using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using NetDriveManager.Services.Helpers;
using System.Diagnostics;
using System.Threading;

namespace NetDriveManager.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        ViewModelBase content;

        [ObservableProperty]
        DriveListViewModel list;

        public MainWindowViewModel()
        {
            VMServices.DriveListViewModel = new DriveListViewModel();
            Content = List = VMServices.DriveListViewModel;
            //Thread t = new Thread(() => { NetworkDriveManager.IsMachineOnline("ddd"); });
            //t.Start();
            //string s = string.Empty;
        }

        

    }
}