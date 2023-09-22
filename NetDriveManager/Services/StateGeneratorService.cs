using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class StateGeneratorService
    {
        private readonly DriveListService _listManager;
        private readonly StateResolverService _stateResolver;

        //CTOR
        public StateGeneratorService(DriveListService listManager, StateResolverService stateResolver)
        {
            _listManager = listManager;
            _stateResolver = stateResolver;

            Task.Run(stateResolver.TryMapAllDrives);

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            // Get share and mapping states into model, at regular intervals
            Task.Run(() => ShareStateLoop(5000));
            Task.Run(() => MappingStateLoop(5000));
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
                    shareCheckTasks.Add(Task.Run(() => CheckShareState(m)));
                await Task.WhenAll(shareCheckTasks);
                shareCheckTasks.Clear();
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void CheckShareState(MappingModel m)
        {
            // check share state
            ShareState shState = Directory.Exists(m.NetworkPath)
                ? ShareState.Available : ShareState.Unavailable;
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
                    CheckMappingState(m);
                }
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void CheckMappingState(MappingModel m)
        {
            MappingState mpState;
            if (Utility.IsRegularDriveMapped(m.DriveLetter))
            {
                mpState = MappingState.LetterUnavailable;
            }
            else // no regular drive on this letter, may be a network drive
            {

                // check if network drive mapped
                 mpState = Utility.IsNetworkDriveMapped(m.DriveLetter)
                    ? MappingState.Mapped : MappingState.Unmapped;
                if (mpState == MappingState.Unmapped &&
                    m.ShareStateProp == ShareState.Available &&
                    m.MappingStateProp != MappingState.LetterUnavailable &&
                    m.MappingSettings.AutoConnect)
                {
                    _stateResolver.ConnectDriveToast(m);
                }
                
            }
            m.MappingStateProp = mpState;
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
