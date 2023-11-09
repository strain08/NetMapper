using NetMapper.Interfaces;
using NetMapper.Models;

namespace NetMapper.Services;

public class UpdateSystemState : IUpdateSystemState
{
    private readonly IConnectService connectService;

    public UpdateSystemState(IConnectService driveConnectService)
    {
        connectService = driveConnectService;
    }

    public void Update(MapModel m)
    {
        if (m.CanAutoConnect) connectService.Connect(m);
        if (m.CanAutoDisconnect) connectService.Disconnect(m);
    }
}