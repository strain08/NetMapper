using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using NetMapper.Services.Static;
using Splat;
using System.Collections.Generic;

namespace NetMapper.ViewModels
{
    public partial class DriveDetailViewModel : ViewModelBase
    {

        [ObservableProperty]
        MappingModel displayItem;
        [ObservableProperty]
        MappingModel selectedItem;

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

        public List<char> DriveLettersList
        { get; set; } = new();

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

        DriveListService driveListService;
        DriveConnectService driveConnectService;

        // CTOR
        public DriveDetailViewModel(MappingModel? selectedItem = null)
        {
            driveListService = Locator.Current.GetRequiredService<DriveListService>();
            driveConnectService = Locator.Current.GetRequiredService<DriveConnectService>();

            LoadDriveLettersList();

            if (selectedItem == null)
            {
                IsEditing = false;
                DisplayItem = new();
            }
            else
            {
                IsEditing = true;

                SelectedItem = selectedItem;

                // decoupled copy of selected item
                DisplayItem = new MappingModel(selectedItem);

                // add selected item letter
                DriveLettersList.Add(selectedItem.DriveLetter);

                SelectedLetter = DisplayItem.DriveLetter;
            }
        }

        private void LoadDriveLettersList()
        {
            var cAvailableLeters = new List<char>(Utility.GetAvailableDriveLetters());

            // remove unmapped managed drive letters
            foreach (MappingModel d in driveListService.DriveList)
                cAvailableLeters.Remove(d.DriveLetter);

            foreach (char cLetter in cAvailableLeters)
                DriveLettersList.Add(cLetter);
        }

        public void Ok()
        {
            if (IsEditing)
            {
                // drive letter changed
                if (SelectedItem.DriveLetter != DisplayItem.DriveLetter)
                {
                    driveConnectService.DisconnectDrive(SelectedItem); // disconnect previous drive
                }

                driveListService.EditDrive(SelectedItem, DisplayItem);
            }
            else
            {
                driveListService.AddDrive(DisplayItem);
            }
            VMServices.DriveListViewModel.SelectedItem = DisplayItem;

            VMServices.MainWindowViewModel!.Content = VMServices.DriveListViewModel;
        }


        public void Cancel()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.DriveListViewModel;


        }
    }
}
