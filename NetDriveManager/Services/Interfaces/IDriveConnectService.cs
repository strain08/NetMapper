using System.Threading.Tasks;
using NetMapper.Models;

namespace NetMapper.Services;

public interface IDriveConnectService
{
    public Task ConnectDrive(MapModel m);
    public Task DisconnectDrive(MapModel m);
}