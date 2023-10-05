using NetMapper.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class DrivePoolingService
    {
        private readonly DriveListService driveListService;
        private readonly DriveConnectService stateResolverService;

        //CTOR
        public DrivePoolingService(DriveListService driveListService, DriveConnectService stateResolverService)
        {
            this.driveListService = driveListService;
            this.stateResolverService = stateResolverService;           

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            // Get share and mapping states into model at regular intervals
            Task.Run(() => StateLoop(5000));

        }
        //DTOR
        ~DrivePoolingService()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }

        private async void StateLoop(int timeMilliseconds)
        {
            List<Task> taskList = new();
            while (true)
            {
                foreach (MapModel m in driveListService.DriveList)
                {
                    taskList.Add(Task.Run(action: ()=> { 
                        UpdateModel(m);
                        UpdateSystem(m);
                    }));
                }
                await Task.WhenAll(taskList);
                taskList.Clear();
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void UpdateModel(MapModel m)
        {
            m.UpdateProperties();
        }

        private void UpdateSystem(MapModel m)
        {
            if (m.CanAutoConnect)
            {
                stateResolverService.ConnectDrive(m);
            }
            if (m.CanAutoDisconnect)
            {
                stateResolverService.DisconnectDrive(m);
            }
        }        
        
        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                foreach (MapModel m in driveListService.DriveList.Where((m)=> m.Settings.AutoConnect))
                    stateResolverService.ConnectDrive(m);
            }
            else
            {
                foreach (MapModel m in driveListService.DriveList.Where((m) => m.Settings.AutoDisconnect))
                    stateResolverService.DisconnectDrive(m);
            }
        }

    }
}
