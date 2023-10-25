using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using Splat;

namespace NetMapper.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        AppSettingsModel displaySettings;

        private readonly ISettingsService settingsService;
        private readonly NavService navService;
        public SettingsViewModel()
        {
            //if (Design.IsDesignMode) return; // design mode bypass            
            settingsService = Locator.Current.GetRequiredService<ISettingsService>();
            navService = Locator.Current.GetRequiredService<NavService>();

            DisplaySettings = settingsService.GetAppSettings().Clone();
        }
        public void OkCommand()
        {
            settingsService.SetAppSettings(DisplaySettings.Clone());
            settingsService.ApplyAll();
            settingsService.SaveAll();

            navService.GoTo(typeof(DriveListViewModel));
            //(VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
        public void CancelCommand()
        {
            navService.GoTo(typeof(DriveListViewModel));
            //(VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;


        }
    }
}
