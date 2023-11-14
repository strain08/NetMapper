using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Messages;

namespace NetMapper.Models;

public partial class MapModel : ObservableObject
{
    [JsonIgnore]
    public string ID { get; init; }
    
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

    public MapModel()
    {
        ID = Guid.NewGuid().ToString();
    }
    public MapModel Clone()
    {
        var clone = (MapModel)MemberwiseClone();
        clone.Settings = Settings.Clone();
        return clone;
    }
    
}