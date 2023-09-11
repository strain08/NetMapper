using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Enums;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NetDriveManager.Models;

public partial class MappingModel : ObservableObject
{
    // CTOR


    public MappingModel(MappingModel copyModel)
    {
        NetworkPath = copyModel.NetworkPath;
        DriveLetter = copyModel.DriveLetter;
        MappingSettings = copyModel.MappingSettings;
    }
    public MappingModel()
    {
        MappingSettings = new MappingSettingsModel();
    }

    // PUBLIC PROP
    public string DriveLetter { get; set; } = string.Empty;

    public string NetworkPath { get; set; } = string.Empty;

    // Return SERVER from \\SERVER\share\etc
    [JsonIgnore]
    public string Hostname
    {
        get 
        {
            
            if (string.IsNullOrEmpty(NetworkPath)) return string.Empty;            
            string pattern = @"\\\\(.*?)\\";
            Match m = Regex.Match(NetworkPath, pattern);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return string.Empty;
        }
    }    
    
    public MappingSettingsModel MappingSettings { get; set; }

    [JsonIgnore]
    public bool ConnectCommandVisible =>
        ShareStateProp == ShareState.Available && 
        MappingStateProp == MappingState.Unmapped;
    
    [JsonIgnore]
    public bool DisconnectCommandVisible =>
        MappingStateProp == MappingState.Mapped;    

    [JsonIgnore]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    ShareState shareStateProp = ShareState.Undefined;   

    [JsonIgnore]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    MappingState mappingStateProp = MappingState.Undefined;

    //string? name;
    //string? caption;
    //string? volumeName;
    //string? deviceID;
    //string? fileSystem;
    //string? freeSpace;
    //string? size;
    //string? volumeSerialNumber;

}

