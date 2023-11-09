using NetMapper.Enums;
using NetMapper.Models;
using System;

namespace NetMapper.Services
{
    public class test : IDriveResultAction
    {
        public void Connect(ConnectResult result)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(DisconnectResult result)
        {
            throw new NotImplementedException();
        }

        public void SetModel(MapModel m)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDriveResultAction : 
        IActionModel<MapModel>, 
        IActionConnect<ConnectResult>, 
        IActionDisconnect<DisconnectResult> 
    { }
    public interface IActionModel<TModel> where TModel : class      
    {     
        void SetModel(TModel m);
    }
    public interface IActionConnect<T> where T : Enum
    {
        void Connect(T result);
    }
    public interface IActionDisconnect<T> where T: Enum
    {
        void Disconnect(T result);
    }
}
