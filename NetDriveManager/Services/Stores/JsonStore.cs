using NetMapper.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace NetMapper.Services
{
    public class JsonStore<T> : IStore<T> where T : new()
    {
        readonly string jsonSettingsFile;

        private T ItemsList { get; set; } = new();

        // CTOR

        public JsonStore(string jsonFile)
        {
            string strExeFilePath = Process.GetCurrentProcess()?.MainModule?.FileName
                ?? throw new ApplicationException("Process.GetCurrentProcess()?.MainModule?.FileName null");

            string strWorkPath = Path.GetDirectoryName(strExeFilePath)
                ?? throw new ApplicationException("Path.GetDirectoryName(strExeFilePath) null");

            jsonSettingsFile = Path.Combine(strWorkPath, jsonFile);
        }      


        // READ
        public bool Load()
        {
            try
            {
                var jsonString = File.ReadAllText(jsonSettingsFile);
                ItemsList = JsonSerializer.Deserialize<T>(jsonString) ?? throw new JsonException();
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
                var jsonString = JsonSerializer.Serialize(ItemsList, ItemsList.GetType());
                File.WriteAllText(jsonSettingsFile, jsonString);
                return true;
            }
            catch
            {
                return false;

            }
        }

        // UPDATE
        public bool Update(T updatedList)
        {

            ItemsList = updatedList;
            return Save();
        }

        public T GetAll()
        {
            Load();
            return ItemsList;
        }
    }

}
