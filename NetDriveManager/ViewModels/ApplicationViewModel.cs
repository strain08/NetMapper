using Avalonia.Controls;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public MainWindow MainWindowView;
        readonly SettingsService settingsService;

        public ApplicationViewModel()
        {
            if (Design.IsDesignMode) return;


            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            settingsService.Add(new SetRunAtStartup());
            settingsService.Add(new SetMinimizeTaskbar());

            MainWindowView = new()
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
            };
            settingsService.Add(new SetMainWindow(MainWindowView));

            settingsService.ApplyAll();

            VMServices.ApplicationViewModel = this;
        }

        public void ShowWindowCommand()
        {
            App.AppContext((application) =>
            {
                application.MainWindow ??= MainWindowView;
                application.MainWindow.WindowState = WindowState.Normal;
                application.MainWindow.Show();
                application.MainWindow.BringIntoView();
                application.MainWindow.Focus();
            });
        }

        public static void ExitCommand()
        {
            App.AppContext((application) =>
            {
                application.Shutdown();
            });
        }
    }
}
