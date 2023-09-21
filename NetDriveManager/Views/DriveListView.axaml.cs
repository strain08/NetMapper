using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.ViewModels;
namespace NetMapper.Views;

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
        MappingModel selectedDriveModel = (MappingModel)driveListBox.SelectedItem!;

        // Navigate to DetailView
        VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel(selectedDriveModel);

    }
}