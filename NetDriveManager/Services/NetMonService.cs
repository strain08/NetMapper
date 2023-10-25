using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    internal class NetMonService
    {
        private readonly IDriveConnectService driveConnectService;
        private readonly IDriveListService driveListService;
        
        // CTOR
        public NetMonService(
            IDriveListService driveListService, 
            IDriveConnectService driveConnectService)
        {
            this.driveConnectService = driveConnectService;
            this.driveListService = driveListService;
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        }
        
        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                foreach (MapModel m in driveListService.DriveList.Where((m) => m.Settings.AutoConnect))
                    driveConnectService.ConnectDrive(m);
            }
            else
            {
                foreach (MapModel m in driveListService.DriveList.Where((m) => m.Settings.AutoDisconnect))
                    driveConnectService.DisconnectDrive(m);
            }
        }
    }
}
