using DynamicData;
using NetMapper.Models;
using NetMapper.Services.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NetMapper.Services
{

    public class DriveListService
    {
        public ObservableCollection<MapModel> DriveList { get; set; }
        public Action? ModelPropertiesUpdated;

        private readonly IStore<List<MapModel>> store;

        // CTOR
        public DriveListService(IStore<List<MapModel>> storeService)
        {
            store = storeService;
            DriveList = new ObservableCollection<MapModel>(store.GetAll());
            foreach (var model in DriveList)
            {
                model.PropertyChanged += Model_PropertyChanged;
            }
        }

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(MapModel.MappingStateProp) ||
                e.PropertyName == nameof(MapModel.ShareStateProp))
            {
                ModelPropertiesUpdated?.Invoke();
            }            
        }

        public void AddDrive(MapModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
            DriveList.Add(model);
            store.Update(new List<MapModel>(DriveList));    
        }

        public void RemoveDrive(MapModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            model.PropertyChanged -= Model_PropertyChanged;
            store.Update(new List<MapModel>(DriveList));
                
        }        

        public void EditDrive(MapModel oldModel, MapModel newModel)
        {
            var i = DriveList.IndexOf(oldModel);

            oldModel.PropertyChanged -= Model_PropertyChanged;            
            DriveList.RemoveAt(i);
            
            newModel.PropertyChanged += Model_PropertyChanged;
            DriveList.Insert(i, newModel);
            store.Update(new List<MapModel>(DriveList));
        }        
    }
}

