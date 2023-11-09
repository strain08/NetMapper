using System.Threading.Tasks;
using NetMapper.Models;

namespace NetMapper.Interfaces;

public interface IConnectService
{
    public Task Connect(MapModel m);
    public Task Disconnect(MapModel m);
}