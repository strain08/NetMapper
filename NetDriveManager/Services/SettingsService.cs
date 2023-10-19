using NetMapper.Extensions;
using NetMapper.Models;
using NetMapper.Services.Settings;
using NetMapper.Services.Stores;
using System;
using System.Collections.Generic;

namespace NetMapper.Services
{
    internal class SettingsService
    {
        private IStore<AppSettingsModel> SettingsStore;

        private readonly List<ISetting> SettingsList = new(); //SettingsList.GetSetting(typeof(RunAtStartup));

        public AppSettingsModel AppSettings
        {
            get => appSettings;
            set
            {
                appSettings = value;
                foreach (var setting in SettingsList)
                    setting.SetAppSettings(value);
            }
        }
        private AppSettingsModel appSettings;

        //CTOR
        public SettingsService(IStore<AppSettingsModel> store)
        {
            SettingsStore = store;
            appSettings = store.GetAll();

        }

        public void AddSetting(ISetting setting)
        {
            if (SettingsList._GetSetting(setting.GetType()) == null)
            {
                setting.SetAppSettings(AppSettings);
                SettingsList.Add(setting);
            }
            else throw new InvalidOperationException($"{ToString} : Duplicate setting: {setting.GetType()}");
        }

        public ISetting GetSetting(Type settingType)
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
