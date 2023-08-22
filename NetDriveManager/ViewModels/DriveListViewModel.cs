using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Models;
using NetDriveManager.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetDriveManager.ViewModels
{
    public partial class DriveListViewModel : ViewModelBase
    {
        // PROP
        public ObservableCollection<NDModel> 
            NetDrivesList { get; set; }
        
        [ObservableProperty]
        NDModel? 
            selectedItem;
        
        
        
        // CTOR
        public DriveListViewModel(Database db)
        {            
            NetDrivesList = NDManager.NetDriveList = new ObservableCollection<NDModel>(db.GetDrives);
        }
       
        // COMMS

        public void RemoveItem()
        {
            if (SelectedItem != null)
            {
                NDManager.RemoveDrive(SelectedItem);
            }
        }

        public void AddItem()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.DriveDetailViewModel = new DriveDetailViewModel(false);
            //VMServices.DriveDetailViewModel.DisplayItem = new();
        }
        
       
        
    }
}
