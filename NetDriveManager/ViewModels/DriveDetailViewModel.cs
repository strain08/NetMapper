using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using Splat;
using System.Collections.Generic;


namespace NetMapper.ViewModels
{
    public partial class DriveDetailViewModel : ViewModelBase
    {

        [ObservableProperty]
        MapModel displayItem = new();
        [ObservableProperty]
        MapModel selectedItem = new(); 

        public string NetworkPath
        {
            get => DisplayItem.NetworkPath;
            set => DisplayItem.NetworkPath = value;
        }

        public char SelectedLetter
        {
            get => DisplayItem.DriveLetter;
            set => DisplayItem.DriveLetter = value;
        }

        public List<char> DriveLettersList { get; set; } = new();

        [ObservableProperty]
        string? operationTitle;

        private bool IsEditing
        {
            get => isEditing;
            set
            {
                if (value)
                {
                    OperationTitle = "Edit mapping";
                }
                else
                {
                    OperationTitle = "New mapping";
                }
                isEditing = value;
            }
        }

        bool isEditing;

        readonly IDriveListService driveListService;
        readonly IDriveConnectService driveConnectService;
        readonly NavService nav;

        // CTOR
        public DriveDetailViewModel(MapModel? selectedItem = null)
        {
            if (Design.IsDesignMode) return;
            driveListService = Locator.Current.GetRequiredService<IDriveListService>();
            driveConnectService = Locator.Current.GetRequiredService<IDriveConnectService>();
            nav = Locator.Current.GetRequiredService<NavService>();

            LoadDriveLettersList();

            if (selectedItem == null)
            {
                IsEditing = false;
                return;
            }

            IsEditing = true;

            SelectedItem = selectedItem;

            // decoupled copy of selected item
            DisplayItem = selectedItem.Clone();

            // add selected item letter
            DriveLettersList.Add(selectedItem.DriveLetter);
            DriveLettersList.Sort();

            SelectedLetter = DisplayItem.DriveLetter;

        }

        private void LoadDriveLettersList()
        {
            var cAvailableLeters = new List<char>(Interop.GetAvailableDriveLetters());

            // remove unmapped managed drive letters
            foreach (MapModel d in driveListService.DriveList)
                cAvailableLeters.Remove(d.DriveLetter);

            foreach (char cLetter in cAvailableLeters)
                DriveLettersList.Add(cLetter);
        }

        public void Ok()
        {
            if (IsEditing)
            {                
                if (SelectedItem.DriveLetter != DisplayItem.DriveLetter) // drive letter changed
                {
                    driveConnectService.DisconnectDrive(SelectedItem); // disconnect previous drive
                }

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
}
