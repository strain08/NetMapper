using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace NetDriveManager.Models
{
    public static class NDManager
    {
        public static ObservableCollection<NDModel> NetDriveList { get; set; } = new ();

        public static void AddDrive(NDModel model)
        {
            NetDriveList.Add(model);
        }

        public static void RemoveDrive(NDModel model) 
        {
            var i = NetDriveList.IndexOf(model);
            NetDriveList.RemoveAt(i); 
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
        }
        
        public static void GetMappedDrives()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_LogicalDisk WHERE DriveType={(int)DriveType.Network}");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    var nd = new NDModel();
                    nd.DriveLetter = (string)queryObj["Caption"];                    
                    nd.Provider = (string)queryObj["ProviderName"];
                    AddDrive(nd);
                    //nd.DeviceID = (string)queryObj["DeviceID"];
                    //Debug.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
                    //Debug.WriteLine("DriveType: {0}", queryObj["DriveType"]);
                    //Debug.WriteLine("FileSystem: {0}", queryObj["FileSystem"]);
                    //Debug.WriteLine("FreeSpace: {0}", queryObj["FreeSpace"]);
                    //Debug.WriteLine("MediaType: {0}", queryObj["MediaType"]);
                    //Debug.WriteLine("Name: {0}", queryObj["Name"]);
                    //Console.WriteLine("ProviderName: {0}", queryObj["ProviderName"]);
                    //Console.WriteLine("Size: {0}", queryObj["Size"]);
                    //Console.WriteLine("SystemName: {0}", queryObj["SystemName"]);
                    //Console.WriteLine("VolumeName: {0}", queryObj["VolumeName"]);
                    //Console.WriteLine("VolumeSerialNumber: {0}", queryObj["VolumeSerialNumber"]);
                }
            }
            catch (ManagementException e)
            {
                Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        

    }
}

