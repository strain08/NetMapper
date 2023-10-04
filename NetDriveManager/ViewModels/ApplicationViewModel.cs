using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.Services;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public readonly MainWindow MainWindowView;
        readonly SettingsService settingsService;

        public ApplicationViewModel()
        {
            VMServices.ApplicationViewModel = this;

            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            settingsService.ApplyAll();

            MainWindowView = new()
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel(),
                MinWidth = 250,
                MinHeight = 150,
                Width = settingsService.Settings.WindowWidth,
                Height = settingsService.Settings.WindowHeight,
                WindowStartupLocation = settingsService.Settings.PositionOK() ? WindowStartupLocation.Manual : WindowStartupLocation.CenterScreen,
            };

            // hide window on close
            MainWindowView.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
            MainWindowView.Resized += (s, e) =>
            {
                settingsService.Settings.WindowWidth = ((MainWindow)s!).Width;
                settingsService.Settings.WindowHeight = ((MainWindow)s!).Height;
            };

            MainWindowView.PositionChanged += (s, e) =>
            {
                if (settingsService.WindowIsOpened)
                {
                    settingsService.Settings.WindowPosition = MainWindowView.Position;
                }

            };
            MainWindowView.Opened += (s, e) =>
            {
                if (settingsService.Settings.PositionOK())
                {
                    ((Window)s!).Position = settingsService.Settings.WindowPosition;
                }
                settingsService.WindowIsOpened = true;

            };

            MainWindowView.PropertyChanged += (s, e) => 
            {
                if (!settingsService.Settings.bMinimizeToTaskbar) return;

                if (e.NewValue is WindowState windowState)
                {
                    switch (windowState)
                    {
                        case WindowState.Minimized:
                            MainWindowView.Hide();
                            MainWindowView.ShowInTaskbar = false;
                            break;
                        default:
                            MainWindowView.ShowInTaskbar = true;
                            break;
                    }
                }
            };

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
