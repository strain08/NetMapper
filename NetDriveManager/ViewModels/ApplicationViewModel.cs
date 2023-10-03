using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public readonly MainWindow MainWindowView;
        public readonly IStore<AppSettingsModel> Store;

        public ApplicationViewModel()
        {
            Store = Locator.Current.GetRequiredService<IStore<AppSettingsModel>>();
            StaticSettings.Settings = Store.GetAll();
            RunAtStartup r = new(StaticSettings.Settings);
            r.Apply();

            MainWindowView = new()
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel(),
                MinWidth = 250,
                MinHeight = 150,
                Width = StaticSettings.Settings?.WindowWidth ?? 225,
                Height = StaticSettings.Settings?.WindowHeight ?? 400,
                WindowStartupLocation = StaticSettings.PositionOK() ? WindowStartupLocation.Manual : WindowStartupLocation.CenterScreen,
            };

            // hide window on close
            MainWindowView.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
            MainWindowView.Resized += (s, e) =>
            {
                StaticSettings.Settings!.WindowWidth = ((MainWindow)s!).Width;
                StaticSettings.Settings.WindowHeight = ((MainWindow)s!).Height;
            };

            MainWindowView.PositionChanged += (s, e) =>
            {
                if (StaticSettings.WindowIsOpened)
                {
                    StaticSettings.Settings!.WinX = ((MainWindow)s!).Position.X;
                    StaticSettings.Settings.WinY = ((MainWindow)s!).Position.Y;
                }

            };
            VMServices.ApplicationViewModel = this;
            MainWindowView.PropertyChanged += _mainWindow_PropertyChanged;

        }

        private void _mainWindow_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (StaticSettings.Settings != null)
            {
                if (!StaticSettings.Settings.bMinimizeToTaskbar) return;
            }
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
        }

        public void ShowWindowCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow ??= MainWindowView;
            }
            MainWindowView.WindowState = WindowState.Normal;
            MainWindowView.Show();
            MainWindowView.BringIntoView();
        }


        public void ExitCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }
        }
    }
}
