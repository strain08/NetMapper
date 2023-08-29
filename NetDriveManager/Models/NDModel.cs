using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NetDriveManager.Models
{
    public partial class NDModel : ObservableObject
    {
        // CTOR

        public NDModel(NDModel copyModel)
        {
            NetworkPath = copyModel.NetworkPath;
            DriveLetter = copyModel.DriveLetter;
        }
        public NDModel() { }
        
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
        
        [JsonIgnore]
        [ObservableProperty]
        string connectionState = "Default Connection State" ;

        [JsonIgnore]
        [ObservableProperty]
        string connectionColor = "White";
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
