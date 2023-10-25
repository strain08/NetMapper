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
            s.Register<IDataStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("NetMapperSettings.json"));

            // Json Drive List Store
            s.Register<IDataStore<List<MapModel>>>(() => new JsonStore<List<MapModel>>("NetMapperData.json"));

            // Settings
            s.RegisterConstant<ISettingsService>(new SettingsService(
                r.GetRequiredService<IDataStore<AppSettingsModel>>()));

            // Connect service
            s.RegisterLazySingleton<IDriveConnectService>(() => new DriveConnectService());

            // Drive List CRUD
            s.RegisterLazySingleton<IDriveListService>(() => new DriveListService(
                r.GetRequiredService<IDataStore<List<MapModel>>>()));

            // StateGenerator
            s.RegisterConstant(new DrivePoolingService(
                r.GetRequiredService<IDriveListService>(),
                r.GetRequiredService<IDriveConnectService>()));

            s.RegisterLazySingleton(() => new NavService());
        }

        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver) where TService : class
        {
            TService service = resolver.GetService<TService>() ??
                throw new InvalidOperationException($"Splat failed to resolve object of type {typeof(TService)}");  // throw error with detailed description

            return service; // return instance if not null
        }

    }



}
