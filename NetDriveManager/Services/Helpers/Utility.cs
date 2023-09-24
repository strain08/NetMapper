using NetMapper.Enums;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetMapper.Services.Helpers
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

        public static ConnectResult ConnectNetworkDrive(char cDriveLetter, string sNetworkPath)
        {

            //Checks if the last character is \ as this causes error on mapping a drive.
            if (sNetworkPath.Substring(sNetworkPath.Length - 1, 1) == @"\")
            {
                // sNetworkPath.Substring(0, sNetworkPath.Length - 1);
                sNetworkPath = sNetworkPath[..^1];
            }

            NETRESOURCE oNetworkResource = new()
            {
                oResourceType = ResourceType.RESOURCETYPE_DISK,
                sLocalName = cDriveLetter + ":",
                sRemoteName = sNetworkPath
            };
            return (ConnectResult)WNetAddConnection2(ref oNetworkResource, null, null, 0);
        }
        public static async Task<ConnectResult> ConnectNetworkDriveAsync(char cDriveLetter, string sNetworkPath) => 
            await Task.Run(() => ConnectNetworkDrive(cDriveLetter, sNetworkPath));

        public static CancelConnection DisconnectNetworkDrive(char cDriveLetter, bool bForceDisconnect = false)
        {
            if (bForceDisconnect)
            {
                return (CancelConnection)WNetCancelConnection2(cDriveLetter + ":", 0, 1);
            }
            else
            {
                return (CancelConnection)WNetCancelConnection2(cDriveLetter + ":", 0, 0);
            }
        }

        public static async Task<CancelConnection> DisconnectNetworkDriveAsync(char cDriveLetter, bool bForceDisconnect = false) =>
            await Task.Run(()=> DisconnectNetworkDrive(cDriveLetter, bForceDisconnect));

        public static bool IsNetworkDriveMapped(char cDriveLetter)
        {
            try
            {
                if (new DriveInfo(cDriveLetter + @":\").DriveType == DriveType.Network)
                {
                    return true;
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
            return false;
        }

        public static bool IsRegularDriveMapped(char cDriveLetter)
        {
            try
            {
                DriveInfo di = new(cDriveLetter + @":\");
                if (di.DriveType != DriveType.Network && di.IsReady)
                {                    
                    return true;
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
            return false;
        }
        public static void IsMachineOnline(string hostName)
        {
            using Ping pingSender = new();
            PingOptions options = new()
            {
                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                DontFragment = true
            };


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

        public static List<char> GetAvailableDriveLetters()
        {
            List<char> availableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
            List<char> usedLetters = new(Directory.GetLogicalDrives().Select(l => l[..1].ToUpperInvariant()[0]));

            foreach (char c in usedLetters) 
                availableLetters.Remove(c);

            return availableLetters;
        }
        /// <summary>
        /// Get network path for network drive letter.
        /// </summary>
        /// <param name="letter"></param>
        /// <returns>Empty string if letter not mapped to a network drive</returns>
        public static string GetPathForLetter(char letter)
        {
            var mappedList = GetMappedDrives();
            foreach (MappingModel item in mappedList)
            {
                if (item.DriveLetter == letter)
                    return item.NetworkPath;
            }
            return string.Empty;
        }

        public static List<MappingModel> GetMappedDrives()
        {
            var mappedDrives = new List<MappingModel>();
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_LogicalDisk WHERE DriveType={(int)DriveType.Network}");

                foreach (ManagementObject queryObj in searcher.Get().Cast<ManagementObject>())
                {
                    var caption = queryObj["Caption"] as String
                        ?? throw new ApplicationException("Wmi error getting mapped drive letter.");
                    mappedDrives.Add(
                        new MappingModel
                        {
                            DriveLetter = caption[0],
                            NetworkPath = (string)queryObj["ProviderName"]
                        });
                    #region Other possible properties
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
                    #endregion
                }
                return mappedDrives;
            }
            catch (ManagementException e)
            {
                Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                return mappedDrives;
            }
        }

        public static bool IsNetworkPath(string path)
        {
            // source: https://regexlib.com/REDetails.aspx?regexp_id=2285
            string uncPattern = @"^((\\\\[a-zA - Z0 - 9 -]+\\[a-zA - Z0 - 9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([ ]+[^ \\/:*?""<>|]+)*)*\\?$";

            return Regex.Match(path, uncPattern).Success;
        }
    }

}
