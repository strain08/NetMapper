using Avalonia.Threading;
using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using NetMapper.Services.Toasts;
using NetMapper.ViewModels;
using Serilog;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class DriveConnectService : IDriveConnectService
    {
        private readonly NavService nav;
        //CTOR
        
        public DriveConnectService(NavService navService)
        {
            nav = navService;
        }

        public void ConnectDrive(MapModel m)
        {
            Task.Run(() =>
            {
                var result = Interop.ConnectNetworkDrive(m.DriveLetter, m.NetworkPath);

                switch (result)
                {
                    case ConnectResult.Success:
                        _ = new ToastDriveConnected(m, CallbackToastClicked);
                        break;
                    case ConnectResult.LoginFailure | ConnectResult.InvalidCredentials:
                        _ = new ToastLoginFailure(m, CallbackToastClicked);
                        break;
                    default:
                        Log.Error($"Error connecting to {m.NetworkPath}. Error code: {result} ");
                        break;
                }
            });
        }

        public void DisconnectDrive(MapModel m)
        {
            _ = Task.Run(() =>
            {
                // not a network drive, do nothing
                if (Interop.IsRegularDriveMapped(m.DriveLetter)) return;

                var result = Interop.DisconnectNetworkDrive(m.DriveLetter);

                switch (result)
                {
                    case CancelConnection.DISCONNECT_SUCCESS:
                        _ = new ToastDriveDisconnected(m, CallbackToastClicked);
                        break;
                    default:
                        _ = new ToastCanNotRemoveDrive(m, CallbackDisconnect);
                        break;
                }
            });
        }

        private void CallbackDisconnect(MapModel m, ToastActionsDisconnect answer)
        {
            switch (answer)
            {
                case ToastActionsDisconnect.Retry:
                    DisconnectDrive(m);
                    break;

                case ToastActionsDisconnect.Force:
                    Task.Run(() =>
                    {
                        CancelConnection error = Interop.DisconnectNetworkDrive(m.DriveLetter, true);
                        if (error == CancelConnection.DISCONNECT_SUCCESS)
                        {
                            _ = new ToastDriveDisconnected(m, CallbackToastClicked);
                        }
                    });
                    break;

                case ToastActionsDisconnect.ShowWindow:
                    ShowMainWindow();
                    break;
            }
        }

        private void CallbackToastClicked(MapModel m, ToastActionsSimple answer)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            Dispatcher.UIThread.Post(() =>
            {                
                var appVm = nav.GetViewModel(typeof(ApplicationViewModel)) as ApplicationViewModel;
                appVm?.ShowMainWindow();
            });
        }
    }
}
