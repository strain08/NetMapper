using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public class ConnManager
    {
        private readonly INDManager _ndmanager;
        public ConnManager(INDManager ndmanager)
        {
            _ndmanager = ndmanager;
            Thread t = new Thread(() => TryConnectDrives() );
            t.Start();
            
        }
        private void TryConnectDrives()
        {
            foreach (NDModel model in _ndmanager.NetDriveList)
            {
                ConnectResult result;
                result = (ConnectResult)Utility.MapNetworkDrive(model.DriveLetter[0].ToString(), model.Provider);
                Debug.WriteLine("Connection result of {0} is {1}", model.DriveLetter, result);
            }
        }
        
    }
}
