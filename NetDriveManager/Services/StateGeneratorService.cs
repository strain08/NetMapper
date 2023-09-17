using DynamicData.Experimental;
using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
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

            _taskFactory.StartNew(() => stateResolver.TryMapAllDrives());

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

            List<Task> shareCheckTasks = new();
            while (true)
            {
                foreach (MappingModel m in _listManager.DriveList)                
                    shareCheckTasks.Add(Task.Factory.StartNew(() => CheckShareState(m)));                
                await Task.WhenAll(shareCheckTasks);
                shareCheckTasks.Clear();
                Thread.Sleep(timeMilliseconds);
            }

        }

        private void CheckShareState(MappingModel m)
        {
            // check share state
            ShareState shState = Directory.Exists(m.NetworkPath) ? ShareState.Available : ShareState.Unavailable;
            // if share state changed
            if (shState != m.ShareStateProp)
            {
                m.ShareStateProp = shState; // UI notify
                ShareStateChanged(m); // handle new state
            }
        }

        private void ShareStateChanged(MappingModel m)
        {
            if (m.ShareStateProp == ShareState.Available &&
                m.MappingStateProp == MappingState.Unmapped &&
                m.MappingSettings.AutoConnect)
            {
                _stateResolver.ConnectDriveToast(m);
                return;
            }
            if (m.ShareStateProp == ShareState.Unavailable &&
                m.MappingStateProp == MappingState.Mapped &&
                m.MappingSettings.AutoDisconnect)
            {
                _stateResolver.DisconnectDriveToast(m);
            }
        }

        private void MappingStateLoop(int timeMilliseconds)
        {
            while (true)
            {
                foreach (MappingModel m in _listManager.DriveList)
                {
                    // check if drive mapped
                    MappingState mpState = Utility.IsNetworkDrive(m.DriveLetter) ? MappingState.Mapped : MappingState.Unmapped;
                  
                        m.MappingStateProp = mpState;
                    
                }
                Thread.Sleep(timeMilliseconds);
            }
        }


        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                _stateResolver.TryMapAllDrives();
            }
            else
            {
                _stateResolver.TryUnmapAllDrives();
            }
        }


    }
}
