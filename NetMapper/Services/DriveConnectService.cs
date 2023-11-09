﻿using Avalonia.Threading;
using NetMapper.Attributes;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using NetMapper.Services.Toasts;
using NetMapper.ViewModels;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetMapper.Services;

public class DriveConnectService : IConnectService
{
    private readonly INavService nav;

    public DriveConnectService(INavService navService)
    {
        nav = navService;
    }

    public async Task Connect(MapModel m)
    {
        m.MappingStateProp = MappingState.Undefined;

        var result = await Interop.ConnectNetworkDriveAsync(m.DriveLetter, m.NetworkPath);

       switch (result)
        {
            case ConnectResult.Success:
                m.MappingStateProp = MappingState.Mapped;
                _ = new ToastDriveConnected(m, ActionToastClicked).Show();
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
        m.MappingStateProp = MappingState.Undefined;

        // not a network drive, do nothing
        if (Interop.IsRegularDriveMapped(m.DriveLetter)) return;

        var result = await Interop.DisconnectNetworkDriveAsync(m.DriveLetter);
        
        switch (result)
        {
            case DisconnectResult.DISCONNECT_SUCCESS:
                _ = new ToastDriveDisconnected(m, ActionToastClicked).Show();
                m.MappingStateProp = MappingState.Unmapped;
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
                    var error = Interop.DisconnectNetworkDrive(m.DriveLetter, true);
                    if (error == DisconnectResult.DISCONNECT_SUCCESS)
                        _ = new ToastDriveDisconnected(m, ActionToastClicked).Show();
                });
                break;

            case ToastActions.ShowWindow:
                ShowMainWindow();
                break;
        }
    }

    private void ActionToastClicked(MapModel m, ToastActions answer)
    {
        ProcessStartInfo psi = new()
        {
            UseShellExecute = true,
            FileName = m.DriveLetterColon + "\\"
        };
        Process.Start(psi);
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