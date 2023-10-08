using NetMapper.Models;
using NetMapper.Services.Stores;
using Splat;
using System;
using System.Collections.Generic;

namespace NetMapper.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {
            // Json Settings Store
            s.Register<IStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("AppSettings.json"));

            // Json Drive List Store
            s.Register<IStore<List<MapModel>>>(() => new JsonStore<List<MapModel>>("NetDriveSettings.json"));

            s.RegisterConstant(new SettingsService(
                r.GetRequiredService<IStore<AppSettingsModel>>()));

            // Notification
            s.RegisterConstant(new ToastService());

            // Connect service
            s.RegisterConstant(new DriveConnectService(
                r.GetRequiredService<ToastService>()));

            // Drive List CRUD
            s.RegisterConstant(new DriveListService(
                r.GetRequiredService<IStore<List<MapModel>>>()));

            // StateGenerator
            s.RegisterConstant(new DrivePoolingService(
                r.GetRequiredService<DriveListService>(),
                r.GetRequiredService<DriveConnectService>()));
        }

        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver) where TService : class
        {
            TService service = resolver.GetService<TService>() ??
                throw new InvalidOperationException($"Splat failed to resolve object of type {typeof(TService)}"); // throw error with detailed description

            return service; // return instance if not null
        }

    }



}
