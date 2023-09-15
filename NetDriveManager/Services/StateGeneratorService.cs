using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public class StateGeneratorService
    {
        private readonly IListManager _listManager;
        private readonly StateResolverService _stateResolver;
        private readonly TaskFactory _taskFactory = new();

        //CTOR
        public StateGeneratorService(IListManager listManager, StateResolverService stateResolver)
        {
            _listManager = listManager;
            _stateResolver = stateResolver;

            _taskFactory.StartNew(() => MapAllDrives());

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            // Get share and mapping states into model, at regular intervals

            _taskFactory.StartNew(() => ShareStateLoop(5000));
            _taskFactory.StartNew(() => MappingStateLoop(5000));

        }

        //DTOR
        ~StateGeneratorService()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }

        private async void ShareStateLoop(int timeMilliseconds)
        {

            List<Task> _tasks = new();
            while (true)
            {
                foreach (DriveModel m in _listManager.DriveList)
                {
                    _tasks.Add(Task.Factory.StartNew(() =>
                    {// long op
                        ShareState shState = Directory.Exists(m.NetworkPath)
                            ? ShareState.Available : ShareState.Unavailable;
                        if (shState != m.ShareStateProp)
                        {
                            m.ShareStateProp = shState;
                            _stateResolver.ShareStateChanged(m);
                        }
                    })
                    );
                }
                await Task.WhenAll(_tasks);
                _tasks.Clear();
                Thread.Sleep(timeMilliseconds);
            }

        }



        private void MappingStateLoop(int timeMilliseconds)
        {
            while (true)
            {
                foreach (DriveModel m in _listManager.DriveList)
                {
                    // long op
                    MappingState mpState = Utility.IsNetworkDrive(m.DriveLetter)
                        ? MappingState.Mapped : MappingState.Unmapped;
                    if (mpState != m.MappingStateProp)
                    {
                        m.MappingStateProp = mpState;
                        //MappingStateChange(m);
                        // TODO call an action to state change
                    }
                }
                Thread.Sleep(timeMilliseconds);
            }
        }

        //private void MappingStateChange(MappingModel m)
        //{

        //}

        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                MapAllDrives();
            }
            else
            {
                UnmapAllDrives();
            }
        }

        private void MapAllDrives()
        {
            foreach (DriveModel model in _listManager.DriveList)
            {
                _stateResolver.ConnectDriveToast(model);
            }
        }

        private void UnmapAllDrives()
        {
            foreach (DriveModel model in _listManager.DriveList)
            {
                _stateResolver.DisconnectDriveToast(model);
            }
        }
    }
}
