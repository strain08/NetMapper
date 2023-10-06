using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Static;
using Splat;

namespace NetMapper.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        AppSettingsModel displaySettings;

        readonly SettingsService settingsService;

        public SettingsViewModel()
        {
            //if (Design.IsDesignMode) return; // design mode bypass            
            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            DisplaySettings = settingsService.AppSettings.Clone();
        }
        public void OkCommand()
        {
            settingsService.AppSettings = DisplaySettings.Clone();
            settingsService.ApplyAll();
            settingsService.SaveAll();
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
        public static void CancelCommand()
        {
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
    }
}
