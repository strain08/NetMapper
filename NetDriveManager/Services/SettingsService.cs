using Avalonia;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Settings;
using NetMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    internal class SettingsService
    {
        public IStore<AppSettingsModel> SettingsStore;

        public AppSettingsModel Settings;

        public List<ISetting> SettingsList = new();


        public SettingsService(IStore<AppSettingsModel> store)
        {            
            SettingsStore = store;
            Settings = store.GetAll();
            SettingsList.Add(new RunAtStartup(Settings));
            SettingsList.Add(new MinimizeTaskbar(Settings));
            SettingsList.Add(new SetupWindow(Settings));

            //var a= SettingsList.Find((s)=>s.GetType()==typeof(RunAtStartup));
            //SettingsList.GetSetting(typeof(RunAtStartup));
        }
        public void ApplyAll()
        {
            foreach (var setting in SettingsList)
            {
                setting.Apply();
            }
        }
        public void SaveAll()
        {
            SettingsStore.Update(Settings);
        }
    }
}
