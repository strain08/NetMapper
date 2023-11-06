using NetMapper.Models;
using NetMapper.Services.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMapper.Services;

public class DriveListService : IDriveListService
{
    private readonly IDataStore<List<MapModel>> _store;
    
    public ObservableCollection<MapModel> DriveList { get; set; }
    
    // CTOR
    public DriveListService(IDataStore<List<MapModel>> storeService)
    {
        _store = storeService;
        DriveList = new ObservableCollection<MapModel>(_store.GetData());
    }

    public void AddDrive(MapModel model)
    {
        DriveList.Add(model);
    }

    public void RemoveDrive(MapModel model)
    {
        var i = DriveList.IndexOf(model);
        DriveList.RemoveAt(i);
    }

    public void EditDrive(MapModel oldModel, MapModel newModel)
    {
        var i = DriveList.IndexOf(oldModel);
        DriveList.RemoveAt(i);

        DriveList.Insert(i, newModel);
    }

    public void SaveAll()
    {
        _store.Update(new List<MapModel>(DriveList));
    }
}