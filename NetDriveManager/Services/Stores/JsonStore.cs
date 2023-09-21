using NetMapper.Interfaces;
using NetMapper.Models;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;
using Windows.Media.Capture;
using Avalonia;
using System.Diagnostics;

namespace NetMapper.Services
{
    public class JsonStore : IStorage
    {        
        private readonly string jsonSettingsFile;
        
        private List<MappingModel> DrivesDb { get; set; } = new();
        
        // CTOR
        
        public JsonStore(string jsonFile)
        {
            string strExeFilePath = Process.GetCurrentProcess()?.MainModule?.FileName 
                ?? throw new ApplicationException("Process.GetCurrentProcess()?.MainModule?.FileName null");

            string strWorkPath = Path.GetDirectoryName(strExeFilePath)
                ?? throw new ApplicationException("Path.GetDirectoryName(strExeFilePath) null");

            jsonSettingsFile =Path.Combine(strWorkPath, jsonFile);
        }



        // READ
        public bool Load()
        {
            try
            {
                var jsonString = File.ReadAllText(jsonSettingsFile);
                DrivesDb = JsonSerializer.Deserialize<List<MappingModel>>(jsonString) ?? throw new JsonException();
                return true;
            }
            catch
            {
                return Save();
            }
        }

        // WRITE
        public bool Save()
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(DrivesDb, DrivesDb.GetType());
                File.WriteAllText(jsonSettingsFile, jsonString);
                return true;
            }
            catch
            {                
                return false;
                
            } 
            

        }

        // UPDATE
        public bool Update(List<MappingModel> updatedList)
        {

            DrivesDb = new(updatedList);
            return Save();
        }

        List<MappingModel> IStorage.GetAll()
        {
            Load();
            return DrivesDb;
        }
    }

}
