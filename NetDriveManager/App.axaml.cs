using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Services;
using NetMapper.Services.Static;
using NetMapper.ViewModels;
using Splat;
using System;

namespace NetMapper
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public static void AppContext(Action<IClassicDesktopStyleApplicationLifetime> command)
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                command.Invoke(application);
            }
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
            DataContext = new ApplicationViewModel();

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            var s = Locator.Current.GetRequiredService<SettingsService>();
            s.SaveAll();
        }

        private void Desktop_ShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            var s = Locator.Current.GetRequiredService<SettingsService>();
            s.SaveAll();
        }

        public void OnTrayClicked(object sender, EventArgs e)
        {

            if (VMServices.ApplicationViewModel == null) return;
            VMServices.ApplicationViewModel.ShowWindowCommand();


        }
    }
}