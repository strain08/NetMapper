using NetMapper.Models;
using System;
using System.Collections.ObjectModel;

namespace NetMapper.Services
{
    public interface IDriveListService
    {
        ObservableCollection<MapModel> DriveList { get; set; }        

        void AddDrive(MapModel model);
        void EditDrive(MapModel oldModel, MapModel newModel);
        void RemoveDrive(MapModel model);
    }
}