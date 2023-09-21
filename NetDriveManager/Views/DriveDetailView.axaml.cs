using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetMapper.Services;
using System.Linq;

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