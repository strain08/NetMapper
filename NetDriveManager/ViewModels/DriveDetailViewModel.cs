using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using NetDriveManager.Services.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NetDriveManager.ViewModels
{
    public partial class DriveDetailViewModel : ViewModelBase
    {

        [ObservableProperty]
        NDModel
            displayItem;

        public List<string> 
            DriveLettersList { get; set; } = new();

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

        public DriveDetailViewModel(bool isEditing)
        {
            var dl = new List<char>(Utility.GetAvailableDriveLetters());
            foreach (var item in dl)
            {
                DriveLettersList.Add(item.ToString());
            }
            
            IsEditing = isEditing;
            if (isEditing)
            {
                DisplayItem = new NDModel(VMServices.DriveListViewModel!.SelectedItem!);
                SelectedLetter = DisplayItem.DriveLetter.Substring(0,1);
            }
            else
            {
                DisplayItem = new();
            }
        }

        public void Ok()
        {
            Debug.WriteLine(DisplayItem.Hostname);
            DisplayItem.DriveLetter = selectedLetter + ":";
            if (IsEditing)
            {
                
                NDManager.EditDrive(VMServices.DriveListViewModel!.SelectedItem!, DisplayItem!);
                VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.List;
            }
            else
            {
                NDManager.AddDrive(DisplayItem!);
                VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.List;
            }
        }

        public void Cancel()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.MainWindowViewModel.List;

        }
    }
}
