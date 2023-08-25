using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NetDriveManager.Models;
using NetDriveManager.Services;
using NetDriveManager.Services.Helpers;
using NetDriveManager.ViewModels;
using NetDriveManager.Views;

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

                Database.jsonSettingsFile = "NetDriveSettings.json";
                Database.ReadFromFile();
                
                

                desktop.MainWindow = new MainWindow
                {
                    DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel(),
                };
            }
            
            base.OnFrameworkInitializationCompleted();
        }
        
        public override void RegisterServices()
        {
            base.RegisterServices();
        }
    }
}