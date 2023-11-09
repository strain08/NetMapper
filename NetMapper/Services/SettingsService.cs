using System;
using System.Collections.Generic;
using NetMapper.Interfaces;
using NetMapper.Models;

namespace NetMapper.Services;

public class SettingsService : ISettingsService
{
    private readonly Dictionary<Type, ISettingModule> SettingsDictionary = new();
    private readonly IDataStore<AppSettingsModel> SettingsStore;
    private AppSettingsModel appSettings;

    //CTOR
    public SettingsService(IDataStore<AppSettingsModel> store)
    {
        SettingsStore = store;
        appSettings = store.GetData();
    }

    public AppSettingsModel AppSettings
    {
        get => appSettings;
        set
        {
            appSettings = value;
            foreach (var setting in SettingsDictionary)
                setting.Value.SetAppSettings(appSettings);
        }
    }

    public void AddModule(ISettingModule settingModule)
    {
        try
        {
            SettingsDictionary.Add(settingModule.GetType(), settingModule);
            settingModule.SetAppSettings(appSettings);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException($"{ToString} : Duplicate setting: {settingModule.GetType()}");
        }
    }

    public ISettingModule GetModule(Type settingType)
    {
        if (SettingsDictionary.TryGetValue(settingType, out var settingModule))
            return settingModule;
        throw new InvalidOperationException($"{ToString} : Can not find {settingType} in SettingsList.");
    }

    public void ApplyAll()
    {
        foreach (var setting in SettingsDictionary) setting.Value.Apply();
    }

    public void SaveAll()
    {
        SettingsStore.Update(AppSettings);
    }
}