using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.ViewModels;
using System;
using System.Collections.Generic;

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
        List<Image?> lightImg = new();
        List<Image?> darkImg = new();
        lightImg.Add(this.FindControl<Image>("SettingsLight"));
        lightImg.Add(this.FindControl<Image>("InfoLight"));
        darkImg.Add(this.FindControl<Image>("SettingsDark"));
        darkImg.Add(this.FindControl<Image>("InfoDark"));

        ThemeVariant? s = ActualThemeVariant;

        if (s != null)
        {
            bool showDark = (s.ToString() == "Dark");
            foreach (var img in lightImg)
                if (img != null) img.IsVisible = !showDark;
            foreach (var img in darkImg)
                if (img != null) img.IsVisible = showDark;
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