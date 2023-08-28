using NetDriveManager.Interfaces;
using Splat;

namespace NetDriveManager.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {

            // transient data storage
            s.Register<IStorage>(
                () => new JsonStore("NetDriveSettings.json"));
            // singleton list manager for mappings
            s.RegisterConstant<NDManager>(
                new NDManager(r.GetService<IStorage>()));
            s.RegisterConstant<ConnManager>(
                new ConnManager(r.GetService<NDManager>()));

        }
    }
}
