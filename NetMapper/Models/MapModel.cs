using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json.Serialization;

namespace NetMapper.Models;

public partial class MapModel : ObservableObject
{
    [JsonIgnore]
    public string ID { get; init; }

    // PUBLIC PROP
    [ObservableProperty]
    private char driveLetter;

    [ObservableProperty]
    [field: JsonIgnore]
    private string? volumeLabel;

    [ObservableProperty]
    [field: JsonIgnore]
    private bool disconnectDismissed = false;

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