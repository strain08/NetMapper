using System.Runtime.InteropServices;

namespace NetworkDrive
{
    public class NetworkDrive
    {   
        
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2
            (ref NETRESOURCE oNetworkResource, string? sPassword,
            string? sUserName, int iFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2
            (string sLocalName, uint iFlags, int iForce);

        public static int MapNetworkDrive(string sDriveLetter, string sNetworkPath)
        {
            //Checks if the last character is \ as this causes error on mapping a drive.
            if (sNetworkPath.Substring(sNetworkPath.Length - 1, 1) == @"\")
            {
                sNetworkPath = sNetworkPath.Substring(0, sNetworkPath.Length - 1);
            }

            NETRESOURCE oNetworkResource = new NETRESOURCE()
            {
                oResourceType = ResourceType.RESOURCETYPE_DISK,
                sLocalName = sDriveLetter + ":",
                sRemoteName = sNetworkPath
            };

            //If Drive is already mapped disconnect the current 
            //mapping before adding the new mapping
            //if (IsDriveMapped(sDriveLetter))
            //{
            //    DisconnectNetworkDrive(sDriveLetter, true);
            //}

            return WNetAddConnection2(ref oNetworkResource, null, null, 0);
        }

        /// <summary>
        /// Disconnects a Network Drive
        /// </summary>
        /// <param name="sDriveLetter"></param>
        /// <param name="bForceDisconnect"></param>
        /// <returns>
        /// 0 = Success ; 2250  = Error        
        /// </returns>
        public static CancelConnection DisconnectNetworkDrive(string sDriveLetter, bool bForceDisconnect)
        {
            if (bForceDisconnect)
            {
                return (CancelConnection) WNetCancelConnection2(sDriveLetter + ":", 0, 1);
            }
            else
            {
                return (CancelConnection) WNetCancelConnection2(sDriveLetter + ":", 0, 0);
            }
        }

        public static bool IsDriveMapped(string sDriveLetter)
        {
            string[] DriveList = Environment.GetLogicalDrives();
            for (int i = 0; i < DriveList.Length; i++)
            {
                if (sDriveLetter  + ":\\" == DriveList[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDriveLetter"></param>
        /// <returns></returns>
        public static bool IsNetworkDrive(string sDriveLetter)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Network && d.Name==sDriveLetter)
                {                    
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DriveInfo> GetNetworkDrives()
        {
            List<DriveInfo> ndList = new List<DriveInfo> { };
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Network)
                {
                    ndList.Add(d);
                }
            }
            return ndList;
        }

    }

}
