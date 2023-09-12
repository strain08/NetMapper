using Avalonia.Threading;
using NetDriveManager.Enums;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using Splat;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public class StateResolver
    {
        private readonly TaskFactory taskFactory = new();
        private readonly NotificationService notificationService;

        public StateResolver()
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass      
            notificationService = Locator.Current.GetRequiredService<NotificationService>();

        }

        internal void ConnectDrive(MappingModel model)
        {
            taskFactory.StartNew(() =>
            {
                if (Utility.MapNetworkDrive(model.DriveLetter[0], model.NetworkPath) == 0)
                {
                    notificationService.ToastDriveAdded(model, DriveAddedRemovedCallback);
                }
            });

        }

        internal void DisconnectDrive(MappingModel model)
        {
            taskFactory.StartNew(() =>
            {
                CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter[0]);
                switch (error)
                {
                    case CancelConnection.DISCONNECT_SUCCESS:
                        notificationService.ToastDriveRemoved(model, DriveAddedRemovedCallback);
                        break;
                    default:
                        notificationService.ToastCanNotRemoveDrive(model, CanNotRemoveDriveCallback);
                        break;

                }
            });
        }

        private void CanNotRemoveDriveCallback(MappingModel model, RemoveDriveAnswer answer)
        {
            switch (answer)
            {
                case RemoveDriveAnswer.Retry:
                    VMServices.DriveListViewModel.DisconnectDriveCommand(model);
                    break;
                case RemoveDriveAnswer.Cancel:
                    break;
                case RemoveDriveAnswer.Force:
                    CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter[0], true);
                    if (error == CancelConnection.DISCONNECT_SUCCESS)
                    {
                        notificationService.ToastDriveRemoved(model, DriveAddedRemovedCallback);
                    }
                    break;
                case RemoveDriveAnswer.ShowWindow:
                    ShowMainWindow();
                    break;

            }
        }

        private void DriveAddedRemovedCallback(MappingModel mappingModel, AddRemoveAnswer toast)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            Dispatcher.UIThread.Post(() =>
            {
                VMServices.mainWindow!.WindowState = Avalonia.Controls.WindowState.Normal;
                VMServices.mainWindow.Activate();
            });
        }
    }
}
