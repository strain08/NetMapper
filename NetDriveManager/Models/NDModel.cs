using CommunityToolkit.Mvvm.ComponentModel;

using System.Text.RegularExpressions;

namespace NetDriveManager.Models
{
    public partial class NDModel : ObservableObject
    {
        public NDModel(NDModel copyModel)
        {
            Provider = copyModel.Provider;
            DriveLetter = copyModel.DriveLetter;
        }
        public NDModel() { }
        
        [ObservableProperty]
        string? driveLetter;

        public string Provider { get; set; }
        
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

        //[ObservableProperty]
        //string? provider;

        //[ObservableProperty]
        //string? name;

        //[ObservableProperty]
        //string? caption;

        //[ObservableProperty]
        //string? volumeName;

        //[ObservableProperty]
        //string? deviceID;

        //[ObservableProperty]
        //string? fileSystem;

        //[ObservableProperty]
        //string? freeSpace;

        //[ObservableProperty]
        //string? size;

        //[ObservableProperty]
        //string? volumeSerialNumber;
        



    }
}
