using System;
using NetMapper.Models;
using NetMapper.Services.Interfaces;

namespace NetMapper.Services.Settings;

public abstract class SettingBase : ISettingModule
{
    private AppSettingsModel?
        appSettings;

    public AppSettingsModel
        GetAppSettings()
    {
        return appSettings ?? throw new ArgumentNullException();
    }

    public void
        SetAppSettings(AppSettingsModel value)
    {
        appSettings = value;
    }

    public abstract void Apply();
}