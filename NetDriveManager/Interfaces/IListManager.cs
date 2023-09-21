using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Interfaces
{
    public interface IListManager
    {
        public ObservableCollection<MappingModel> DriveList { get; set; }
        public void AddDrive(MappingModel model);
        public void RemoveDrive(MappingModel model);
        public void EditDrive(MappingModel oldModel, MappingModel newModel);
        public bool ContainsDriveLetter(char letter);

    }
}
