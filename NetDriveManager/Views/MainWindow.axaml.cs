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

        // window position does not update when closed

        protected override void OnClosed(EventArgs e)
        {
            StaticSettings.WindowIsOpened = false;
            base.OnClosed(e);
        }
    }
}