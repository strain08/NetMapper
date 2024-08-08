using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetMapper.Services.Helpers;
public class Interop : IInterop
{
    public Interop() { }

    public GetConnectionStatus GetUncName(string driveLetter, out string uncName)
    {
        int length = 255;
        GetConnectionStatus status = GetConnectionStatus.MoreData;
        uncName = String.Empty;

        //var driveLetter = configuration.DriveLetter.Value.ToDriveLetterString();
        //just make a loop in order to requery if the buffer was too small
        while (status == GetConnectionStatus.MoreData)
        {
            var sb = new StringBuilder(length);
            status = (GetConnectionStatus)ComImports.WNetGetConnection(driveLetter, sb, ref length);
            uncName = sb.ToString();
        }

        return status;
    }
    public ConnectResult ConnectNetworkDrive(
        char cDriveLetter,
        string sNetworkPath,
        string? sUser = null,
        string? sPassword = null)
    {
        //Checks if the last character is \ as this causes error on mapping a drive.
        if (sNetworkPath[^1..] == @"\")
            sNetworkPath = sNetworkPath[..^1];

        NETRESOURCE oNetworkResource = new()
        {
            oResourceType = ResourceType.RESOURCETYPE_DISK,
            sLocalName = cDriveLetter + ":",
            sRemoteName = sNetworkPath
        };
        return (ConnectResult)ComImports.WNetAddConnection2(ref oNetworkResource, sPassword, sUser, 0);
    }

    public DisconnectResult DisconnectNetworkDrive(
        char cDriveLetter,
        bool bForceDisconnect = false)
    {
        if (bForceDisconnect)
            return (DisconnectResult)ComImports.WNetCancelConnection2(cDriveLetter + ":", 0, 1);
        return (DisconnectResult)ComImports.WNetCancelConnection2(cDriveLetter + ":", 0, 0);
    }

    public async Task<DisconnectResult> DisconnectNetworkDriveAsync(char cDriveLetter,
        bool bForceDisconnect = false)
    {
        return await Task.Run(() => DisconnectNetworkDrive(cDriveLetter, bForceDisconnect));
    }

    public async Task<ConnectResult> ConnectNetworkDriveAsync(char cDriveLetter, string sNetworkPath)
    {
        return await Task.Run(() => ConnectNetworkDrive(cDriveLetter, sNetworkPath));
    }

    public bool IsNetworkDriveMapped(char cDriveLetter)
    {
        try
        {
            if (new DriveInfo(cDriveLetter + @":\").DriveType == DriveType.Network) return true;
        }
        catch (ArgumentException)
        {
            return false;
        }

        return false;
    }

    public bool IsRegularDriveMapped(char cDriveLetter)
    {
        try
        {
            DriveInfo di = new(cDriveLetter + @":\");
            if (di.DriveType != DriveType.Network && di.IsReady) return true;
        }
        catch (ArgumentException)
        {
            return false;
        }

        return false;
    }

    public string GetVolumeLabel(MapModel m)
    {
        DriveInfo drive = new(m.DriveLetterColon);
        return drive.IsReady ? drive.VolumeLabel : string.Empty;
    }

    public List<char> GetAvailableDriveLetters()
    {
        var availableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        List<char> usedLetters = new(Directory.GetLogicalDrives().Select(l => l[..1].ToUpperInvariant()[0]));

        foreach (var c in usedLetters)
            availableLetters.Remove(c);

        return availableLetters;
    }

    /// <summary>
    ///     Get network path for network drive letter.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns>Empty string if letter not mapped to a network drive</returns>
    public string GetActualPathForLetter(char letter)
    {
        foreach (var item in GetMappedDrives())
            if (item.DriveLetter == letter)
                return item.NetworkPath;
        return string.Empty;
    }

    public List<MapModel> GetMappedDrives()
    {
        var mappedDrives = new List<MapModel>();
        try
        {
            var searcher = new ManagementObjectSearcher("root\\CIMV2",
                $"SELECT * FROM Win32_LogicalDisk WHERE DriveType={(int)DriveType.Network}");

            foreach (var queryObj in searcher.Get().Cast<ManagementObject>())
            {
                var caption = queryObj["Caption"] as string
                              ?? throw new ApplicationException("Wmi error getting mapped drive letter.");
                mappedDrives.Add(
                    new MapModel
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

    public bool IsNetworkPath(string path)
    {
        Uri testUri;
        bool isUnc;
        try
        {
            testUri = new Uri(path, UriKind.Absolute);
            isUnc = testUri.IsUnc;

        }
        catch
        {
            return false;
        }


        try
        {
            var pathSegment = testUri.Segments[1];
        }
        catch
        {
            return false;
        }

        if (isUnc)
            return true;
        else
            return false;
    }

    /*
    public bool IsNetworkPath(string path)
    {
        //// source: https://regexlib.com/REDetails.aspx?regexp_id=2285
        //var uncPattern =
        //    @"^((\\\\[a-zA - Z0 - 9 -]+\\[a-zA - Z0 - 9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([ ]+[^ \\/:*?""<>|]+)*)*\\?$";
        if (path.Length < 2) return false;
        var pathSlash = path.Replace("\\", "/");
        //var uncPattern = "^((\\/\\/[a-zA-Z0-9-]+\\/[a-zA-Z0-9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]))(\\/[^ \\/:*?\"\"<>|]+([ ]+[^ \\/:*?\"\"<>|]+)*)*\\/?$";
        var uncPattern = "^((\\/\\/[a-zA-Z0-9-.]+\\/[a-zA-Z0-9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]))(\\/[^ \\/:*?\"\"<>|]+([ ]+[^ \\/:*?\"\"<>|]+)*)*\\/?$";
        //var g = Regex.Match(pathSlash, uncPattern).Groups;
        //Debug.WriteLine(g.Count);
        //Debug.WriteLine(Regex.Match(pathSlash, uncPattern).Success);
        return Regex.Match(pathSlash, uncPattern).Success;


    }
    */
    public void OpenFolderInExplorer(string path)
    {
        ProcessStartInfo psi = new()
        {
            UseShellExecute = true,
            FileName = path + "\\"
        };
        Process.Start(psi);
    }
    //public static void IsMachineOnline(string hostName)
    //{
    //    using Ping pingSender = new();
    //    PingOptions options = new()
    //    {
    //        // Use the default Ttl value which is 128,
    //        // but change the fragmentation behavior.
    //        DontFragment = true
    //    };


    //    // Create a buffer of 32 bytes of data to be transmitted.
    //    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
    //    byte[] buffer = Encoding.ASCII.GetBytes(data);
    //    int timeout = 120;
    //    try
    //    {
    //        PingReply reply = pingSender.Send(hostName, timeout, buffer, options);

    //        if (reply.Status == IPStatus.Success)
    //        {
    //            Debug.WriteLine(hostName + " is online.");
    //        }
    //        else
    //        {
    //            Debug.WriteLine(hostName + " is offline.");
    //        }
    //    }
    //    catch
    //    {
    //        Debug.WriteLine(hostName + " is offline.");
    //    }

    //}
}