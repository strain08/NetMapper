using NetDriveManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Interfaces
{
    public interface IListManager
    {
        public ObservableCollection<DriveModel> DriveList { get; set; }
        public void AddDrive(DriveModel model);
        public void RemoveDrive(DriveModel model);
        public void EditDrive(DriveModel oldModel, DriveModel newModel);
        public bool ContainsDriveLetter(char letter);

    }
}
