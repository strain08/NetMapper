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
            foreach (MappingModel model in driveListService.DriveList)            
                ConnectDriveToast(model);            
        }

        public void TryUnmapAllDrives()
        {
            foreach (MappingModel model in driveListService.DriveList)            
                DisconnectDriveToast(model);            
        }

        public void ConnectDriveToast(MappingModel model)
        {
            Task.Run(() =>
            {
                switch (Utility.MapNetworkDrive(model.DriveLetter, model.NetworkPath))
                {
                    case ConnectResult.Success:

                        // toast notify success
                        notificationService.DriveConnectedToast(model, DriveAddedRemovedCallback);
                        break;
                    
                }
            });
        }

        public void DisconnectDriveToast(MappingModel model)
        {
            Task.Run(() =>
            {
                if (Utility.IsRegularDriveMapped(model.DriveLetter)) return;

                CancelConnection result = Utility.DisconnectNetworkDrive(model.DriveLetter);
                
                if (result == CancelConnection.DISCONNECT_SUCCESS)
                {
                    notificationService.DriveDisconnectedToast(model, DriveAddedRemovedCallback);

                }
                else
                {
                    notificationService.NotifyCanNotRemoveDrive(model, CanNotDisconnectDriveCallback);
                }
            });
        }

        private void CanNotDisconnectDriveCallback(MappingModel model, DisconnectDriveAnswer answer)
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
                            notificationService.DriveDisconnectedToast(model, DriveAddedRemovedCallback);
                        }
                        break;
                    case DisconnectDriveAnswer.ShowWindow:
                        ShowMainWindow();
                        break;
                }
            });
            
        }


        private void DriveAddedRemovedCallback(MappingModel mappingModel, AddRemoveAnswer toast)
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
