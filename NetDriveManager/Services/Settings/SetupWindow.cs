using Avalonia.Controls;
using NetMapper.Models;
using NetMapper.Services.Static;
using NetMapper.ViewModels;
using NetMapper.Views;
using System;
using Windows.ApplicationModel.Background;

namespace NetMapper.Services.Settings
{
    internal class SetupWindow : SettingBase
    {
        public SetupWindow(AppSettingsModel settings) : base(settings)
        {
        }

        public override void Apply()
        {
        }

        public override void Apply(object obj)
        {
            MainWindow? MainWindowView = (MainWindow)obj;
            MainWindowView.MinWidth = 250;
            MainWindowView.MinHeight = 150;
            MainWindowView.Width = settings.WindowWidth;
            MainWindowView.Height = settings.WindowHeight;
            MainWindowView.WindowStartupLocation = settings.PositionOK() ? 
                WindowStartupLocation.Manual : WindowStartupLocation.CenterScreen;
            
            // hide window on close
            MainWindowView.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
            MainWindowView.Resized += (s, e) =>
            {
                settings.WindowWidth = ((MainWindow)s!).Width;
                settings.WindowHeight = ((MainWindow)s!).Height;
            };

            MainWindowView.PositionChanged += (s, e) =>
            {
                if (settings.WindowIsOpened)
                {
                    settings.WindowPosition = MainWindowView.Position;
                }

            };
            MainWindowView.Opened += (s, e) =>
            {
                if (settings.PositionOK())
                {
                    ((Window)s!).Position = settings.WindowPosition;
                }
                settings.WindowIsOpened = true;

            };

            MainWindowView.PropertyChanged += (s, e) =>
            {
                if (!settings.bMinimizeToTaskbar) return;

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
    }
}
