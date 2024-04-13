using NetMapper.Models;
using System.Threading.Tasks;

namespace NetMapper.Interfaces;

public interface IConnectService
{
    public Task Connect(MapModel m);
    public Task Disconnect(MapModel m, bool forceDisconnect = false);
}