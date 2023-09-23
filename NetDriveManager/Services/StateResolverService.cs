using Avalonia.Threading;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class StateResolverService
    {
        private readonly NotificationService notificationService;
        private readonly DriveListService driveListService;

        public StateResolverService(DriveListService driveListService, NotificationService notificationService)
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass            

            this.notificationService = notificationService;
            this.driveListService = driveListService;

        }
        public void TryMapAllDrives()
        {
            foreach (MappingModel m in driveListService.DriveList)
                MapDriveToast(m);
        }

        public void TryUnmapAllDrives()
        {
            foreach (MappingModel m in driveListService.DriveList)
                UnmapDriveToast(m);
        }

        public void MapDriveToast(MappingModel m)
        {
            Task.Run(() =>
            {
                var result = Utility.MapNetworkDrive(m.DriveLetter, m.NetworkPath);

                if (result == ConnectResult.Success)
                {
                    notificationService.DriveConnectedToast(m, ToastClickedCallback);
                }
            });
        }

        public void UnmapDriveToast(MappingModel m)
        {
            Task.Run(() =>
            {
                if (Utility.IsRegularDriveMapped(m.DriveLetter)) return;

                var result = Utility.DisconnectNetworkDrive(m.DriveLetter);

                if (result == CancelConnection.DISCONNECT_SUCCESS)
                {
                    notificationService.DriveDisconnectedToast(m, ToastClickedCallback);
                }
                else
                {
                    notificationService.NotifyCanNotRemoveDrive(m, CanNotUnmapDriveCallback);
                }
            });
        }

        private void CanNotUnmapDriveCallback(MappingModel model, DisconnectDriveAnswer answer)
        {
            Task.Run(() =>
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
                            notificationService.DriveDisconnectedToast(model, ToastClickedCallback);
                        }
                        break;
                    case DisconnectDriveAnswer.ShowWindow:
                        ShowMainWindow();
                        break;
                }
            });

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
