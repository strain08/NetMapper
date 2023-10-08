using Avalonia;
using Avalonia.Controls;
using NetMapper.Services.Static;
using System;
using System.Diagnostics;

namespace NetMapper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            Debug.Print("Closing");
            base.OnClosing(e);
        }
    }
}