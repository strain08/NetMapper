using Avalonia.Threading;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class DriveConnectService
    {
        private readonly NotificationService notificationService;
        private readonly DriveListService driveListService;

        public DriveConnectService(DriveListService driveListService, NotificationService notificationService)
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            

            this.notificationService = notificationService;
            this.driveListService = driveListService;

        }

        public void ConnectDrive(MappingModel m)
        {
            Task.Run(() =>
            {
                var result = Utility.ConnectNetworkDrive(m.DriveLetter, m.NetworkPath);

                if (result == ConnectResult.Success)
                {
                    notificationService.ToastDriveConnected(m, ToastClickedCallback);
                }
            });
        }

        public void DisconnectDrive(MappingModel m)
        {
            Task.Run(() =>
            {
                if (Utility.IsRegularDriveMapped(m.DriveLetter)) return;

                var result = Utility.DisconnectNetworkDrive(m.DriveLetter);

                if (result == CancelConnection.DISCONNECT_SUCCESS)
                {
                    notificationService.ToastDriveDisconnected(m, ToastClickedCallback);
                }
                else
                {
                    notificationService.ToastCanNotRemoveDrive(m, CanNotUnmapDriveCallback);
                }

            });
        }

        private void CanNotUnmapDriveCallback(MappingModel model, DisconnectDriveAnswer answer)
        {

            switch (answer)
            {
                case DisconnectDriveAnswer.Retry:
                    VMServices.DriveListViewModel.DisconnectDriveCommand(model);
                    break;

                case DisconnectDriveAnswer.Force:
                    CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter, true);
                    if (error == CancelConnection.DISCONNECT_SUCCESS)
                    {
                        notificationService.ToastDriveDisconnected(model, ToastClickedCallback);
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
