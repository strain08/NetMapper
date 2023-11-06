using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Messages;
using NetMapper.Services.Helpers;

namespace NetMapper.Models;

public partial class MapModel
{
    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    private MappingState mappingStateProp = MappingState.Undefined;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    private ShareState shareStateProp = ShareState.Undefined;

    [JsonIgnore]
    private bool CanConnect =>
        ShareStateProp == ShareState.Available &&
        MappingStateProp == MappingState.Unmapped;

    [JsonIgnore]
    private bool CanDisconnect =>
        ShareStateProp == ShareState.Unavailable &&
        MappingStateProp == MappingState.Mapped;

    [JsonIgnore]
    public bool CanAutoConnect =>
        CanConnect &&
        Settings.AutoConnect;

    [JsonIgnore]
    public bool CanAutoDisconnect =>
        CanDisconnect &&
        Settings.AutoDisconnect;

    partial void OnMappingStatePropChanged(MappingState value)
    {
        WeakReferenceMessenger.Default.Send(new PropertyChangedMessage(nameof(MappingStateProp), value));

        if ((value == MappingState.Mapped) |
            (value == MappingState.LetterUnavailable))
            VolumeLabel = Interop.GetVolumeLabel(this);
        if (value == MappingState.Unmapped) VolumeLabel = "";
    }
}