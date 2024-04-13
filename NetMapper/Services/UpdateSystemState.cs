using NetMapper.Enums;
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
        if (CanAutoConnect(m))
            connectService.Connect(m);

        if (CanAutoDisconnect(m))
            connectService.Disconnect(m);
    }
    private bool CanAutoConnect(MapModel m)
    {
        bool result = true;
        result &= m.MappingStateProp == MapState.Unmapped;
        result &= m.ShareStateProp == ShareState.Available;
        result &= m.Settings.AutoConnect;
        return result;

    }
    private bool CanAutoDisconnect(MapModel m)
    {
        bool result = true;
        result &= m.MappingStateProp == MapState.Mapped;
        result &= m.ShareStateProp == ShareState.Unavailable;
        result &= m.Settings.AutoDisconnect;
        return result;

    }

}