using Avalonia;
using Avalonia.Controls;
using NetMapper.Services.Static;
using System;

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
    }
}