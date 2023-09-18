using Avalonia;
using Avalonia.Controls;

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
        }
    }
}