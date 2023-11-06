using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NetMapper.Views;

public partial class DriveDetailView : UserControl
{
    public DriveDetailView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}