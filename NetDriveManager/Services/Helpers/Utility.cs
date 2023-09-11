using NetDriveManager.Enums;
using NetDriveManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace NetDriveManager.Services.Helpers
{
    public static class Utility
    {

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2
            (ref NETRESOURCE oNetworkResource, string? sPassword,
            string? sUserName, int iFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2
            (string sLocalName, uint iFlags, int iForce);

        public static int
            MapNetworkDrive(string sDriveLetter, string sNetworkPath)
        {
            //Checks if the last character is \ as this causes error on mapping a drive.
            if (sNetworkPath.Substring(sNetworkPath.Length - 1, 1) == @"\")
            {
                // sNetworkPath.Substring(0, sNetworkPath.Length - 1);
                sNetworkPath = sNetworkPath[..^1];
            }

            NETRESOURCE oNetworkResource = new NETRESOURCE()
            {
                oResourceType = ResourceType.RESOURCETYPE_DISK,
                sLocalName = sDriveLetter + ":",
                sRemoteName = sNetworkPath
            };
            return WNetAddConnection2(ref oNetworkResource, null, null, 0);
        }

        public static CancelConnection
            DisconnectNetworkDrive(string sDriveLetter, bool bForceDisconnect)
        {
            if (bForceDisconnect)
            {
                return (CancelConnection)WNetCancelConnection2(sDriveLetter + ":", 0, 1);
            }
            else
            {
                return (CancelConnection)WNetCancelConnection2(sDriveLetter + ":", 0, 0);
            }
        }

        public static bool
            IsDriveMapped(string sDriveLetter)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.DriveType == DriveType.Network && drive.Name[0] == sDriveLetter[0]) 
                { 
                    return true; 
                }
            }
            return false;
        }

        public static bool
            IsNetworkDrive(string sDriveLetter)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Network && d.Name == sDriveLetter)
                {
                    return true;
                }
            }
            return false;
        }

        public static void
            IsMachineOnline(string hostName)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;


            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {
                PingReply reply = pingSender.Send(hostName, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    Debug.WriteLine(hostName + " is online.");
                }
                else
                {
                    Debug.WriteLine(hostName + " is offline.");
                }
            }
            catch
            {
                Debug.WriteLine(hostName + " is offline.");
            }

        }

        public static List<char>
            GetAvailableDriveLetters()
        {
            const int lower = 'A';
            const int upper = 'Z';

            List<char> availableLetters = new List<char>();
            var letters = Directory.GetLogicalDrives().Select(l => l.Substring(0, 1).ToUpperInvariant()[0]);
            for (int i = lower; i < upper; i++)
            {
                char letter = (char)i;
                if (!letters.Contains(letter))
                {
                    availableLetters.Add(letter);
                }
            }

            return availableLetters;
        }

        public static string GetPathForLetter(string letter)
        {
            var mappedList = GetMappedDrives();
            foreach (MappingModel item in mappedList)
            {
                if (item.DriveLetter[0] == letter[0]) return item.NetworkPath;
            }
            return string.Empty;
        }

        public static List<MappingModel>
            GetMappedDrives()
        {
            var mappedDrives = new List<MappingModel>();
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_LogicalDisk WHERE DriveType={(int)DriveType.Network}");

                foreach (ManagementObject queryObj in searcher.Get().Cast<ManagementObject>())
                {
                    var nd = new MappingModel();
                    nd.DriveLetter = (string)queryObj["Caption"];
                    nd.NetworkPath = (string)queryObj["ProviderName"];
                    mappedDrives.Add(nd);

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
                return mappedDrives;
            }
            catch (ManagementException e)
            {
                Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                return mappedDrives;
            }
        }

    }

}
