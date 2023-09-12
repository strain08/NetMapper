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
    public class StateGenerator
    {
        private readonly IListManager _listManager;        
        private readonly TaskFactory _taskFactory = new();

        //CTOR
        public StateGenerator(IListManager listManager)
        {
            _listManager = listManager;

            _taskFactory.StartNew(() => MapAllDrives());

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            // Get share and mapping states into model, at regular intervals

            _taskFactory.StartNew(() => ShareStateLoop(5000));
            _taskFactory.StartNew(() => MappingStateLoop(5000));

        }

        //DTOR
        ~StateGenerator()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }
        
        private void ShareStateLoop(int timeMilliseconds)
        {
            while (true)
            {
                foreach (MappingModel m in _listManager.NetDriveList)
                {
                    ShareState shState = Directory.Exists(m.NetworkPath) ? ShareState.Available : ShareState.Unavailable;
                    if (shState != m.ShareStateProp)
                    {
                        m.ShareStateProp = shState;
                        // TODO call an action to state change
                    }

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
                    MappingState mpState = Utility.IsDriveMapped(m.DriveLetter) ? MappingState.Mapped : MappingState.Unmapped;
                    if (mpState != m.MappingStateProp)
                    {
                        m.MappingStateProp = mpState;
                        // TODO call an action to state change
                    }
                }
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                _taskFactory.StartNew(() => MapAllDrives());               
            }
            else
            {
                _taskFactory.StartNew(() => UnmapAllDrives());                
            }
        }

        private void MapAllDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.MapNetworkDrive(model.DriveLetter[0], model.NetworkPath);
            }
        }

        private void UnmapAllDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.DisconnectNetworkDrive(model.DriveLetter[0], true);
            }
        }
    }
}
