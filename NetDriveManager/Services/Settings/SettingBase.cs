using NetMapper.Models;
using NetMapper.Services.Interfaces;
using System;

namespace NetMapper.Services.Settings
{
    public abstract class SettingBase : ISettingModule
    {
        public AppSettingsModel 
            GetAppSettings() => appSettings ?? throw new ArgumentNullException();

        public void 
            SetAppSettings(AppSettingsModel value) => appSettings = value;

        private AppSettingsModel? 
            appSettings;

        public abstract void Apply();

    }
}
