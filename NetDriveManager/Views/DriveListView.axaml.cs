using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Models;
using NetMapper.Services.Interfaces;
using NetMapper.ViewModels;
using Splat;

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
        var MyList = this.FindControl<ListBox>("listBox");
        var selectedItem = (MapModel?)MyList?.SelectedItem;
        if (selectedItem == null) return;

        //var x = Locator.Current.CreateWithConstructorInjection<DriveDetailViewModel>();
        var nav = Locator.Current.GetRequiredService<INavService>();
        //nav.GoTo(x.EditItem(selectedItem));

        nav.GoToNew<DriveDetailViewModel>().EditItem(selectedItem);
        //nav.GetViewModel<DriveDetailViewModel>().EditItem(selectedItem);
        //nav.GoTo(new DriveDetailViewModel().EditItem((MapModel?)MyList?.SelectedItem));
    }
}