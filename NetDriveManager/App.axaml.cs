using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Services;
using NetMapper.ViewModels;
using Serilog;
using Splat;

namespace NetMapper;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static void AppContext(Action<IClassicDesktopStyleApplicationLifetime> command)
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            command.Invoke(application);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            
            // Register services with Splat
            Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.ShutdownRequested += Desktop_ShutdownRequested;
            desktop.Exit += Desktop_Exit;
        }


        if (Design.IsDesignMode)
            DataContext = new ApplicationViewModel();
        else
            DataContext = Locator.Current.CreateWithConstructorInjection<ApplicationViewModel>();
        base.OnFrameworkInitializationCompleted();
    }

    private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        AppShutdown();
    }

    private void Desktop_ShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        AppShutdown();
    }

    public void OnTrayClicked(object sender, EventArgs e)
    {
        var mainWindow = DataContext as ApplicationViewModel;
        mainWindow?.ShowWindowCommand();
    }

    private static void AppShutdown()
    {
        ToastNotificationManagerCompat.Uninstall();
        Locator.Current.GetRequiredService<ISettingsService>().SaveAll();
        Log.Information("Application exit.");
        Log.CloseAndFlush();
    }
}