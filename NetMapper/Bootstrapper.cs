using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using NetMapper.Services.Toasts.Implementations;
using NetMapper.Services.Toasts.Interfaces;
using Splat;

namespace NetMapper;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
    {
        if (Avalonia.Controls.Design.IsDesignMode) return;

        // Json Settings Store
        s.Register<IDataStore<AppSettingsModel>>(() =>
            new JsonStore<AppSettingsModel>(AppDataFiles.SettingsFile));

        // Json Drive List Store
        s.Register<IDataStore<AppDataModel>>(() =>
            new JsonStore<AppDataModel>(AppDataFiles.MapDataFile));
        
        // Interop, system-related functions
        s.RegisterConstant<IInterop>(new Interop());
        
        // Settings
        s.RegisterConstant<ISettingsService>(new SettingsService(
            r.GetRequiredService<IDataStore<AppSettingsModel>>()));

        // Navigation service
        s.RegisterConstant<INavService>(new NavService(r));

        // Connect service
        s.RegisterLazySingleton<IConnectService>(() => new DriveConnectService(
            r.GetRequiredService<INavService>(),
            r.GetRequiredService<IInterop>(),
            r.GetRequiredService<IToastFactory>()));

        // Drive List CRUD
        s.RegisterLazySingleton<IDriveListService>(() => new DriveListService(
            r.GetRequiredService<IDataStore<AppDataModel>>()));

        // Toast
        s.Register<IToastFactory>(() => new ToastFactory());

        s.RegisterConstant(new ToastActivatedReceiver(
            r.GetRequiredService<IToastFactory>(),
            r.GetRequiredService<IDriveListService>(),
            r.GetRequiredService<IConnectService>(),
            r.GetRequiredService<INavService>(),
            r.GetRequiredService<IInterop>()));

        s.RegisterConstant<IToastActivatedMessenger>(new ToastActivatedMessenger(
            r.GetRequiredService<IDriveListService>()));

        s.RegisterConstant(new ToastActivatedHandler(
            r.GetRequiredService<IToastActivatedMessenger>()));

        // Model state updater
        s.Register<IUpdateModelState>(() => new UpdateModelState(
            r.GetRequiredService<IInterop>()));

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