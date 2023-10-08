using Avalonia.Threading;

using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using NetMapper.Services.Static;
using NetMapper.Services.Toasts;
using NetMapper.ViewModels;
using System;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class DriveConnectService
    {
        //IDisposable? a;
        private readonly ToastService toastService;
        //CTOR
        public DriveConnectService(ToastService toastService)
        {
            this.toastService = toastService;
        }

        public void ConnectDrive(MapModel m)
        {
            Task.Run(() =>
            {
                var result = Utility.ConnectNetworkDrive(m.DriveLetter, m.NetworkPath);

                switch(result)
                {
                    case ConnectResult.Success:
                        //toastService.ToastDriveConnected(m, ToastClickedCallback);
                        _ = new ToastDriveConnected(m, ToastClickedCallback);
                        break;
                    case ConnectResult.LoginFailure | ConnectResult.InvalidCredentials :
                        toastService.ToastBadLogin(m, ToastClickedCallback);
                        break;
                }
                GC.Collect();
                
            });
        }

        public void DisconnectDrive(MapModel m)
        {
            _ = Task.Run(() =>
            {
                // not a network drive, do nothing
                if (Utility.IsRegularDriveMapped(m.DriveLetter)) return;

                var result = Utility.DisconnectNetworkDrive(m.DriveLetter);

                switch (result)
                {
                    case CancelConnection.DISCONNECT_SUCCESS:
                        //toastService.ToastDriveDisconnected(m, ToastClickedCallback);
                        _ = new ToastDriveDisconnected(m, ToastClickedCallback);

                        break;
                    default:
                        toastService.ToastDriveDisconnectError(m, UnableToDisconnectCallback);
                       // a = new ToastCanNotRemoveDrive(m, UnableToDisconnectCallback);
                        break;
                }
            });
        }

        private void UnableToDisconnectCallback(MapModel m, ToastActionsDisconnect answer)
        {
            switch (answer)
            {
                case ToastActionsDisconnect.Retry:
                    //m.MappingStateProp = MappingState.Undefined;
                    DisconnectDrive(m);
                    break;

                case ToastActionsDisconnect.Force:
                    Task.Run(() =>
                    {
                        CancelConnection error = Utility.DisconnectNetworkDrive(m.DriveLetter, true);
                        if (error == CancelConnection.DISCONNECT_SUCCESS)
                        {
                            toastService.ToastDriveDisconnected(m, ToastClickedCallback);
                           // _ = new ToastDriveDisconnected(m, ToastClickedCallback);
                        }
                    });
                    break;

                case ToastActionsDisconnect.ShowWindow:
                    ShowMainWindow();
                    break;
            }
        }

        private void ToastClickedCallback(MapModel m, ToastActionsSimple answer)
        {
            ShowMainWindow();
        }

        private static void ShowMainWindow()
        {
            Dispatcher.UIThread.Post(() =>
            {
                //App.AppContext((app) => {
                //    app.MainWindow?.Show(); ;
                //});
                VMServices.ApplicationViewModel?.ShowWindowCommand();
            });
        }
    }
}
