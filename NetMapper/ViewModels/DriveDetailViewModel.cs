using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System.Collections.Generic;

namespace NetMapper.ViewModels;

public partial class DriveDetailViewModel : ViewModelBase
{
    private readonly IConnectService driveConnectService;
    private readonly IDriveListService driveListService;
    private readonly INavService nav;    

    [ObservableProperty]    
    bool isEditing = false;

    partial void OnIsEditingChanged(bool value)
    {
        if (value) OperationTitle = "Edit mapping";
        else OperationTitle = "New mapping";
    }

    public List<char> DriveLettersList { get; set; } = new();
    
    [ObservableProperty]
    char driveLetter;
    partial void OnDriveLetterChanged(char value) => ValidateAll();

    [ObservableProperty]
    string networkPath = "";
    partial void OnNetworkPathChanged(string value) => ValidateAll();

    [ObservableProperty]
    bool autoConnect;

    [ObservableProperty]
    bool autoDisconnect;

    [ObservableProperty]
    private string? operationTitle;    

    [ObservableProperty]
    private MapModel selectedItem = new();

    [ObservableProperty]
    bool isUserInputValid = false;

#nullable disable
    public DriveDetailViewModel() { }
#nullable enable

    [ResolveThis]
    public DriveDetailViewModel(
        IDriveListService driveListService,
        IConnectService driveConnectService,
        INavService nav)
    {
        this.driveListService = driveListService;
        this.driveConnectService = driveConnectService;
        this.nav = nav;

        LoadDriveLettersList();
        OnIsEditingChanged(false);
        ValidateAll();
    }

    public DriveDetailViewModel EditItem(MapModel selectedItem)
    {
        IsEditing = true;

        SelectedItem = selectedItem;

        DriveLettersList.Add(SelectedItem.DriveLetter);
        DriveLettersList.Sort();

        NetworkPath = SelectedItem.NetworkPath;
        DriveLetter = SelectedItem.DriveLetter;
        AutoConnect = SelectedItem.Settings.AutoConnect;
        AutoDisconnect = SelectedItem.Settings.AutoDisconnect;

        //ValidateAll();
        return this;
    }

    public void Ok()
    {
        var editedItem = new MapModel
        {
            DriveLetter = DriveLetter,
            NetworkPath = NetworkPath,
            Settings = new()
            {
                AutoConnect = AutoConnect,
                AutoDisconnect = AutoDisconnect
            }
        };
        
        if (IsEditing)
        {
            // disconnect previous letter if changed
            if (SelectedItem.DriveLetter != editedItem.DriveLetter) 
                driveConnectService.Disconnect(SelectedItem); 
            
            driveListService.EditDrive(SelectedItem, editedItem);
        }
        else
        {            
            driveListService.AddDrive(editedItem);
        }

        driveListService.SaveAll();
        
        nav.GoTo<DriveListViewModel>();
    }

    public void Cancel()
    {
        nav.GoTo<DriveListViewModel>();
    }

    [DependsOn(nameof(IsUserInputValid))]
    bool CanOk(object param)
    {
        return IsUserInputValid;
    }

    private bool ValidateAll()
    {
        var result = true;
        result &= ValidateNetworkPath(nameof(NetworkPath), NetworkPath.Trim());
        result &= ValidateDriveLetter(nameof(DriveLetter), DriveLetter);
        IsUserInputValid = result;
        return result;
    }

    private void LoadDriveLettersList()
    {
        var cAvailableLeters = new List<char>(Interop.GetAvailableDriveLetters());

        // remove unmapped managed drive letters
        foreach (var d in driveListService.DriveCollection)
            cAvailableLeters.Remove(d.DriveLetter);

        foreach (var cLetter in cAvailableLeters)
            DriveLettersList.Add(cLetter);
    }
}