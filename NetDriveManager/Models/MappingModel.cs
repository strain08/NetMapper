using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Interfaces;
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
    [NotifyPropertyChangedFor(nameof(VolumeLabel))]
    MappingState mappingStateProp = MappingState.Undefined;


}

