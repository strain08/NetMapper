using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NetDriveManager.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<MappingModel>
            NetDrivesList { get; set; }

        [ObservableProperty]
        MappingModel?
            selectedItem;

        private readonly DriveListManager _listManager;
        //private readonly NetMonitor _netMonitor;

        // CTOR
        public DriveListViewModel()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return;

            _listManager = Locator.Current.GetService<DriveListManager>() ?? throw new KeyNotFoundException("Error getting service " + typeof(DriveListManager));
            NetDrivesList = _listManager.NetDriveList;
        }


        // COMMS        
        public void Dcv(object obj)
        {
            var model = (MappingModel)obj;
            Debug.WriteLine("Viwe model:" + model.DriveLetter);
        }

        public void RemoveItem()
        {
            if (SelectedItem != null)
            {
                _listManager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();
        }



    }
}
