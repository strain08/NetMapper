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
        AppSettingsModel settings;

        readonly IStore<AppSettingsModel> store;

        public SettingsViewModel()
        {
            store = Locator.Current.GetRequiredService<IStore<AppSettingsModel>>();
            Settings = StaticSettings.Settings ?? throw new ArgumentNullException("Settings null in View");
            
        }
        public void OkCommand()
        {
            // >> place to apply settings
            var r = new RunAtStartup(Settings);
            r.Apply();

            // <<
            store.Update(Settings);
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
        public void CancelCommand()
        {
            (VMServices.MainWindowViewModel ??= new()).Content = VMServices.DriveListViewModel;
        }
    }
}
