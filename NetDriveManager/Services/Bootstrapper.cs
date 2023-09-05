using NetDriveManager.Interfaces;
using NetDriveManager.Services.Monitoring;
using Splat;

namespace NetDriveManager.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {

            // transient data storage
            s.Register
                <IStorage>(() => new JsonStore("NetDriveSettings.json"));
            // singleton list manager for mappings
            s.RegisterConstant
                <DriveListManager>(new DriveListManager(r.GetService<IStorage>()));
            s.RegisterLazySingleton
                <NetworkAvailabilityMon>(() => new NetworkAvailabilityMon());
            s.RegisterConstant
                <ConnectionEventHandler>(new ConnectionEventHandler(r.GetService<DriveListManager>(), r.GetService<NetworkAvailabilityMon>()));

        }
    }
}
