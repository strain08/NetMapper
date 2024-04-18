using NetMapper.Attributes;
using NetMapper.Enums;
using NetMapper.Extensions;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Toasts.Interfaces;
using Serilog;
using System.Threading.Tasks;

namespace NetMapper.Services;

public class DriveConnectService : IConnectService
{
    private readonly IInterop interop;
    private readonly IToastFactory toastFactory;
    IToastPresenter toastPresenter;

    public DriveConnectService(IInterop interopService,
                               IToastFactory toastFactory)
    {        
        interop = interopService;
        this.toastFactory = toastFactory;
        toastPresenter = toastFactory.CreateToastPresenter();
    }

    public async Task Connect(MapModel m)
    {
        IToast toast;
        m.MappingStateProp = MapState.Undefined;

        var result = await interop.ConnectNetworkDriveAsync(m.DriveLetter, m.NetworkPath);

        switch (result)
        {
            case ConnectResult.Success:
                m.MappingStateProp = MapState.Mapped;
                toast = toastFactory.CreateToast("INFO", ToastType.INF_CONNECT, m);
                break;

            case ConnectResult.LoginFailure | ConnectResult.InvalidCredentials:
                toast = toastFactory.CreateToast("FAILURE", ToastType.INF_LOGIN_FAILURE, m);
                break;

            default:
                var errorDescription = result.GetAttributeOfType<DescriptionAttribute>()?.GetDescription();
                var errorMessage = $"Error connecting to {m.NetworkPath}. Error code: {result}, {errorDescription} ";
                toast = toastFactory.CreateToast(m.ID, ToastType.INF_CUSTOM, m, "Unknown error", errorMessage);
                Log.Error(errorMessage);
                break;
        }

        toastPresenter.Show(toast);
    }

    public async Task Disconnect(MapModel m, bool forceDisconnect = false)
    {
        IToast toast;
        m.MappingStateProp = MapState.Undefined;

        // not a network drive, do nothing
        if (interop.IsRegularDriveMapped(m.DriveLetter)) return;

        var result = await interop.DisconnectNetworkDriveAsync(m.DriveLetter, forceDisconnect);

        switch (result)
        {
            case DisconnectResult.Success:
                toast = toastFactory.CreateToast("INFO", ToastType.INF_DISCONNECT, m);
                m.MappingStateProp = MapState.Unmapped;
                break;

            case DisconnectResult.OpenFiles:
                toast = toastFactory.CreateToast("CAN_NOT_REMOVE", ToastType.DLG_CAN_NOT_DISCONNECT, m);
                break;

            default:
                var errorDescription = result.GetAttributeOfType<DescriptionAttribute>()?.GetDescription();
                var errorMessage = $"Error disconnecting {m.DriveLetterColon}. Error code: {result}, {errorDescription} ";
                toast = toastFactory.CreateToast("ERROR_DISCONNECT", ToastType.INF_CUSTOM, m, "Unknown error", errorMessage);
                Log.Error(errorMessage);
                break;
        }

        toastPresenter.Show(toast);
    }


}