using NetDriveManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Interfaces
{
    public interface IStorage
    {
        public List<NDModel> GetAll();
        public bool Load();
        public bool Save();
        public bool Update(List<NDModel> updatedList);
    }
    
    public interface INDManager
    {
        public ObservableCollection<NDModel> NetDriveList { get; set; }
        public void AddDrive(NDModel model);
        public void RemoveDrive(NDModel model);
        public void EditDrive(NDModel oldModel, NDModel newModel);
        public void Clear();

    }
}
