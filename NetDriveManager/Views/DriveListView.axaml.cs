using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using NetMapper.Models;
using NetMapper.Services;
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
        ListBox? MyList = this.FindControl<ListBox>("listBox");
        var nav = Locator.Current.GetRequiredService<NavService>();
        nav.GoTo(new DriveDetailViewModel((MapModel?)MyList?.SelectedItem));
    }
}