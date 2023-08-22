using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetDriveManager.Models;
using NetDriveManager.Services;

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
        var si = lb!.SelectedItem as NDModel;

        VMServices.DriveDetailViewModel = new(true);

        // Pass the selected item to the VM
        VMServices.DriveDetailViewModel.DisplayItem = new(si!);


        // Navigate to DetailView
        VMServices.MainWindowViewModel!.Content = VMServices.DriveDetailViewModel;

    }
}