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
        public ObservableCollection<MappingModel> NetDriveList { get; set; }
        public void AddDrive(MappingModel model);
        public void RemoveDrive(MappingModel model);
        public void EditDrive(MappingModel oldModel, MappingModel newModel);
        

    }
}
