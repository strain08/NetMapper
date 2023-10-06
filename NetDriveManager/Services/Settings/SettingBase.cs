using NetMapper.Models;
using System;

namespace NetMapper.Services.Settings
{
    internal abstract class SettingBase : ISetting
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
