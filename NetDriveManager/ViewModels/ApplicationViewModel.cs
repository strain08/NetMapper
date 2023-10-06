using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;
using System.Diagnostics;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public MainWindow MainWindowView;
        readonly SettingsService settingsService;

        public ApplicationViewModel()
        {            
            if (Design.IsDesignMode) return;

            VMServices.ApplicationViewModel = this;
            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            settingsService.Add(new SetRunAtStartup());
            settingsService.Add(new SetMinimizeTaskbar());
            MainWindowView = new()
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
            };
            settingsService.Add(new SetMainWindow(MainWindowView));
            settingsService.ApplyAll();
        }

        public void ShowWindowCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.MainWindow ??= MainWindowView;
                application.MainWindow.WindowState = WindowState.Normal;
                application.MainWindow.Show();
                application.MainWindow.BringIntoView();                
            }
        }


        public static void ExitCommand()
        {         

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }

            
        }
    }
}
