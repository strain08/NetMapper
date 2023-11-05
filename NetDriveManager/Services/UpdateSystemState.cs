using NetMapper.Models;
using NetMapper.Services.Interfaces;

namespace NetMapper.Services;

public class UpdateSystemState : IUpdateSystemState
{
    private readonly IDriveConnectService connectService;

    public UpdateSystemState(IDriveConnectService driveConnectService)
    {
        connectService = driveConnectService;
    }

    public void Update(MapModel m)
    {
        if (m.CanAutoConnect) connectService.ConnectDrive(m);
        if (m.CanAutoDisconnect) connectService.DisconnectDrive(m);
    }
}