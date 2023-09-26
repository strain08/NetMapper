using Avalonia.Threading;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class DriveConnectService
    {
        private readonly ToastService toastService;        
        //CTOR
        public DriveConnectService( ToastService toastService)
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            

            this.toastService = toastService;            

        }

        public void ConnectDrive(MappingModel m)
        {
            Task.Run(() =>
            {
                var result = Utility.ConnectNetworkDrive(m.DriveLetter, m.NetworkPath);

                if (result == ConnectResult.Success)
                {
                    toastService.ToastDriveConnected(m, ToastClickedCallback);
                }
            });
        }

        public void DisconnectDrive(MappingModel m)
        {
            Task.Run(() =>
            {
                // regular drive, do nothing
                if (Utility.IsRegularDriveMapped(m.DriveLetter)) return;

                var result = Utility.DisconnectNetworkDrive(m.DriveLetter);

                if (result == CancelConnection.DISCONNECT_SUCCESS)
                {
                    toastService.ToastDriveDisconnected(m, ToastClickedCallback);
                }
                else
                {
                    toastService.ToastCanNotRemoveDrive(m, CanNotUnmapDriveCallback);
                }

            });
        }

        private void CanNotUnmapDriveCallback(MappingModel m, DisconnectDriveAnswer answer)
        {

            switch (answer)
            {
                case DisconnectDriveAnswer.Retry:
                    m.MappingStateProp = MappingState.Undefined;
                    DisconnectDrive(m);
                    break;

                case DisconnectDriveAnswer.Force:
                    CancelConnection error = Utility.DisconnectNetworkDrive(m.DriveLetter, true);
                    if (error == CancelConnection.DISCONNECT_SUCCESS)
                    {
                        toastService.ToastDriveDisconnected(m, ToastClickedCallback);
                    }
                    break;

                case DisconnectDriveAnswer.ShowWindow:
                    ShowMainWindow();
                    break;
            }
        }

        private void ToastClickedCallback(MappingModel mappingModel, AddRemoveAnswer toast)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            Dispatcher.UIThread.Post(() =>
            {
                VMServices.MainWindow!.WindowState = Avalonia.Controls.WindowState.Normal;
                VMServices.MainWindow.Show();
            });
        }
    }
}
