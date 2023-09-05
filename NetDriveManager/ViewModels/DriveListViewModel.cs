using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NetDriveManager.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<MappingModel>
            NetDrivesList
        { get; set; }

        [ObservableProperty]
        MappingModel?
            selectedItem;

        private readonly DriveListManager _ndmanager;
        //private readonly NetMonitor _netMonitor;

        // CTOR
        public DriveListViewModel()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return;

            _ndmanager = Locator.Current.GetService<DriveListManager>() ?? throw new KeyNotFoundException("Error getting service " + typeof(DriveListManager));
            NetDrivesList = _ndmanager.NetDriveList;
        }

        // COMMS
        public void DisconnectCommand()
        {
            Debug.WriteLine("Viwe model:" + SelectedItem.DriveLetter);
        }

        public void RemoveItem()
        {
            if (SelectedItem != null)
            {
                _ndmanager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();

        }



    }
}
