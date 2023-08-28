using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetDriveManager.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<NDModel>
            NetDrivesList
        { get; set; }

        [ObservableProperty]
        NDModel?
            selectedItem;

        private readonly NDManager _ndmanager;

        // CTOR
        public DriveListViewModel()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return;

            _ndmanager = Locator.Current.GetService<NDManager>() ?? throw new KeyNotFoundException("Error getting service " + typeof(NDManager));

            NetDrivesList = _ndmanager.NetDriveList;
        }

        // COMMS

        public void RemoveItem()
        {
            if (SelectedItem != null)
            {
                _ndmanager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel();

        }



    }
}
