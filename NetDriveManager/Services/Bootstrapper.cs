using NetDriveManager.Interfaces;
using NetDriveManager.Services.Helpers;
using Splat;
using System;

namespace NetDriveManager.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {
            s.RegisterConstant<NotificationService>(new NotificationService());

            // JsonStore
            s.Register<IStorage>(() => new JsonStore("NetDriveSettings.json"));

            // DriveListManager
            s.RegisterLazySingleton<DriveListManager>(() => new DriveListManager(r.GetRequiredService<IStorage>()));

            // StateManager
            s.RegisterConstant<StateGenerator>(new StateGenerator(r.GetRequiredService<DriveListManager>()));

        }
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {
            var service = resolver.GetService<TService>() ??
                throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}"); // throw error with detailed description

             // design mode bypass      
            return service; // return instance if not null
        }
        
    }

    

}
