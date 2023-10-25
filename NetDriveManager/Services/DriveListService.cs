using NetMapper.Models;
using NetMapper.Services.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMapper.Services
{

    public class DriveListService : IDriveListService
    {
        public ObservableCollection<MapModel> DriveList { get; set; }

        public Action? ModelPropertiesUpdated
        {
            get => propertiesUpdated; 
            set => propertiesUpdated = value;
        }

        private Action? propertiesUpdated;

        private readonly IDataStore<List<MapModel>> _store;

        // CTOR
        public DriveListService(IDataStore<List<MapModel>> storeService)
        {
            _store = storeService;
            DriveList = new ObservableCollection<MapModel>(_store.GetData());

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
                propertiesUpdated?.Invoke();
            }
        }

        public void AddDrive(MapModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
            DriveList.Add(model);
            _store.Update(new List<MapModel>(DriveList));
        }

        public void RemoveDrive(MapModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            model.PropertyChanged -= Model_PropertyChanged;
            _store.Update(new List<MapModel>(DriveList));

        }

        public void EditDrive(MapModel oldModel, MapModel newModel)
        {
            var i = DriveList.IndexOf(oldModel);

            oldModel.PropertyChanged -= Model_PropertyChanged;
            DriveList.RemoveAt(i);

            newModel.PropertyChanged += Model_PropertyChanged;
            DriveList.Insert(i, newModel);
            _store.Update(new List<MapModel>(DriveList));
        }
    }
}

