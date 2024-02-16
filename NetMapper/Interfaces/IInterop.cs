using NetMapper.Enums;
using NetMapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetMapper.Interfaces
{
    public interface IInterop
    {
        GetConnectionStatus GetUncName(string driveLetter, out string uncName);
        ConnectResult ConnectNetworkDrive(char cDriveLetter, string sNetworkPath, string? sUser = null, string? sPassword = null);
        Task<ConnectResult> ConnectNetworkDriveAsync(char cDriveLetter, string sNetworkPath);
        DisconnectResult DisconnectNetworkDrive(char cDriveLetter, bool bForceDisconnect = false);
        Task<DisconnectResult> DisconnectNetworkDriveAsync(char cDriveLetter, bool bForceDisconnect = false);
        string GetActualPathForLetter(char letter);
        List<char> GetAvailableDriveLetters();
        List<MapModel> GetMappedDrives();
        string GetVolumeLabel(MapModel m);
        bool IsNetworkDriveMapped(char cDriveLetter);
        bool IsNetworkPath(string path);
        bool IsRegularDriveMapped(char cDriveLetter);
        void OpenFolderInExplorer(string path);
    }
}