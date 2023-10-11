using NetMapper.Services.Helpers;
using NetMapper.Services.Stores;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace NetMapper.Services
{
    public class JsonStore<T> : IStore<T> where T : new()
    {
        readonly string jsonFile;

        private T? StoreData { get; set; }

        // CTOR

        public JsonStore(string jsonFileName)
        {             
            var strWorkPath = AppStartupFolder.GetStartupFolder();
            jsonFile = Path.Combine(strWorkPath, jsonFileName);
        }      


        // READ
        public bool Load()
        {
            try
            {
                var jsonString = File.ReadAllText(jsonFile);
                StoreData = JsonSerializer.Deserialize<T>(jsonString) ?? throw new JsonException($"Invalid json file: {jsonFile}");
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
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var jsonString = JsonSerializer.Serialize(StoreData ?? new(), typeof(T), jsonOptions);
                File.WriteAllText(jsonFile, jsonString);
                return true;
            }
            catch
            {
                Log.Error($"Unable to save json file: {jsonFile}");
                return false;
            }
        }

        // UPDATE
        public bool Update(T updatedData)
        {

            StoreData = updatedData;
            return Save();
        }

        public T GetAll()
        {
            if (StoreData != null) return StoreData;            
            Load();
            return StoreData ?? new();
        }
    }

}
