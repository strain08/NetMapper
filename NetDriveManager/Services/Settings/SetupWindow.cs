using Avalonia;
using Avalonia.Controls;
using NetMapper.Models;
using NetMapper.Views;
using System;
using System.Diagnostics;

namespace NetMapper.Services.Settings
{
    internal class SetupWindow : SettingBase
    {
        readonly MainWindow MainWindowView;        
        public SetupWindow(AppSettingsModel settings, MainWindow MainWindowView) : base(settings)
        {
            this.MainWindowView = MainWindowView;
        }

        public override void Apply()
        {
            MainWindowView.MinWidth = 250;
            MainWindowView.MinHeight = 150;
            MainWindowView.Width = settings.WindowWidth;
            MainWindowView.Height = settings.WindowHeight;
            MainWindowView.WindowStartupLocation = settings.PositionOK() ?
                WindowStartupLocation.Manual : WindowStartupLocation.CenterScreen;
            if (!settings.EventsInitialized)
            {
                MainWindowView.Opened += MainWindowView_Opened;
                MainWindowView.Closing += MainWindowView_Closing;
                MainWindowView.Resized += MainWindowView_Resized;
                MainWindowView.PositionChanged += MainWindowView_PositionChanged;                
                MainWindowView.PropertyChanged += MainWindowView_PropertyChanged;
                settings.EventsInitialized = true;
            }
        }

        void MainWindowView_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
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
        }

        void MainWindowView_Opened(object? sender, EventArgs e)
        {
            if (settings.PositionOK())
            {
                ((Window)sender!).Position = settings.WindowPosition;
            }
            settings.WindowIsOpened = true;
        }

        void MainWindowView_PositionChanged(object? sender, PixelPointEventArgs e)
        {
            if (settings.WindowIsOpened)
            {
                settings.WindowPosition = e.Point;
            }
        }

        void MainWindowView_Resized(object? sender, WindowResizedEventArgs e)
        {
            settings.WindowWidth = ((Window)sender!).Width;
            settings.WindowHeight = ((Window)sender!).Height;
        }

        void MainWindowView_Closing(object? sender, WindowClosingEventArgs e)
        {
            ((Window)sender!).Hide();
            e.Cancel = true;
        }
    }
}

