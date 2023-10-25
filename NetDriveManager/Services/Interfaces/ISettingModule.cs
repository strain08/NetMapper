using NetMapper.Models;

namespace NetMapper.Services.Interfaces
{
    public interface ISettingModule
    {
        AppSettingsModel GetAppSettings();
        void SetAppSettings(AppSettingsModel value);
        public void Apply();
    }
}