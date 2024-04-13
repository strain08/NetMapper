using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Models;

namespace NetMapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    AppSettingsModel displaySettings;

    private readonly INavService nav;
    private readonly ISettingsService settings;

#nullable disable
    public SettingsViewModel() { }
#nullable enable

    [ResolveThis]
    public SettingsViewModel(INavService navService, ISettingsService settingsService)
    {
        nav = navService;
        settings = settingsService;
        DisplaySettings = settingsService.AppSettings.Clone();
    }

    public void OkCommand()
    {
        settings.AppSettings = DisplaySettings.Clone();
        settings.ApplyAll();
        settings.SaveAll();

        nav.GoTo<DriveListViewModel>();
    }

    public void CancelCommand()
    {
        nav.GoTo<DriveListViewModel>();
    }
}