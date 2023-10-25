using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services;
using Splat;
using System;
using System.Collections.ObjectModel;

namespace NetMapper.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<MapModel>
            DriveList
        { get; set; }

        [ObservableProperty]
        MapModel?
            selectedItem;

        readonly IDriveListService driveListService;
        readonly IDriveConnectService driveConnectService;
        readonly NavService navService;

        // CTOR
        public DriveListViewModel()
        {
            if (Design.IsDesignMode) return; // design mode bypass            

            driveListService = Locator.Current.GetRequiredService<IDriveListService>();
            driveConnectService = Locator.Current.GetRequiredService<IDriveConnectService>();
            navService = Locator.Current.GetRequiredService<NavService>();
            DriveList = driveListService.DriveList;

        }

        // COMMS        
        public void DisconnectDriveCommand(object commandParameter)
        {
            var m = commandParameter as MapModel
                ?? throw new InvalidOperationException("Error getting command parameter for DisconnectDriveCommand");
            m.MappingStateProp = MappingState.Undefined;
            m.Settings.AutoConnect = false; // prevent auto reconnection in Mapping Loop
            driveConnectService.DisconnectDrive(m);
        }

        public void ConnectDriveCommand(object commandParameter)
        {
            var m = commandParameter as MapModel
                ?? throw new InvalidOperationException("Error getting command parameter for ConnectDriveCommand");
            m.MappingStateProp = MappingState.Undefined;
            driveConnectService.ConnectDrive(m);
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
            //VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();

            navService.GoTo(new DriveDetailViewModel());

        }
        public void About()
        {
            navService.GoTo(new AboutViewModel());
        }
        public void Settings()
        {
            navService.GoTo(new SettingsViewModel());
        }

    }
}
