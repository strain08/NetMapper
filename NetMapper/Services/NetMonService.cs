using System.Linq;
using System.Net.NetworkInformation;
using NetMapper.Interfaces;

namespace NetMapper.Services;

internal class NetMonService
{
    private readonly IConnectService driveConnectService;
    private readonly IDriveListService driveListService;

    // CTOR
    public NetMonService(
        IDriveListService driveListService,
        IConnectService driveConnectService)
    {
        this.driveConnectService = driveConnectService;
        this.driveListService = driveListService;
        NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
    }

    private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
    {
        if (e.IsAvailable)
            foreach (var m in driveListService.DriveCollection.Where(m => m.Settings.AutoConnect))
                driveConnectService.Connect(m);
        else
            foreach (var m in driveListService.DriveCollection.Where(m => m.Settings.AutoDisconnect))
                driveConnectService.Disconnect(m);
    }
}