using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetDriveManager.Services;
using System.Linq;

namespace NetDriveManager.Views;

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