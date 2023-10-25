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
        private const int POLL_INTERVAL_MSEC = 5000;
        private readonly IDriveListService driveListService;
        private readonly IDriveConnectService driveConnectService;

        //CTOR
        public DrivePoolingService()
        {

        }
        public DrivePoolingService(
            IDriveListService driveListService, 
            IDriveConnectService driveConnectService)
        {
            this.driveListService = driveListService;
            this.driveConnectService = driveConnectService;
            // Get share and mapping states into model at regular intervals
            Task.Run(() => StateLoop(POLL_INTERVAL_MSEC));
        }        

        private async void StateLoop(int timeMilliseconds)
        {
            List<Task> taskList = new();
            while (true)
            {
                foreach (MapModel m in driveListService.DriveList)
                {
                    taskList.Add(Task.Run(()=> { 
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
                driveConnectService.ConnectDrive(m);                
            }
            if (m.CanAutoDisconnect)
            {
                driveConnectService.DisconnectDrive(m);
            }
        }
    }
}
