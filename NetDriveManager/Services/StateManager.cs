using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public class StateManager
    {
        private readonly IListManager _listManager;       
        //private readonly ShellCommand? _shellCmd;

        //CTOR
        public StateManager(IListManager listManager)
        {
            _listManager = listManager;

            var mapDrivesTask = new Task(() => MapAllDrives());
            mapDrivesTask.Start();

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
            
            // Get share and mapping states into model, at regular intervals
            var shareStateTask = new Task(() => ShareStateLoop(5000));
            var mappingStateTask = new Task(() => MappingStateLoop(5000));
            shareStateTask.Start();
            mappingStateTask.Start();
        }

        //DTOR
        ~StateManager()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }

       

        private void ShareStateLoop(int timeMilliseconds)
        {
            while (true)
            {
                foreach (MappingModel m in _listManager.NetDriveList)
                {
                    m.ShareStateProp = Directory.Exists(m.NetworkPath) ? ShareState.Available : ShareState.Unavailable;
                }                
                Thread.Sleep(timeMilliseconds);
            }
            
        }

        private void MappingStateLoop(int timeMilliseconds)
        {
            while (true)
            {
                foreach (MappingModel m in _listManager.NetDriveList)
                {                    
                    m.MappingStateProp = Utility.IsDriveMapped(m.DriveLetter) ? MappingState.Mapped : MappingState.Unmapped;
                }                
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                var t = new Task(() => MapAllDrives());
                t.Start();
            }
            else
            {
                var t = new Task(() => UnmapAllDrives());
                t.Start();
            }
        }        

        private void MapAllDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.MapNetworkDrive(model.DriveLetter[0].ToString(), model.NetworkPath);
            }
        }
        private void UnmapAllDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.DisconnectNetworkDrive(model.DriveLetter[0].ToString(), true);
            }
        }








    }
}
