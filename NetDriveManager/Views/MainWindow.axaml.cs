using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
            this.Height = 500;
            this.Width = 325;
#if DEBUG
            this.AttachDevTools();
#endif
        }
        

    }
}