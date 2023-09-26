using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.ViewModels;
using System;

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

    // Dock doubleclick
    private void OnDoubleClick(object source, TappedEventArgs args)
    {
        ListBox? MyList = this.FindControl<ListBox>("listBox");
        VMServices.MainWindowViewModel!.Content = new DriveDetailViewModel((MappingModel?)MyList?.SelectedItem);
    }
}