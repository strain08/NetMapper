using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NetDriveManager.Services;
using NetDriveManager.ViewModels;
using NetDriveManager.Views;
using Splat;

namespace NetDriveManager
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
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
                
                DataContext = new ApplicationViewModel();
                
                //desktop.MainWindow = new MainWindow
                //{
                //    DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
                //};
            }
            base.OnFrameworkInitializationCompleted();
        }
        
    }
}