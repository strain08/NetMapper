using NetMapper.Interfaces;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetMapper.Services
{

    public class DriveListService
    {
        public ObservableCollection<MapModel> DriveList { get; set; }

        private readonly IStore<List<MapModel>> store;

        // CTOR
        public DriveListService(IStore<List<MapModel>> storeService)
        {
            store = storeService;
            DriveList = new ObservableCollection<MapModel>(store.GetAll());

        }
        // DTOR
        ~DriveListService()
        {
            if (!store.Update(new List<MapModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void AddDrive(MapModel model)
        {
            DriveList.Add(model);
            if (!store.Update(new List<MapModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void RemoveDrive(MapModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            if (!store.Update(new List<MapModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }        

        public void EditDrive(MapModel oldModel, MapModel newModel)
        {
            var i = DriveList.IndexOf(oldModel);
            DriveList.RemoveAt(i);
            DriveList.Insert(i, newModel);
            if (!store.Update(new List<MapModel>(DriveList))) 
                throw new ApplicationException("Can not write settings file.");
        }
    }


}

