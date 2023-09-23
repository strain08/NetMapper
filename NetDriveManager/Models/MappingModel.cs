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
    
    private bool CanAutoConnect =>
        CanConnect &&
        MappingSettings.AutoConnect;

    private bool CanAutoDisconnect => 
        CanDisconnect && 
        MappingSettings.AutoDisconnect;

    private bool CanDisconnect =>
        ShareStateProp == ShareState.Unavailable &&
        MappingStateProp == MappingState.Mapped &&
        MappingSettings.AutoDisconnect;

    [JsonIgnore]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    ShareState shareStateProp = ShareState.Undefined;

    [JsonIgnore]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(VolumeLabel))]
    MappingState mappingStateProp = MappingState.Undefined;

    private void UpdateShareState()
    {
        ShareStateProp = Directory.Exists(NetworkPath) ? ShareState.Available : ShareState.Unavailable;
    }

    private void UpdateMappingState()
    {
        // if it is a network drive mapped to this path -> Mapped
        string testPath = Utility.GetPathForLetter(DriveLetter);
        if (testPath == NetworkPath)
        {
            MappingStateProp = MappingState.Mapped;
            return;
        }
        // if letter available -> unmapped, else -> unavailable
        if (Utility.GetAvailableDriveLetters().Contains(DriveLetter))
        {
            MappingStateProp = MappingState.Unmapped;
        }
        else
        {
            MappingStateProp = MappingState.LetterUnavailable;
        }

    }

    public void UpdateProperties()
    {
        UpdateShareState();
        UpdateMappingState();
    }


}

