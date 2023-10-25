using NetMapper.Models;
using NetMapper.Services.Interfaces;
using NetMapper.Services.Stores;
using System;
using System.Collections.Generic;

namespace NetMapper.Services
{
    public class SettingsService : ISettingsService
    {
        private AppSettingsModel appSettings;

        private readonly IDataStore<AppSettingsModel> SettingsStore;

        private readonly List<ISettingModule> SettingsList; //SettingsList.GetSetting(typeof(RunAtStartup));        

        //CTOR
        public SettingsService(IDataStore<AppSettingsModel> store)
        {
            SettingsStore = store;
            appSettings = store.GetData();
            SettingsList = new();
        }

        public AppSettingsModel GetAppSettings()
        {
            return appSettings;
        }

        public void SetAppSettings(AppSettingsModel value)
        {
            appSettings = value;
            foreach (var setting in SettingsList)
                setting.SetAppSettings(value);
        }

        public void AddModule(ISettingModule settingModule)
        {
            // check if module already exists
            if (SettingsList.Find((sm) => sm.GetType() == settingModule.GetType()) != null)
                throw new InvalidOperationException($"{ToString} : Duplicate setting: {settingModule.GetType()}");

            settingModule.SetAppSettings(GetAppSettings());
            SettingsList.Add(settingModule);
        }

        public ISettingModule GetModule(Type settingType)
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
            SettingsStore.Update(GetAppSettings());
        }
    }
}
