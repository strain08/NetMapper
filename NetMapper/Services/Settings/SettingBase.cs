using NetMapper.Interfaces;
using NetMapper.Models;
using System;

namespace NetMapper.Services.Settings;

public abstract class SettingBase : ISettingModule
{
    private AppSettingsModel?
        appSettings;

    public AppSettingsModel
        GetAppSettings()
    {
        return appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public void
        SetAppSettings(AppSettingsModel value)
    {
        appSettings = value;
    }

    public abstract void Apply();
}