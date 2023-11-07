using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace NetMapper.ViewModels;

public partial class DriveListViewModel : ViewModelBase
{
    private readonly IDriveConnectService driveConnectService;
    private readonly IDriveListService driveListService;
    private readonly INavService nav;

    [ObservableProperty]
    private MapModel? selectedItem;
    public ObservableCollection<MapModel> DriveList { get; set; }
    
    // CTOR
    [ResolveThis]
    public DriveListViewModel(
        IDriveListService driveListService,
        IDriveConnectService driveConnectService,
        INavService navService)
    {
        this.driveListService = driveListService;
        this.driveConnectService = driveConnectService;
        nav = navService;
        DriveList = driveListService.DriveCollection;
    }
#nullable disable
    public DriveListViewModel() { }
#nullable enable

    // COMMANDS        
    public void DisconnectDriveCommand(object commandParameter)
    {
        var m = commandParameter as MapModel
                ?? throw new InvalidOperationException("Error getting command parameter for DisconnectDriveCommand");
        m.Settings.AutoConnect = false; // prevent auto reconnection in Mapping Loop
        driveConnectService.DisconnectDrive(m);
    }

    public void ConnectDriveCommand(object commandParameter)
    {
        var m = commandParameter as MapModel
                ?? throw new InvalidOperationException("Error getting command parameter for ConnectDriveCommand");
        driveConnectService.ConnectDrive(m);
    }

    public void RemoveItem()
    {
        if (SelectedItem == null) return;
        driveListService.RemoveDrive(SelectedItem);
        driveListService.SaveAll();
    }

    public void AddItem()
    {
        nav.GoToNew<DriveDetailViewModel>();
    }

    public void About()
    {
        nav.GoToNew<AboutViewModel>();
    }

    public void Settings()
    {
        nav.GoToNew<SettingsViewModel>();
    }
}