using NetDriveManager.Interfaces;
using NetDriveManager.Services.Helpers;
using Splat;

namespace NetDriveManager.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {
            // calls cmd shell
            s.Register<ShellCommand>(() => new ShellCommand());          
            
            // returns network connection status
            s.Register<MonNetworkAvailability>(() => new MonNetworkAvailability());

            // transient data storage
            s.Register<IStorage>(() => new JsonStore("NetDriveSettings.json"));

            // singleton list manager for mappings
            s.RegisterLazySingleton<DriveListManager>( ()=> new DriveListManager(r.GetService<IStorage>()));           

            // drive state manager
            s.RegisterConstant<StateManager>(new StateManager(r.GetService<DriveListManager>()));


        }
    }
}
