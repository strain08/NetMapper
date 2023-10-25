using CommunityToolkit.Mvvm.ComponentModel;
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
            if (volumeLabel == null)
            {
                DriveInfo drive = new(DriveLetterColon);
                return volumeLabel = drive.IsReady ? drive.VolumeLabel : string.Empty;
            }
            return volumeLabel;
        }
    }

    private string? volumeLabel = null;

    public MapModel Clone()
    {
        var clone = (MapModel)MemberwiseClone();
        clone.Settings = Settings.Clone();
        return clone;
    }
}

