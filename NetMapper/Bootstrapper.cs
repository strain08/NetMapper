﻿using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using Splat;

namespace NetMapper;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
    {

        // Json Settings Store
        s.Register<IDataStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("NetMapperSettings.json"));

        // Json Drive List Store
        s.Register<IDataStore<AppDataModel>>(() => new JsonStore<AppDataModel>("NetMapperData.json"));

        // Settings
        s.RegisterConstant<ISettingsService>(new SettingsService(
            r.GetRequiredService<IDataStore<AppSettingsModel>>()));

        // Navigation service
        s.RegisterConstant<INavService>(new NavService());

        // Connect service
        s.RegisterLazySingleton<IConnectService>(() => new DriveConnectService(
            r.GetRequiredService<INavService>()
        ));

        // Drive List CRUD
        s.RegisterLazySingleton<IDriveListService>(() => new DriveListService(
            r.GetRequiredService<IDataStore<AppDataModel>>()));

        // Model state updater
        s.Register<IUpdateModelState>(() => new UpdateModelState());

        // System state updater
        s.Register<IUpdateSystemState>(() => new UpdateSystemState(
            r.GetRequiredService<IConnectService>()));

        // Pooling Service
        s.RegisterConstant(new PoolingService(
            r.GetRequiredService<IDriveListService>(),
            r.GetRequiredService<IUpdateModelState>(),
            r.GetRequiredService<IUpdateSystemState>()));
    }
}