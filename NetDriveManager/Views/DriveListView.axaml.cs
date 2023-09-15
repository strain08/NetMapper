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
        ListBox driveListBox = (ListBox)source;
        DriveModel selectedDriveModel = (DriveModel)driveListBox.SelectedItem!;

        // Navigate to DetailView
        VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel(selectedDriveModel);

    }
}