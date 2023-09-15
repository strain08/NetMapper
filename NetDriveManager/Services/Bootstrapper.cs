using NetDriveManager.Interfaces;
using NetDriveManager.Services.Helpers;
using Splat;
using System;
using System.ComponentModel.Design;

namespace NetDriveManager.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {
            // JsonStore
            s.Register<IStorage>(() => new JsonStore("NetDriveSettings.json"));

            // Notification
            s.RegisterConstant<NotificationService>(new NotificationService());            
           

            // DriveListManager
            s.RegisterConstant<DriveListService>(new DriveListService(
                r.GetRequiredService<IStorage>()));

            // StateResolver
            s.RegisterConstant<StateResolverService>(new StateResolverService(
                r.GetRequiredService<DriveListService>(),
                r.GetRequiredService<NotificationService>()));

            // StateGenerator
            s.RegisterConstant<StateGeneratorService>(new StateGeneratorService(
                r.GetRequiredService<DriveListService>(),
                r.GetRequiredService<StateResolverService>()));
            
            
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
