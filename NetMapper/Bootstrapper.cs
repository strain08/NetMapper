using System.Collections.Generic;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Interfaces;
using NetMapper.Services.Stores;
using Splat;

namespace NetMapper;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
    {
        // navigation service
        s.RegisterConstant<INavService>(new NavService());

        // Json Settings Store
        s.Register<IDataStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("NetMapperSettings.json"));

        // Json Drive List Store
        s.Register<IDataStore<List<MapModel>>>(() => new JsonStore<List<MapModel>>("NetMapperData.json"));

        // Settings
        s.RegisterConstant<ISettingsService>(new SettingsService(
            r.GetRequiredService<IDataStore<AppSettingsModel>>()));

        // Connect service
        s.RegisterLazySingleton<IDriveConnectService>(() => new DriveConnectService(
            r.GetRequiredService<INavService>()
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
}