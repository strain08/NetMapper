using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using System;
using System.Threading;

namespace NetDriveManager.Services
{
    public class ConnManager
    {
        private readonly INDManager _ndmanager;
        private readonly NetMonitor _netMonitor;
        public ConnManager(INDManager ndmanager, NetMonitor netMonitor)
        {

            _ndmanager = ndmanager;
            _netMonitor = netMonitor;

            _netMonitor.ConnectionClosed += _netMonitor_ConnectionClosed;
            _netMonitor.ConnectionOnline += _netMonitor_ConnectionOnline;
            Thread t = new Thread(() => TryConnectDrives());
            t.Start();

        }

        private void _netMonitor_ConnectionOnline(object? sender, EventArgs e)
        {
            TryConnectDrives();
        }

        private void _netMonitor_ConnectionClosed(object? sender, EventArgs e)
        {
            TryDisconnectDrives();
        }


        private void TryConnectDrives()
        {
            foreach (NDModel model in _ndmanager.NetDriveList)
            {
                ConnectResult result;
                result = (ConnectResult)Utility.MapNetworkDrive(model.DriveLetter[0].ToString(), model.NetworkPath);
                UIUpdate(model, result);
            }
        }
        private void TryDisconnectDrives()
        {
            foreach (NDModel model in _ndmanager.NetDriveList)
            {
                CancelConnection result;
                result=Utility.DisconnectNetworkDrive(model.DriveLetter[0].ToString(), true);
                UIUpdate(model, result);
            }
        }

        private void UIUpdate(NDModel model, object status)
        {
            switch (status)
            {
                case ConnectResult:

                    ConnectResult connectResult = (ConnectResult)status;
                    HandleConnectionResult(model, connectResult);
                    break;
                case CancelConnection:
                    CancelConnection cancelConnection = (CancelConnection)status;
                    HandleCancelConnectionResult(model, cancelConnection);
                    break;
            }
        }

        private void HandleCancelConnectionResult(NDModel model, CancelConnection cancelConnection)
        {
            switch (cancelConnection)
            {
                case CancelConnection.DISCONNECT_SUCCESS:
                    UIMessage(model, "Disconnected", "Red");
                    break;
                case CancelConnection.DISCONNECT_FAILURE:
                    UIMessage(model, "Failed to disconnect", "Red");
                    break;
            }
        }

        private void HandleConnectionResult(NDModel model, ConnectResult connectResult)
        {
            switch (connectResult)
            {
                case ConnectResult.DriveLetterAlreadyAssigned:
                    if (Utility.GetPathForLetter(model.DriveLetter[0].ToString()) == model.NetworkPath)
                    {
                        UIMessage(model, "Connected", "Green");
                    }
                    else
                    {
                        UIMessage(model, "Mapped to a different path", "Yellow");
                    }
                    break;
                case ConnectResult.Success:
                    UIMessage(model, "Connected", "Green");
                    break;
                default:
                    UIMessage(model, "Disconnected", "Red");
                    break;
            }

        }


        private void UIMessage(NDModel model, string message, string color)
        {
            model.ConnectionState = message;
            model.ConnectionColor = color;
        }

    }
}
