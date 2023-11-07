using NetMapper.Models;
using NetMapper.Services.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMapper.Services;

public class DriveListService : IDriveListService
{
    private readonly IDataStore<List<MapModel>> _store;
    
    public ObservableCollection<MapModel> DriveCollection { get; set; }
    
    // CTOR
    public DriveListService(IDataStore<List<MapModel>> storeService)
    {
        _store = storeService;
        DriveCollection = new ObservableCollection<MapModel>(_store.GetData());
    }

    public void AddDrive(MapModel model)
    {
        DriveCollection.Add(model);
    }

    public void RemoveDrive(MapModel model)
    {
       
        var i = DriveCollection.IndexOf(model);
        if (i == -1)
            throw new KeyNotFoundException($"Element not found in {DriveCollection}");
        
        DriveCollection.RemoveAt(i);
       
    }

    public void EditDrive(MapModel oldModel, MapModel newModel)
    {
        var i = DriveCollection.IndexOf(oldModel);
        if (i == -1)
            throw new KeyNotFoundException($"Element not found in {DriveCollection}");        
        DriveCollection.RemoveAt(i);
        DriveCollection.Insert(i, newModel);
    }

    public void SaveAll()
    {
        _store.Update(new List<MapModel>(DriveCollection));
    }
}