using NetMapper.Models;
using System.Collections.Generic;
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

            // Get share and mapping states into model, at regular intervals
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
                foreach (MappingModel m in driveListService.DriveList)
                {
                    taskList.Add(Task.Run(action: ()=>UpdateModel(m)));
                }
                await Task.WhenAll(taskList);
                taskList.Clear();
                Thread.Sleep(timeMilliseconds);
            }
        }

        private void UpdateModel(MappingModel m)
        {
            m.UpdateProperties();

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
                foreach (MappingModel m in driveListService.DriveList)
                    stateResolverService.ConnectDrive(m);
            }
            else
            {
                foreach (MappingModel m in driveListService.DriveList)
                    stateResolverService.DisconnectDrive(m);
            }
        }


    }
}
