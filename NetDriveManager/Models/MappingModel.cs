using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Services.Helpers;
using System.IO;
using System.Text.Json.Serialization;

namespace NetMapper.Models;

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
    public char DriveLetter { get; set; }

    [JsonIgnore]
    public string DriveLetterColon
    {
        get => DriveLetter + ":";
        set => DriveLetter = value[0];
    }

    public string NetworkPath { get; set; } = string.Empty;

    [JsonIgnore]
    public string VolumeLabel
    {
        get
        {
            DriveInfo drive = new(DriveLetterColon);
            return drive.IsReady ? drive.VolumeLabel : string.Empty;
        }
    }

    public MappingSettingsModel MappingSettings { get; set; }
    
    private bool CanConnect =>
        ShareStateProp == ShareState.Available &&
        MappingStateProp == MappingState.Unmapped;
    
    private bool CanDisconnect =>
        ShareStateProp == ShareState.Unavailable &&
        MappingStateProp == MappingState.Mapped;

    public bool CanAutoConnect =>
        CanConnect &&
        MappingSettings.AutoConnect;

    public bool CanAutoDisconnect => 
        CanDisconnect && 
        MappingSettings.AutoDisconnect;    


}

