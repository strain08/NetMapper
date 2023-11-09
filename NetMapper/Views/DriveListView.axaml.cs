using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.ViewModels;
using Splat;

namespace NetMapper.Views;

public partial class DriveListView : UserControl
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);    
    private readonly INavService nav;

#nullable disable
    public DriveListView() => InitializeComponent();
#nullable enable

    [ResolveThis]
    public DriveListView(INavService navService)
    {
        nav = navService;
        InitializeComponent();
    }

    // Dock doubleclick
    private void OnDoubleClick(object source, TappedEventArgs args)
    {
        var myList = this.FindControl<ListBox>("listBox");

        if (myList?.SelectedItem is not MapModel selectedItem) 
            return;

        nav.GoToNew<DriveDetailViewModel>().EditItem(selectedItem);        
    }
}