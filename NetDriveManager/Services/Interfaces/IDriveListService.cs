using System.Collections.ObjectModel;
using NetMapper.Models;

namespace NetMapper.Services;

public interface IDriveListService
{
    ObservableCollection<MapModel> DriveList { get; set; }

    void AddDrive(MapModel model);
    void EditDrive(MapModel oldModel, MapModel newModel);
    void RemoveDrive(MapModel model);
}