using System;
using NetMapper.Models;
using NetMapper.Services.Interfaces;

namespace NetMapper.Services;

public interface ISettingsService
{
    void AddModule(ISettingModule settingModule);
    void ApplyAll();
    AppSettingsModel GetAppSettings();
    ISettingModule GetModule(Type settingType);
    void SaveAll();
    void SetAppSettings(AppSettingsModel value);
}