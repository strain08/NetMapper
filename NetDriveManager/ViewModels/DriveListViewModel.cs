using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using Splat;
using System;
using System.Collections.ObjectModel;

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

        private readonly DriveListManager 
            listManager;
        
        private readonly StateResolver 
            stateResolver = new();

        // CTOR
        public DriveListViewModel()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            
           
            listManager = Locator.Current.GetRequiredService<DriveListManager>();
            NetDrivesList = listManager.NetDriveList;
        }

        // COMMS        
        public void DisconnectDriveCommand(object commandParameter)
        {
            var model = (MappingModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for DisconnectDriveCommand");
            stateResolver.DisconnectDrive(model);
        }

        public void ConnectDriveCommand(object commandParameter)
        {
            var model = (MappingModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for ConnectDriveCommand");
            stateResolver.ConnectDrive(model);
        }

        public void RemoveItem()
        {
            if (SelectedItem != null && listManager != null)
            {
                listManager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();
        }
    }
}
