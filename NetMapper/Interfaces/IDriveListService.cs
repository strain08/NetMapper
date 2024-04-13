using NetMapper.Models;
using System.Collections.ObjectModel;

namespace NetMapper.Interfaces;

public interface IDriveListService
{
    ObservableCollection<MapModel> DriveCollection { get; set; }

    void AddDrive(MapModel model);
    void EditDrive(MapModel oldModel, MapModel newModel);
    void RemoveDrive(MapModel model);
    void SaveAll();
}