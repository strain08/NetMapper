using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetDriveManager.Models;
using NetDriveManager.Services;
using NetDriveManager.ViewModels;
namespace NetDriveManager.Views;

public partial class DriveListView : UserControl
{
    public DriveListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnDoubleClick(object source, TappedEventArgs args)
    {
        var lb = source as ListBox;
        var si = lb!.SelectedItem as MappingModel;        
        
        // Navigate to DetailView
        VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel(si);

    }
}