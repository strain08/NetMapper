using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;

namespace NetDriveManager.Services
{

    public class DriveListManager : IListManager
    {

        public ObservableCollection<MappingModel> NetDriveList { get; set; } = new();

        private readonly IStorage _store;

        // CTOR
        public DriveListManager(IStorage store)
        {
            _store = store;
            NetDriveList = new ObservableCollection<MappingModel>(_store.GetAll());
        }

        public void AddDrive(MappingModel model)
        {
            NetDriveList.Add(model);

            _store.Update(new List<MappingModel>(NetDriveList));
        }

        public void RemoveDrive(MappingModel model)
        {
            var i = NetDriveList.IndexOf(model);
            NetDriveList.RemoveAt(i);

            _store.Update(new List<MappingModel>(NetDriveList));
        }


        public void EditDrive(MappingModel oldModel, MappingModel newModel)
        {
            var i = NetDriveList.IndexOf(oldModel);
            NetDriveList.RemoveAt(i);
            NetDriveList.Insert(i, newModel);

            _store.Update(new List<MappingModel>(NetDriveList));
        }

       
    }


}

