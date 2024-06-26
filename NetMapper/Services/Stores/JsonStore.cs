﻿using NetMapper.Interfaces;
using NetMapper.Services.Static;
using Serilog;
using System;
using System.IO;
using System.Text.Json;

namespace NetMapper.Services;

public class JsonStore<T> : IDataStore<T> where T : new()
{
    private readonly string jsonFile;
    private T? StoreData { get; set; }

    // CTOR
    public JsonStore(string jsonFileName)
    {
        var strWorkPath = AppUtil.GetStartupFolder();
        jsonFile = Path.Combine(strWorkPath, jsonFileName);
    }

    public T GetData()
    {
        if (StoreData != null) return StoreData;
        Load();
        return StoreData ?? throw new ArgumentNullException();
    }

    public void Update(T updatedData)
    {
        StoreData = updatedData;
        Save();
    }

    private void Load()
    {
        // file does not exist, try create new one
        if (!File.Exists(jsonFile))
        {
            Log.Information($"Json file {jsonFile} does not exist. Will try create new file.");
            Save();
            return;
        }

        // file exists, try read
        var jsonString = File.ReadAllText(jsonFile);

        try
        {
            StoreData = JsonSerializer.Deserialize<T>(jsonString);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error deserializing {jsonFile}. Will try create new file.");
            Save();
        }
    }

    private void Save()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        StoreData ??= new T(); // initialize if null
        try
        {
            var jsonString = JsonSerializer.Serialize(StoreData, StoreData.GetType(), jsonOptions);
            File.WriteAllText(jsonFile, jsonString);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"Unable to write json file: {jsonFile}");
            throw;
        }
    }
}