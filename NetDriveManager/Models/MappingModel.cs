using CommunityToolkit.Mvvm.ComponentModel;
using NetDriveManager.Enums;
using System.IO;
using System.Text.Json.Serialization;

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
    public char DriveLetter { get; set; }

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

