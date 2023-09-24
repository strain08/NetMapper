using Avalonia;
using Avalonia.Controls;

namespace NetMapper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Height = 400;
            Width = 225;
#if DEBUG
            this.AttachDevTools();
#endif
            MinWidth = Width;
        }
    }
}