using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace NetDriveManager.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<MappingModel>?
            NetDrivesList
        { get; set; }

        [ObservableProperty]
        MappingModel?
            selectedItem;

        private readonly DriveListManager? _listManager;

        // CTOR
        public DriveListViewModel()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass

            _listManager = Locator.Current.GetService<DriveListManager>()
                ?? throw new KeyNotFoundException("Error getting service " + typeof(DriveListManager));
            NetDrivesList = _listManager.NetDriveList;
            
        }


        // COMMS        
        public void DisconnectDriveCommand(object commandParameter)
        {
            var model = (MappingModel)commandParameter
                ?? throw new System.Exception("Error getting command parameter for DisconnectDriveCommand");
            // TODO 
            // 1. Error-check command
            // 2. Handle error trough UI
            // 3. If ok or error handled correctly Update status
            
        }

        public void ConnectDriveCommand(object commandParameter)
        {
            var model = (MappingModel)commandParameter
                ?? throw new System.Exception("Error getting command parameter for ConnectDriveCommand");
            // TODO 
            // 1. Error-check command
            // 2. Handle error trough UI
            // 3. If ok or error handled correctly Update status

        }

        public void RemoveItem()
        {
            if (SelectedItem != null && _listManager != null)
            {
                _listManager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();
        }



    }
}
