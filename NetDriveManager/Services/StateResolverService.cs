using Avalonia.Threading;
using NetDriveManager.Enums;
using NetDriveManager.Interfaces;
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
            
        }
        public void TryMapAllDrives()
        {
            foreach (MappingModel model in driveListService.DriveList)
            {
                ConnectDriveToast(model);
            }
        }

        public void TryUnmapAllDrives()
        {
            foreach (MappingModel model in driveListService.DriveList)
            {
                DisconnectDriveToast(model);
            }
        }

        public void ConnectDriveToast(MappingModel model)
        {
            taskFactory.StartNew(() =>
            {
                switch (Utility.MapNetworkDrive(model.DriveLetter, model.NetworkPath))
                {
                    case ConnectResult.Success:

                        // toast notify success
                        notificationService.DriveConnectedToast(model, DriveAddedRemovedCallback);
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

        public void DisconnectDriveToast(MappingModel model)
        {
            taskFactory.StartNew(() =>
            {
                CancelConnection error = Utility.DisconnectNetworkDrive(model.DriveLetter);
                switch (error)
                {
                    case CancelConnection.DISCONNECT_SUCCESS:
                        notificationService.DriveDisconnectedToast(model, DriveAddedRemovedCallback);
                        break;
                        
                    default:
                        notificationService.NotifyCanNotRemoveDrive(model, CanNotRemoveDriveCallback);
                        break;

                }
            });
        }

        private void CanNotRemoveDriveCallback(MappingModel model, DisconnectDriveAnswer answer)
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
                        notificationService.DriveDisconnectedToast(model, DriveAddedRemovedCallback);
                    }
                    break;
                case DisconnectDriveAnswer.ShowWindow:
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
