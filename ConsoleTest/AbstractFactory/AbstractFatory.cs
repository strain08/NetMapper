using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.AbstractFactory
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }

    public class AbstractFactory<T> : IAbstractFactory<T>
    {
        private readonly Func<T> _factory;
        public AbstractFactory(Func<T> factory)
        {
            _factory = factory;
        }
        public T Create()
        {
            return _factory();
        }
    }

public interface IToastFactory
    {
        ToastBase SimpleToast(MapModel m, Action<MapModel,ToastActions> action);
        ToastBase ToastDisconnect(MapModel m, Action<MapModel, ToastActions> action);

    }

    public class Test
    {
        public Test()
        {
            var a = new AbstractFactory<MapModel>(() => new MapModel());
            
        }
    };
}
