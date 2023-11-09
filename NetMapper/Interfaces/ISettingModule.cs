using NetMapper.Models;

namespace NetMapper.Interfaces;

public interface ISettingModule
{
    AppSettingsModel GetAppSettings();
    void SetAppSettings(AppSettingsModel value);
    public void Apply();
}