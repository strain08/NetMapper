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
        MappingModel
            displayItem;

        public List<string>
            DriveLettersList
        { get; set; } = new();

        [ObservableProperty]
        string?
            selectedLetter;

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

        private readonly DriveListManager? _ndmanager;

        // CTOR
        public DriveDetailViewModel(MappingModel? selectedItem = null)
        {
            _ndmanager = Locator.Current.GetService<DriveListManager>() ?? throw new KeyNotFoundException();
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
                DisplayItem = new MappingModel(selectedItem);

                DriveLettersList.Add(selectedItem.DriveLetter[0].ToString());
                // extract just drive letter from X:
                SelectedLetter = DisplayItem.DriveLetter[0].ToString();
            }
        }

        private void LoadDriveLetters()
        {
            var dl = new List<char>(Utility.GetAvailableDriveLetters());
            foreach (var item in dl)
            {
                DriveLettersList.Add(item.ToString());
            }
        }
        public void Ok()
        {

            DisplayItem.DriveLetter = SelectedLetter + ":";
            if (IsEditing)
            {
                _ndmanager.EditDrive(VMServices.DriveListViewModel!.SelectedItem!, DisplayItem!);
            }
            else
            {
                _ndmanager.AddDrive(DisplayItem!);
            }
            VMServices.DriveListViewModel.SelectedItem = DisplayItem;
            VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.List;
        }

        public void Cancel()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.List;

        }
    }
}
