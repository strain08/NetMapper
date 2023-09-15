using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using NetDriveManager.Services.Helpers;
using Splat;
using System.Collections.Generic;

namespace NetDriveManager.ViewModels
{
    public partial class DriveDetailViewModel : ViewModelBase
    {

        [ObservableProperty]
        DriveModel
            displayItem;

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

        public List<char>
            DriveLettersList
        { get; set; } = new();



        [ObservableProperty]
        string?
            operationTitle;

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

        private bool isEditing;

        private readonly DriveListService driveListService;

        // CTOR
        public DriveDetailViewModel(DriveModel? selectedItem = null)
        {
            driveListService = Locator.Current.GetRequiredService<DriveListService>();
            LoadDriveLetters();

            if (selectedItem == null)
            {
                IsEditing = false;
                DisplayItem = new();
            }
            else
            {
                IsEditing = true;
                // decoupled copy of selected item
                DisplayItem = new DriveModel(selectedItem);

                DriveLettersList.Add(selectedItem.DriveLetter);

                // extract just drive letter from X:
                SelectedLetter = DisplayItem.DriveLetter;
            }
        }

        private void LoadDriveLetters()
        {
            var cLetterList = new List<char>(Utility.GetAvailableDriveLetters());
            foreach (var cLetter in cLetterList)
            {
                if (!driveListService.ContainsDriveLetter(cLetter))
                { 
                    DriveLettersList.Add(cLetter); 
                }
            }
        }

        public void Ok()
        {

            //DisplayItem.DriveLetter = SelectedLetter + ":";
            if (IsEditing)
            {
                driveListService.EditDrive(VMServices.DriveListViewModel!.SelectedItem!, DisplayItem!);
            }
            else
            {
                driveListService.AddDrive(DisplayItem!);
            }
            VMServices.DriveListViewModel.SelectedItem = DisplayItem;
            VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.DriveListViewModel;
        }


        public void Cancel()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.DriveListViewModel;

        }
    }
}
