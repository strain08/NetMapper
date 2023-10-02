using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using NetMapper.Views;
using NetMapper.Services;
using NetMapper.Models;
using Splat;
using NetMapper.Services.Static;
using NetMapper.Interfaces;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel:ViewModelBase
    {
        public readonly MainWindow MainWindowView;

        public ApplicationViewModel()
        {
            StaticSettings.Settings = Locator.Current.GetRequiredService<IStore<AppSettingsModel>>().GetAll();

            MainWindowView = new MainWindow
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
            };            
            // hide window on close
            MainWindowView.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
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
            MainWindowView.Focus();

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
