using NetDriveManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Interfaces
{
    public interface IStorage
    {
        public List<DriveModel> GetAll();
        public bool Load();
        public bool Save();
        public bool Update(List<DriveModel> updatedList);
        
    }
}
