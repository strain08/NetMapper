using Avalonia;
using Avalonia.Controls;

namespace NetMapper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Height = 400;
            this.Width = 225;
#if DEBUG
            this.AttachDevTools();
#endif
            MinWidth = this.Width;
        }
    }
}