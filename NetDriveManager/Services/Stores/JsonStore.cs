using NetMapper.Interfaces;
using NetMapper.Services.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace NetMapper.Services
{
    public class JsonStore<T> : IStore<T> where T : new()
    {
        readonly string jsonFile;

        private T storeData { get; set; } = new();

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
                storeData = JsonSerializer.Deserialize<T>(jsonString) ?? throw new JsonException();
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
                var jsonString = JsonSerializer.Serialize(storeData, typeof(T), jsonOptions);
                File.WriteAllText(jsonFile, jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // UPDATE
        public bool Update(T updatedData)
        {

            storeData = updatedData;
            return Save();
        }

        public T GetAll()
        {
            Load();
            return storeData;
        }
    }

}
