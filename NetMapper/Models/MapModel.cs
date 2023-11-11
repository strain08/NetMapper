using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NetMapper.Models;

public partial class MapModel : ObservableObject
{
    // PUBLIC PROP
    [ObservableProperty]
    private char driveLetter;

    [ObservableProperty]
    [property: JsonIgnore]
    private string? volumeLabel;

    [JsonIgnore]
    public string DriveLetterColon
    {
        get => DriveLetter + ":";
        set => DriveLetter = value[0];
    }

    private string networkPath = string.Empty;
    public string NetworkPath 
    { 
        get => networkPath; 
        set => networkPath = value.Trim();         
    }

    public MapSettingsModel Settings { get; set; } = new();

    public MapModel Clone()
    {
        var clone = (MapModel)MemberwiseClone();
        clone.Settings = Settings.Clone();
        return clone;
    }
}