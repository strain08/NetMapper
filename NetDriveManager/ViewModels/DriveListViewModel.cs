using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Static;
using Splat;
using System;
using System.Collections.ObjectModel;

namespace NetMapper.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<MapModel>?
            DriveList
        { get; set; }

        [ObservableProperty]
        MapModel?
            selectedItem;

        readonly DriveListService driveListService;
        readonly DriveConnectService stateResolverService;



        // CTOR
        public DriveListViewModel()
        {

            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            
            
            driveListService = Locator.Current.GetRequiredService<DriveListService>();
            stateResolverService = Locator.Current.GetRequiredService<DriveConnectService>();

            DriveList = driveListService.DriveList;
        }

        // COMMS        
        public void DisconnectDriveCommand(object commandParameter)
        {
            var m = (MapModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for DisconnectDriveCommand");
            m.MappingStateProp = MappingState.Undefined;
            m.Settings.AutoConnect = false; // prevent auto reconnection in Mapping Loop
            stateResolverService.DisconnectDrive(m);
        }

        public void ConnectDriveCommand(object commandParameter)
        {
            var m = (MapModel)commandParameter
                ?? throw new InvalidOperationException("Error getting command parameter for ConnectDriveCommand");
            m.MappingStateProp = MappingState.Undefined;
            stateResolverService.ConnectDrive(m);
        }

        public void RemoveItem()
        {
            if (SelectedItem != null && driveListService != null)
            {
                driveListService.RemoveDrive(SelectedItem);
            }
        }

        public static void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();
        }
        public static void Info()
        {

        }
        public static void Settings()
        {
            VMServices.MainWindowViewModel!.Content = new SettingsViewModel();
        }
        
    }
}
