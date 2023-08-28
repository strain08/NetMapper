using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;


namespace NetDriveManager.Services
{

    public class NDManager : INDManager
    {

        public ObservableCollection<NDModel> NetDriveList { get; set; } = new();

        private readonly IStorage _store;

        public NDManager(IStorage store)
        {
            _store = store;
            NetDriveList = new ObservableCollection<NDModel>(_store.GetAll());
        }

        public void AddDrive(NDModel model)
        {
            NetDriveList.Add(model);

            _store.Update(new List<NDModel>(NetDriveList));
        }

        public void RemoveDrive(NDModel model)
        {
            var i = NetDriveList.IndexOf(model);
            NetDriveList.RemoveAt(i);

            _store.Update(new List<NDModel>(NetDriveList));
        }

        public void Clear()
        {
            NetDriveList.Clear();
        }

        public void EditDrive(NDModel oldModel, NDModel newModel)
        {
            var i = NetDriveList.IndexOf(oldModel);
            NetDriveList.RemoveAt(i);
            NetDriveList.Insert(i, newModel);

            _store.Update(new List<NDModel>(NetDriveList));
        }

    }


}

