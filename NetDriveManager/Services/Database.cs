using NetDriveManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace NetDriveManager.Services
{
    public class Database
    {
        public static List<NDModel> DrivesDb { get; set; } = new List<NDModel>();

        public static string jsonSettingsFile {get; set;} = string.Empty;

        // READ
        public static bool ReadFromFile()
        {
            try
            {
                var jsonString = File.ReadAllText(jsonSettingsFile);
                DrivesDb = JsonSerializer.Deserialize<List<NDModel>>(jsonString);
            }
            catch
            {
                WriteToFile();
            }
            return true;

        }
        // WRITE
        public static bool WriteToFile()
        {

            var jsonString = JsonSerializer.Serialize(DrivesDb, DrivesDb.GetType());

            File.WriteAllText(jsonSettingsFile, jsonString);



            return true;
        }
        // UPDATE
        public static void UpdateDb(List<NDModel> updatedList)
        {
            
            DrivesDb=new (updatedList);
            WriteToFile();
        }

    }
}
