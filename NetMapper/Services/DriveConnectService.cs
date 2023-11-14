using Avalonia.Threading;
using NetMapper.Attributes;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Toasts;
using NetMapper.Services.Toasts.Interfaces;
using NetMapper.ViewModels;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetMapper.Services;

public class DriveConnectService : IConnectService
{
    private readonly INavService nav;
    private readonly IInterop interop;
    private readonly IToastFactory toastFactory;
    IToastPresenter toastPresent;
    IToast? toast;


    public DriveConnectService(INavService navService,
                               IInterop interopService,
                               IToastFactory toastFactory)
    {
        nav = navService;
        interop = interopService;
        this.toastFactory = toastFactory;
        toastPresent = toastFactory.CreateToastPresenter();
    }

    public async Task Connect(MapModel m)
    {
        m.MappingStateProp = MapState.Undefined;

        var result = await interop.ConnectNetworkDriveAsync(m.DriveLetter, m.NetworkPath);

        switch (result)
        {
            case ConnectResult.Success:
                m.MappingStateProp = MapState.Mapped;
                //_ = new ToastDriveConnected(m, ActionToastClicked).Show();                
                toast = toastFactory.CreateToast("TAG1", ToastType.INF_CONNECT, m);
                toastPresent.Show(toast);
                break;

            case ConnectResult.LoginFailure | ConnectResult.InvalidCredentials:
                _ = new ToastLoginFailure(m, ActionToastClicked, "TAG1").Show();
                break;

            default:
                var errorDescription = result.GetAttributeOfType<DescriptionAttribute>()?.GetDescription();
                Log.Error($"Error connecting to {m.NetworkPath}. Error code: {result}, {errorDescription} ");
                break;
        }
    }

    public async Task Disconnect(MapModel m, bool forceDisconnect = false)
    {
        m.MappingStateProp = MapState.Undefined;

        // not a network drive, do nothing
        if (interop.IsRegularDriveMapped(m.DriveLetter)) return;

        var result = await interop.DisconnectNetworkDriveAsync(m.DriveLetter, forceDisconnect);

        switch (result)
        {
            case DisconnectResult.DISCONNECT_SUCCESS:
                _ = new ToastDriveDisconnected(m, ActionToastClicked, "TAG1").Show();
                m.MappingStateProp = MapState.Unmapped;
                break;

            default:
                _ = new ToastCanNotRemoveDrive(m, ActionDisconnect, "TAG2").Show();
                break;
        }
    }

    private void ActionDisconnect(MapModel m, ToastActions answer)
    {
        switch (answer)
        {
            case ToastActions.Retry:
                _ = Disconnect(m);
                break;

            case ToastActions.Force:
                _ = Disconnect(m, forceDisconnect: true);
                break;

            case ToastActions.ToastClicked:
                ShowMainWindow();
                break;
        }
    }

    private void ActionToastClicked(MapModel m, ToastActions answer)
    {
        if (m.MappingStateProp == MapState.Mapped)
        {
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = m.DriveLetterColon + "\\"
            };
            Process.Start(psi);
        }
        else
        {
            ShowMainWindow();
        }
    }

    private void ShowMainWindow()
    {
        Dispatcher.UIThread.Post(() =>
        {
            nav.GetViewModel<ApplicationViewModel>()
                .ShowMainWindow();
        });
    }
}