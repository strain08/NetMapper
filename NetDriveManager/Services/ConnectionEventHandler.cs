using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using NetDriveManager.Services.Monitoring;
using Splat;
using System.Collections.Generic;
using System.Threading;

namespace NetDriveManager.Services
{
    public class ConnectionEventHandler
    {
        private readonly IListManager _listManager;
        private readonly NetworkAvailabilityMon _netMonitor;
        private readonly DriveAddRemoveMon _wmiEventHandler;
        private readonly ShellCommand _shellCmd;

        public ConnectionEventHandler(
            IListManager listManager,
            NetworkAvailabilityMon netMonitor
            )
        {

            _shellCmd = Locator.Current.GetService<ShellCommand>() 
                ?? throw new KeyNotFoundException("Error getting service " + typeof(ShellCommand));
            _wmiEventHandler = new DriveAddRemoveMon();
            _wmiEventHandler.OnDriveConnected += WmiDriveConnected;

            _listManager = listManager;

            _netMonitor = netMonitor;
            _netMonitor.OnConnectionOnline += _netMonitor_ConnectionOnline;
            _netMonitor.OnConnectionOffline += _netMonitor_ConnectionClosed;

            var t = new Thread(() => TryConnectDrives());
            t.Start();

            var timer1 = new System.Timers.Timer();
            timer1.Interval = 5000;
            timer1.Elapsed += Timer_NetUseStatusChanged;
            timer1.Start();
        }

        private void Timer_NetUseStatusChanged(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (MappingModel m in _listManager.NetDriveList)
            {
                switch (_shellCmd.NetUseStatus(m.DriveLetter))
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
            _wmiEventHandler.OnDriveConnected -= WmiDriveConnected;
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
