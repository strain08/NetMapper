using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using Splat;
using System;

namespace NetMapper.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        AppSettingsModel displaySettings;

        readonly IStore<AppSettingsModel> store;

        public SettingsViewModel()
        {
            store = Locator.Current.GetRequiredService<IStore<AppSettingsModel>>();
            DisplaySettings = StaticSettings.Settings!.Clone() ?? new AppSettingsModel();
            
        }
        public void OkCommand()
        {
            StaticSettings.Settings = DisplaySettings.Clone();
            // >> place to apply settings
            var r = new RunAtStartup(StaticSettings.Settings);

            r.Apply();

            // <<
            store.Update(StaticSettings.Settings);
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
        public static void CancelCommand()
        {
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
    }
}
