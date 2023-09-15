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
        public ObservableCollection<DriveModel>?
            DriveList
        { get; set; }

        [ObservableProperty]
        DriveModel?
            selectedItem;

        DriveListService driveListService;

        StateResolverService stateResolverService;

        // CTOR
        public DriveListViewModel()
        {

            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            
           
            driveListService = Locator.Current.GetRequiredService<DriveListService>();
            stateResolverService = Locator.Current.GetRequiredService<StateResolverService>();

            DriveList = driveListService.DriveList;
        }

        // COMMS        
        public void DisconnectDriveCommand(object commandParameter)
        {
            var model = (DriveModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for DisconnectDriveCommand");
            model.MappingStateProp = Enums.MappingState.Undefined;
            stateResolverService.DisconnectDriveToast(model);
        }

        public void ConnectDriveCommand(object commandParameter)
        {
            var model = (DriveModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for ConnectDriveCommand");
            model.MappingStateProp = Enums.MappingState.Undefined;
            stateResolverService.ConnectDriveToast(model);
        }

        public void RemoveItem()
        {
            if (SelectedItem != null && driveListService != null)
            {
                driveListService.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();
        }
    }
}
