using NetMapper.Models;
using NetMapper.Extensions;

using NetMapper.Services.Settings;
using System;
using System.Collections.Generic;
using NetMapper.Services.Stores;

namespace NetMapper.Services
{
    internal class SettingsService
    {
        public IStore<AppSettingsModel> SettingsStore;

        public AppSettingsModel AppSettings;

        public List<ISetting> SettingsList = new(); //SettingsList.GetSetting(typeof(RunAtStartup));

        //CTOR
        public SettingsService(IStore<AppSettingsModel> store)
        {
            SettingsStore = store;
            AppSettings = store.GetAll();

        }

        public void Add(ISetting setting)
        {
            if (SettingsList._GetSetting(setting.GetType()) == null)
            {
                setting.SetAppSettings(AppSettings);
                SettingsList.Add(setting);
            }
            else throw new InvalidOperationException($"{ToString} : Duplicate setting: {setting.GetType()}");
        }

        public ISetting Get(Type settingType)
        {            
            return SettingsList.Find((s) => s.GetType() == settingType) ?? 
                throw new InvalidOperationException($"{ToString} : Can not find {settingType} in SettingsList.");
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
            SettingsStore.Update(AppSettings);
        }
    }
}
