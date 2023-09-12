using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NetDriveManager.Services;
using NetDriveManager.ViewModels;
using System;
using System.Diagnostics;

namespace NetDriveManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Height = 400;
            this.Width = 250;
#if DEBUG
            this.AttachDevTools();
#endif
            VMServices.mainWindow = this;
        }
        

    }
}