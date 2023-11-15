using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.ViewModels;
using System;
using Svg.DataTypes;
using System.Diagnostics;

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
        ActualThemeVariantChanged += DriveListView_ActualThemeVariantChanged;
    }

    private void DriveListView_ActualThemeVariantChanged(object? sender, EventArgs e)
    {
        
        var imageSettingsLight = this.FindControl<Image>("SettingsLight");
        
        var imageSettingsDark = this.FindControl<Image>("SettingsDark");
        var imageInfoDark = this.FindControl<Image>("InfoDark");
        var imageInfoLight = this.FindControl<Image>("InfoLight");
        var s = ActualThemeVariant.ToString();
        switch (s)
        {
            case "Dark":
                imageInfoDark.IsVisible = true;
                imageInfoLight.IsVisible = false;
                imageSettingsDark.IsVisible = true;
                imageSettingsLight.IsVisible = false;                
                break;

            case "Light":
                imageInfoDark.IsVisible = false;
                imageInfoLight.IsVisible = true;
                imageSettingsDark.IsVisible = false;
                imageSettingsLight.IsVisible = true;
                
                break;
        }

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