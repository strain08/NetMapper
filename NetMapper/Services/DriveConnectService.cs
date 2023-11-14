﻿using Avalonia.Threading;
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
    IToastPresenter toast;
    IToastType toastType;
    

    public DriveConnectService(INavService navService,
                               IInterop interopService,
                               IToastFactory toastFactory)
    {
        nav = navService;
        interop = interopService;
        this.toastFactory = toastFactory;
        toast = toastFactory.CreateToast();
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
               // var toastArgs = new ToastArgsRecord(T);
                toastType = toastFactory.CreateToastType("TAG1",new(ToastType.INF_CONNECT, m));
                toast.Show(toastType);
                break;

            case ConnectResult.LoginFailure | ConnectResult.InvalidCredentials:
                _ = new ToastLoginFailure(m, ActionToastClicked).Show();
                break;

            default:
                var errorDescription = result.GetAttributeOfType<DescriptionAttribute>()?.GetDescription();
                Log.Error($"Error connecting to {m.NetworkPath}. Error code: {result}, {errorDescription} ");
                break;
        }
    }

    public async Task Disconnect(MapModel m)
    {
        m.MappingStateProp = MapState.Undefined;
        
        // not a network drive, do nothing
        if (interop.IsRegularDriveMapped(m.DriveLetter)) return;

        var result = await interop.DisconnectNetworkDriveAsync(m.DriveLetter);

        switch (result)
        {
            case DisconnectResult.DISCONNECT_SUCCESS:
                _ = new ToastDriveDisconnected(m, ActionToastClicked).Show();
                m.MappingStateProp = MapState.Unmapped;
                break;

            default:
                _ = new ToastCanNotRemoveDrive(m, ActionDisconnect).Show();
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
                Task.Run(() =>
                {
                    var error = interop.DisconnectNetworkDrive(m.DriveLetter, true);
                    if (error == DisconnectResult.DISCONNECT_SUCCESS)
                        _ = new ToastDriveDisconnected(m, ActionToastClicked).Show();
                });
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