using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;

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
            settingsService.Add(new SetupWindow(settingsService.Settings, MainWindowView));
            settingsService.ApplyAll();

            MainWindowView.DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel();

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
