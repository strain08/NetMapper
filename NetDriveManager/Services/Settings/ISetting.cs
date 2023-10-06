using NetMapper.Models;

namespace NetMapper.Services.Settings
{
    internal interface ISetting
    {
        AppSettingsModel GetAppSettings();
        void SetAppSettings(AppSettingsModel value);
        public void Apply();
    }
}