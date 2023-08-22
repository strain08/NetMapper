using NetDriveManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetDriveManager.Services
{
    public class Database
    {
        public List<NDModel> GetDrives => _drivesDb;

        private List<NDModel> _drivesDb = new();

        // CTOR
        public Database()
        {   
            if (ReadFromFile())
            {
                
            }
            else
            {
                WriteToFile();
            }
        }

       

        public bool ReadFromFile()
        {
            // TODO deserialize _drivesDb from json file
            _drivesDb.Add(new NDModel() { DriveLetter = "X:", Provider = "\\\\XOXO\\mir1" });
            _drivesDb.Add(new NDModel() { DriveLetter = "Y:", Provider = "\\\\XOXO\\mir2" });

            return true;
        }
        public bool WriteToFile()
        {
            // TODO serialize _drivesDb to json file
            return true;
        }



    }
}
