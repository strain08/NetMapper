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
        MappingModel
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

        DriveListService driveListService;
        StateResolverService stateResolverService;

        // CTOR
        public DriveDetailViewModel(MappingModel? selectedItem = null)
        {
            driveListService = Locator.Current.GetRequiredService<DriveListService>();
            stateResolverService = Locator.Current.GetRequiredService<StateResolverService>();

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

                // add selected item letter
                DriveLettersList.Add(selectedItem.DriveLetter);

                // extract just drive letter from X:
                SelectedLetter = DisplayItem.DriveLetter;
            }
        }

        private void LoadDriveLetters()
        {
            var cLetterList = new List<char>(Utility.GetAvailableDriveLetters());
            foreach (char cLetter in cLetterList)
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
                // drive letter changed
                if (VMServices.DriveListViewModel!.SelectedItem!.DriveLetter != DisplayItem.DriveLetter)
                {
                    stateResolverService.DisconnectDriveToast(VMServices.DriveListViewModel!.SelectedItem!);
                }                

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
