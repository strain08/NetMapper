using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using NetDriveManager.Services.Monitoring;
using System.Threading;

namespace NetDriveManager.Services
{
    public class ConnectionEventHandler
    {
        private readonly IListManager _listManager;
        private readonly NetworkAvailabilityMon _netMonitor;
        private readonly DriveAddRemoveMon _wmiEventHandler;

        public ConnectionEventHandler(IListManager listManager, NetworkAvailabilityMon netMonitor)
        {

            _wmiEventHandler = new DriveAddRemoveMon();
            _wmiEventHandler.DriveConnectedDelegate += WmiDriveConnected;

            _listManager = listManager;

            _netMonitor = netMonitor;
            _netMonitor.OnConnectionOnline += _netMonitor_ConnectionOnline;
            _netMonitor.OnConnectionOffline += _netMonitor_ConnectionClosed;

            var t = new Thread(() => TryConnectDrives());
            t.Start();

            var timer1 = new System.Timers.Timer();
            timer1.Interval = 3000;
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();


        }

        private void Timer1_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (MappingModel m in _listManager.NetDriveList)
            {
                var test = ExecuteCmd.NetUseStatus(m.DriveLetter);
                switch (test)
                {
                    case "OK":
                        m.ConnectionState = ConnectionState.Connected;
                        break;
                    case "":
                        m.ConnectionState = ConnectionState.Disconnected;
                        break;
                    default:
                        m.ConnectionState = ConnectionState.Degraded;
                        break;
                }

            }
        }

        ~ConnectionEventHandler()
        {
            _wmiEventHandler.DriveConnectedDelegate -= WmiDriveConnected;
            _netMonitor.OnConnectionOnline -= _netMonitor_ConnectionOnline;
            _netMonitor.OnConnectionOffline -= _netMonitor_ConnectionClosed;
        }

        

        private void WmiDriveConnected(string driveLetter)
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                if (model.DriveLetter == driveLetter)
                {
                    //UIMessage(model, )
                }
            }
        }

        private void _netMonitor_ConnectionOnline()
        {
            var t = new Thread(() => TryConnectDrives());
            t.Start();
        }

        private void _netMonitor_ConnectionClosed()
        {
            var t = new Thread(() => TryDisconnectDrives());
            t.Start();

        }

        private void TryConnectDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.MapNetworkDrive(model.DriveLetter[0].ToString(), model.NetworkPath);
            }
        }
        private void TryDisconnectDrives()
        {
            foreach (MappingModel model in _listManager.NetDriveList)
            {
                Utility.DisconnectNetworkDrive(model.DriveLetter[0].ToString(), true);
            }
        }








    }
}
