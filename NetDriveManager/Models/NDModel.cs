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
            Provider = copyModel.Provider;
            DriveLetter = copyModel.DriveLetter;
        }
        public NDModel() { }
        
        // PUBLIC PROP
        public string? DriveLetter { get; set; }

        public string? Provider { get; set; }
        
        [JsonIgnore]
        public string? Hostname
        {
            get
            {
                if (string.IsNullOrEmpty(Provider))
                {
                    return null;
                }
                else
                {
                    // Return SERVER from \\SERVER\share\etc
                    string pattern = @"\\\\(.*?)\\";
                    Match m = Regex.Match(Provider, pattern);
                    if (m.Success)
                    {
                        return m.Groups[1].Value;
                    }
                }
                return null;
            }
        }
        
        [JsonIgnore]
        public string ConnectionState { get; set; } = "Default Connection State" ;

        [JsonIgnore]
        public string ConnectionColor { get; set; } = "Red";
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
