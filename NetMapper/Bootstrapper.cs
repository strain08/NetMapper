using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using Splat;

namespace NetMapper;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
    {

        s.RegisterConstant<IInterop>(new Interop());
        // Json Settings Store
        s.Register<IDataStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("NetMapperSettings.json"));

        // Json Drive List Store
        s.Register<IDataStore<AppDataModel>>(() => new JsonStore<AppDataModel>("NetMapperData.json"));

        // Settings
        s.RegisterConstant<ISettingsService>(new SettingsService(
            r.GetRequiredService<IDataStore<AppSettingsModel>>()));

        // Navigation service
        s.RegisterConstant<INavService>(new NavService(r));

        // Connect service
        s.RegisterLazySingleton<IConnectService>(() => new DriveConnectService(
            r.GetRequiredService<INavService>(),
            r.GetRequiredService<IInterop>()
        ));

        // Drive List CRUD
        s.RegisterLazySingleton<IDriveListService>(() => new DriveListService(
            r.GetRequiredService<IDataStore<AppDataModel>>()));

        // Model state updater
        s.Register<IUpdateModelState>(() => new UpdateModelState(r.GetRequiredService<IInterop>()));

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