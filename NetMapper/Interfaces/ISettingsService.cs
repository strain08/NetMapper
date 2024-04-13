using NetMapper.Models;
using System;

namespace NetMapper.Interfaces;

public interface ISettingsService
{
    AppSettingsModel AppSettings { get; set; }
    void AddModule(ISettingModule settingModule);
    ISettingModule GetModule(Type settingType);
    void ApplyAll();
    void SaveAll();
}