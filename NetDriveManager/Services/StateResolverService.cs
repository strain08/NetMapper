using Avalonia.Threading;
using NetDriveManager.Enums;
using NetDriveManager.Models;
using NetDriveManager.Services.Helpers;
using Splat;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public class StateResolverService
    {
        private readonly TaskFactory taskFactory = new();
        private readonly NotificationService notificationService;
        private readonly DriveListService driveListService;

        public StateResolverService(DriveListService driveListService, NotificationService notificationService)
        {
            if (Avalonia.Controls.Design.IsDesignMode) return; // design mode bypass
            

            this.notificationService = notificationService;
            this.driveListService = driveListService;
            notificationService.TestMessage();
        }

        internal void ShareStateChanged(DriveModel m)
        {
            if (m.ShareStateProp == ShareState.Available &&
                m.MappingStateProp == MappingState.Unmapped &&
                m.MappingSettings.AutoConnect
                )
            {
                ConnectDriveToast(m);
                return;
            }
            if (m.ShareStateProp == ShareState.Unavailable &&
                m.MappingStateProp == MappingState.Mapped &&
                m.MappingSettings.AutoDisconnect
                )
            {
                DisconnectDriveToast(m);
            }
        }

        internal void ConnectDriveToast(DriveModel model)
        {
            taskFactory.StartNew(() =>
            {
                ConnectResult test = Utility.MapNetworkDrive(model.DriveLetter, model.NetworkPath);

                switch (test)
                {
                    case ConnectResult.Success:
                        notificationService.ToastDriveAdded(model, DriveAddedRemovedCallback);
                        break;
                    case ConnectResult.DriveLetterAlreadyAssigned:
                        if (!driveListService.ContainsDriveLetter(model.DriveLetter))
                        {
                            DisconnectDriveToast(model);
                            ConnectDriveToast(model);
                        }
                        
                        break;
                }
            });
        }

        internal void DisconnectDriveToast(DriveModel model)
        {
            taskFactory.StartNew(() =>
            {
                CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter);
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

        private void CanNotRemoveDriveCallback(DriveModel model, RemoveDriveAnswer answer)
        {
            switch (answer)
            {
                case RemoveDriveAnswer.Retry:
                    VMServices.DriveListViewModel.DisconnectDriveCommand(model);
                    break;

                case RemoveDriveAnswer.Force:
                    CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter, true);
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

        private void DriveAddedRemovedCallback(DriveModel mappingModel, AddRemoveAnswer toast)
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
