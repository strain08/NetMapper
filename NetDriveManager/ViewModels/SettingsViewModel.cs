using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.ViewModels
{
    public partial class SettingsViewModel:ViewModelBase
    {
        [ObservableProperty]
        AppSettingsModel settings;
        
        readonly IStore<AppSettingsModel> store;

        public SettingsViewModel()
        {
            store = Locator.Current.GetRequiredService<IStore<AppSettingsModel>>();
            settings = store.GetAll();
        }
        public void OkCommand()
        {
            store.Update(Settings);
            (VMServices.MainWindowViewModel??=new()).Content = VMServices.DriveListViewModel;
        }
        public void CancelCommand()
        {
            (VMServices.MainWindowViewModel??=new()).Content = VMServices.DriveListViewModel;
        }
    }
}
