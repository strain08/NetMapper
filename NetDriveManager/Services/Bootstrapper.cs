using NetMapper.Models;
using NetMapper.Services.Interfaces;
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
            // navigation service
            s.RegisterLazySingleton(() => new NavService());

            // Json Settings Store
            s.Register<IDataStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("NetMapperSettings.json"));

            // Json Drive List Store
            s.Register<IDataStore<List<MapModel>>>(() => new JsonStore<List<MapModel>>("NetMapperData.json"));

            // Settings
            s.RegisterConstant<ISettingsService>(new SettingsService(
                r.GetRequiredService<IDataStore<AppSettingsModel>>()));

            // Connect service
            s.RegisterLazySingleton<IDriveConnectService>(() => new DriveConnectService(
                r.GetRequiredService<NavService>()
                ));

            // Drive List CRUD
            s.RegisterLazySingleton<IDriveListService>(() => new DriveListService(
                r.GetRequiredService<IDataStore<List<MapModel>>>()));

            // Model state updater
            s.Register<IUpdateModelState>(() => new UpdateModelState());
            
            // System state updater
            s.Register<IUpdateSystemState>(() => new UpdateSystemState(
                r.GetRequiredService<IDriveConnectService>()));
            
            // Pooling Service
            s.RegisterConstant(new PoolingService(
                r.GetRequiredService<IDriveListService>(),
                r.GetRequiredService<IUpdateModelState>(),
                r.GetRequiredService<IUpdateSystemState>()));            
        }

        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver) where TService : class
        {
            TService service = resolver.GetService<TService>() ??
                throw new InvalidOperationException($"Splat failed to resolve object of type {typeof(TService)}");  // throw error with detailed description

            return service; // return instance if not null
        }

    }



}
