using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.Services;
using NetMapper.Services.Static;
using NetMapper.Extensions;
using NetMapper.Views;
using Splat;
using NetMapper.Services.Settings;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public MainWindow MainWindowView = new();
        readonly SettingsService settingsService;

        public ApplicationViewModel()
        {
            VMServices.ApplicationViewModel = this;

            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            settingsService.ApplyAll();

            var w = settingsService.SettingsList.GetSetting(typeof(SetupWindow));
            w?.Apply(MainWindowView);

            MainWindowView!.DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel();

        }

        public void ShowWindowCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow ??= MainWindowView;
                desktop.MainWindow.WindowState = WindowState.Normal;
                desktop.MainWindow.Show();
                desktop.MainWindow.BringIntoView();
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
