using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetMapper.Models;
using NetMapper.Services.Stores;

namespace NetMapper.Services;

public class DriveListService : IDriveListService
{
    private readonly IDataStore<List<MapModel>> _store;

    // CTOR
    public DriveListService(IDataStore<List<MapModel>> storeService)
    {
        _store = storeService;
        DriveList = new ObservableCollection<MapModel>(_store.GetData());
    }

    public ObservableCollection<MapModel> DriveList { get; set; }

    public void AddDrive(MapModel model)
    {
        DriveList.Add(model);
        _store.Update(new List<MapModel>(DriveList));
    }

    public void RemoveDrive(MapModel model)
    {
        var i = DriveList.IndexOf(model);
        DriveList.RemoveAt(i);
        _store.Update(new List<MapModel>(DriveList));
    }

    public void EditDrive(MapModel oldModel, MapModel newModel)
    {
        var i = DriveList.IndexOf(oldModel);
        DriveList.RemoveAt(i);

        DriveList.Insert(i, newModel);
        _store.Update(new List<MapModel>(DriveList));
    }
}