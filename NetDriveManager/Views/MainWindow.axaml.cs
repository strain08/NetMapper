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
            Width = StaticSettings.Settings?.WindowWidth ?? 225;
            Height = StaticSettings.Settings?.WindowHeight ?? 400;           

#if DEBUG
            this.AttachDevTools();
#endif

        }

        // window position does not update when closed
        protected override void OnOpened(EventArgs e)
        {
            if (StaticSettings.PositionOK())
            {
                Position = new(StaticSettings.Settings.WinX, StaticSettings.Settings.WinY);
            }
            StaticSettings.WindowIsOpened = true;
            base.OnOpened(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            StaticSettings.WindowIsOpened = false;
            base.OnClosed(e);
        }
    }
}