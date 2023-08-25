using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using NetDriveManager.Services;


namespace NetDriveManager.Models
{

    public static class NDManager
    {
        
        public static ObservableCollection<NDModel> NetDriveList { get; set; } = new ();

        public static void AddDrive(NDModel model)
        {
            NetDriveList.Add(model);
            
            Database.UpdateDb(new List<NDModel>(NetDriveList));
        }

        public static void RemoveDrive(NDModel model) 
        {
            var i = NetDriveList.IndexOf(model);
            NetDriveList.RemoveAt(i);
            
            Database.UpdateDb(new List<NDModel>(NetDriveList));
        }

        public static void Clear() 
        { 
            NetDriveList.Clear();         
        }

        public static void EditDrive(NDModel oldModel, NDModel newModel) 
        {
            var i = NetDriveList.IndexOf(oldModel);
            NetDriveList.RemoveAt(i);
            NetDriveList.Insert(i, newModel);

            Database.UpdateDb(new List<NDModel>(NetDriveList));
        }
        
    }
}

