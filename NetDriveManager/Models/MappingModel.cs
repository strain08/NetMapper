using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Enums;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NetDriveManager.Models
{
    public delegate void DisconnectCommand(MappingModel mapping);
    public delegate void ConnectCommand(MappingModel mapping);

    public partial class MappingModel : ObservableObject
    {
        // CTOR
        public DisconnectCommand? OnDisconnectCommand { get; set; }
        public DisconnectCommand? OnConnectCommand { get; set; }

        public MappingModel(MappingModel copyModel)
        {
            NetworkPath = copyModel.NetworkPath;
            DriveLetter = copyModel.DriveLetter;
        }
        public MappingModel() { }

        // PUBLIC PROP
        public string DriveLetter { get; set; } = string.Empty;

        public string NetworkPath { get; set; } = string.Empty;

        [JsonIgnore]
        public string Hostname
        {
            get
            {
                if (string.IsNullOrEmpty(NetworkPath))
                {
                    return string.Empty;
                }
                else
                {
                    // Return SERVER from \\SERVER\share\etc
                    string pattern = @"\\\\(.*?)\\";
                    Match m = Regex.Match(NetworkPath, pattern);
                    if (m.Success)
                    {
                        return m.Groups[1].Value;
                    }
                }
                return string.Empty;
            }
        }


        public void DisconnectCommand()
        {

            if (OnDisconnectCommand != null)
            {
                OnDisconnectCommand(this);
            }
        }
        public void ConnectCommand()
        {
            if (OnConnectCommand != null)
            {
                OnConnectCommand(this);
            }
        }

        [ObservableProperty]
        bool connectCommandVisible;

        [ObservableProperty]
        bool disconnectCommandVisible;

        [ObservableProperty]
        string connectionStatusMessage = "Status changing...";

        [ObservableProperty]
        string statusColor;

        const string COLOR_OK = "Green";
        const string COLOR_ERROR = "Red";
        const string COLOR_WARNING = "Yellow";

        private ConnectionState _connectionState;

        public ConnectionState ConnectionState
        {
            get
            {
                return _connectionState;
            }
            set
            {
                _connectionState = value;
                switch (value)
                {
                    case ConnectionState.Connected:
                        ConnectCommandVisible = false;
                        DisconnectCommandVisible = true;
                        StatusColor = COLOR_OK;
                        break;
                    case ConnectionState.Disconnected:
                        ConnectCommandVisible = true;
                        DisconnectCommandVisible = false;
                        StatusColor = COLOR_ERROR;
                        break;
                    case ConnectionState.Degraded:
                        ConnectCommandVisible = false;
                        DisconnectCommandVisible = false;
                        StatusColor = COLOR_WARNING;
                        break;

                }
                ConnectionStatusMessage = value.ToString();
            }
        }
        //string? name;
        //string? caption;
        //string? volumeName;
        //string? deviceID;
        //string? fileSystem;
        //string? freeSpace;
        //string? size;
        //string? volumeSerialNumber;

    }
}
