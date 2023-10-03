using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using System.IO;
using System.Text.Json.Serialization;

namespace NetMapper.Models;

public partial class MapModel : ObservableObject
{
    // PUBLIC PROP
    public char DriveLetter { get; set; }    
    public string NetworkPath { get; set; } = string.Empty;
    public MappingSettingsModel Settings { get; set; } = new();

    [JsonIgnore]
    public string DriveLetterColon
    {
        get => DriveLetter + ":";
        set => DriveLetter = value[0];
    }    

    [JsonIgnore]
    public string VolumeLabel
    {
        get
        {
            DriveInfo drive = new(DriveLetterColon);
            return drive.IsReady ? drive.VolumeLabel : string.Empty;
        }
    }
   
    public MapModel Clone() 
    { 
        var clone = (MapModel)MemberwiseClone();
        clone.Settings = Settings.Clone();
        return clone; 
    }
}

