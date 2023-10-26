using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;

namespace NetMapper.Models;

public partial class MapModel : ObservableObject
{
    // PUBLIC PROP
    public char DriveLetter { get; set; }

    [JsonIgnore]
    public string DriveLetterColon
    {
        get => DriveLetter + ":";
        set => DriveLetter = value[0];
    }
    public string NetworkPath { get; set; } = string.Empty;
    

    [ObservableProperty]
    [property: JsonIgnore]
    string? volumeLabel;
    
    public MappingSettingsModel Settings { get; set; } = new();

    public MapModel Clone()
    {
        var clone = (MapModel)MemberwiseClone();
        clone.Settings = Settings.Clone();
        return clone;
    }
}

