using NetMapper.Models;

namespace NetMapper.Services
{
    public interface IDriveConnectService
    {
        void ConnectDrive(MapModel m);
        void DisconnectDrive(MapModel m);
    }
}