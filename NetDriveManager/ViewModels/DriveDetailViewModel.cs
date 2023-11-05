using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using NetMapper.Services.Interfaces;

namespace NetMapper.ViewModels;

public partial class DriveDetailViewModel : ViewModelBase
{
    private readonly IDriveConnectService driveConnectService;

    private readonly IDriveListService driveListService;
    private readonly INavService nav;

    [ObservableProperty] private MapModel displayItem = new();

    private bool isEditing;

    [ObservableProperty] private string? operationTitle;

    [ObservableProperty] private MapModel selectedItem = new();

    [ResolveThis]
    public DriveDetailViewModel(IDriveListService driveListService,
        IDriveConnectService driveConnectService,
        INavService nav)
    {
        this.driveListService = driveListService;
        this.driveConnectService = driveConnectService;
        this.nav = nav;

        LoadDriveLettersList();
        IsEditing = false;
    }

    public List<char> DriveLettersList { get; set; } = new();

    private bool IsEditing
    {
        get => isEditing;
        set
        {
            if (value)
                OperationTitle = "Edit mapping";
            else
                OperationTitle = "New mapping";
            isEditing = value;
        }
    }

    public DriveDetailViewModel EditItem(MapModel selectedItem)
    {
        IsEditing = true;

        SelectedItem = selectedItem;

        DriveLettersList.Add(SelectedItem.DriveLetter);
        DriveLettersList.Sort();

        DisplayItem = SelectedItem.Clone(); // decoupled copy of selected item

        return this;
    }


    private void LoadDriveLettersList()
    {
        var cAvailableLeters = new List<char>(Interop.GetAvailableDriveLetters());

        // remove unmapped managed drive letters
        foreach (var d in driveListService.DriveList)
            cAvailableLeters.Remove(d.DriveLetter);

        foreach (var cLetter in cAvailableLeters)
            DriveLettersList.Add(cLetter);
    }

    public void Ok()
    {
        if (IsEditing)
        {
            if (SelectedItem.DriveLetter != DisplayItem.DriveLetter) // drive letter changed
                driveConnectService.DisconnectDrive(SelectedItem); // disconnect previous drive

            driveListService.EditDrive(SelectedItem, DisplayItem);
        }
        else
        {
            driveListService.AddDrive(DisplayItem);
        }

        nav.GetViewModel<DriveListViewModel>().SelectedItem = DisplayItem;
        nav.GoTo<DriveListViewModel>();
    }

    public void Cancel()
    {
        nav.GoTo<DriveListViewModel>();
    }
}