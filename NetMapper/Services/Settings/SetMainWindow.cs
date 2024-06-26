﻿using Avalonia;
using Avalonia.Controls;
using NetMapper.Views;
using System;

namespace NetMapper.Services.Settings;

public class SetMainWindow : SettingBase
{
    private readonly MainWindow MainWindowView;

    public SetMainWindow(MainWindow MainWindowView)
    {
        this.MainWindowView = MainWindowView;
    }

    public override void Apply()
    {
        MainWindowView.MinWidth = 250;
        MainWindowView.MinHeight = 150;
        MainWindowView.Width = GetAppSettings().WindowWidth;
        MainWindowView.Height = GetAppSettings().WindowHeight;
        MainWindowView.WindowStartupLocation = GetAppSettings().PositionOK()
            ? WindowStartupLocation.Manual
            : WindowStartupLocation.CenterScreen;

        MainWindowView.Opened -= MainWindowView_Opened;
        MainWindowView.Closing -= MainWindowView_Closing;
        MainWindowView.Resized -= MainWindowView_Resized;
        MainWindowView.PositionChanged -= MainWindowView_PositionChanged;
        MainWindowView.PropertyChanged -= MainWindowView_PropertyChanged;

        MainWindowView.Opened += MainWindowView_Opened;
        MainWindowView.Closing += MainWindowView_Closing;
        MainWindowView.Resized += MainWindowView_Resized;
        MainWindowView.PositionChanged += MainWindowView_PositionChanged;
        MainWindowView.PropertyChanged += MainWindowView_PropertyChanged;
    }

    private void MainWindowView_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (!GetAppSettings().SetMinimizeToTaskbar) return;

        if (e.NewValue is WindowState windowState && sender is Window window)
            switch (windowState)
            {
                case WindowState.Minimized:
                    window.Hide();
                    window.ShowInTaskbar = false;
                    break;
                default:
                    window.ShowInTaskbar = true;
                    break;
            }
    }

    private void MainWindowView_Opened(object? sender, EventArgs e)
    {
        if (GetAppSettings().PositionOK() && sender is Window window) window.Position = GetAppSettings().WindowPosition;
        GetAppSettings().WindowIsOpened = true;
    }

    private void MainWindowView_PositionChanged(object? sender, PixelPointEventArgs e)
    {
        if (GetAppSettings().WindowIsOpened) GetAppSettings().WindowPosition = e.Point;
    }

    private void MainWindowView_Resized(object? sender, WindowResizedEventArgs e)
    {
        if (sender is Window window)
        {
            GetAppSettings().WindowWidth = window.Width;
            GetAppSettings().WindowHeight = window.Height;
        }
    }

    private void MainWindowView_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (sender is Window window)
        {
            window.Hide();
            e.Cancel = true;
        }
    }
}