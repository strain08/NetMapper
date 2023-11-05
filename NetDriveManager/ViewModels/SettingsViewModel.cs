using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Interfaces;
using Splat;

namespace NetMapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly INavService navService;

    private readonly ISettingsService settingsService;

    [ObservableProperty] private AppSettingsModel displaySettings;

    public SettingsViewModel()
    {
        //if (Design.IsDesignMode) return; // design mode bypass            
        settingsService = Locator.Current.GetRequiredService<ISettingsService>();
        navService = Locator.Current.GetRequiredService<INavService>();

        DisplaySettings = settingsService.GetAppSettings().Clone();
    }

    public void OkCommand()
    {
        settingsService.SetAppSettings(DisplaySettings.Clone());
        settingsService.ApplyAll();
        settingsService.SaveAll();

        navService.GoTo<DriveListViewModel>();
    }

    public void CancelCommand()
    {
        navService.GoTo<DriveListViewModel>();
    }
}