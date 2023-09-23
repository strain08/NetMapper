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

            //Task.Run(stateResolver.TryMapAllDrives);

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            // Get share and mapping states into model, at regular intervals
            Task.Run(() => StateLoop(5000));

        }


        //DTOR
        ~StateGeneratorService()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }

        private async void StateLoop(int timeMilliseconds)
        {
            List<Task> taskList = new();
            while (true)
            {
                foreach (MappingModel m in _listManager.DriveList)
                {
                    taskList.Add(Task.Run(m.UpdateProperties));
                }
                await Task.WhenAll(taskList);
                taskList.Clear();
                Thread.Sleep(timeMilliseconds);
                //
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
