using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace NetDriveManager.Services
{
    public class JsonStore : IStorage
    {
         

        private string? jsonSettingsFile;
        
        private List<MappingModel> DrivesDb { get; set; } = new();
        
        // CTOR
        
        public JsonStore(string jsonFile)
        {
            jsonSettingsFile = jsonFile;
        }

        

        // READ
        public bool Load()
        {
            try
            {
                var jsonString = File.ReadAllText(jsonSettingsFile);
                DrivesDb = JsonSerializer.Deserialize<List<MappingModel>>(jsonString);
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
