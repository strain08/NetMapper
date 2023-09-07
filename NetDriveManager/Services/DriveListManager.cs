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

        public DriveListManager(IStorage store)
        {
            _store = store;
            NetDriveList = new ObservableCollection<MappingModel>(_store.GetAll());
            foreach (MappingModel model in NetDriveList) 
            {
                // model delegate is bound to list buttons
                model.OnConnectCommand = ConnectDriveCommand;
                model.OnDisconnectCommand = DisconnectDriveCommand;                
            }

        }

        public void AddDrive(MappingModel model)
        {
            model.OnDisconnectCommand+= DisconnectDriveCommand;
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

        private void DisconnectDriveCommand(MappingModel model)
        {
            Utility.DisconnectNetworkDrive(model.DriveLetter[0].ToString(), true);
        }

        private void ConnectDriveCommand(MappingModel model)
        {
            Utility.MapNetworkDrive(model.DriveLetter[0].ToString(),model.NetworkPath );
        }
    }


}

