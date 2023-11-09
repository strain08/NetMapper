using NetMapper.Interfaces;
using NetMapper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMapper.Services;

public class DriveListService : IDriveListService
{
    private readonly IDataStore<AppDataModel> _store;
    
    public ObservableCollection<MapModel> DriveCollection { get; set; }
    
    // CTOR
    public DriveListService(IDataStore<AppDataModel> storeService)
    {
        _store = storeService;
        DriveCollection = new ObservableCollection<MapModel>(_store.GetData().Models);
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
        var data = new AppDataModel();
        data.Models = new List<MapModel>(DriveCollection);
        _store.Update(data);
    }
}