using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Messages;
using System.Text.Json.Serialization;

namespace NetMapper.Models;

public partial class MapModel
{
    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    private MapState mappingStateProp = MapState.Undefined;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
    [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
    private ShareState shareStateProp = ShareState.Undefined;

    partial void OnMappingStatePropChanged(MapState value)
    {
        WeakReferenceMessenger.Default.Send(new PropChangedMessage(this, nameof(MappingStateProp)));
    }

    [JsonIgnore]
    public bool ConnectCommandVisible =>
       ShareStateProp == ShareState.Available &&
       MappingStateProp == MapState.Unmapped;

    [JsonIgnore]
    public bool DisconnectCommandVisible =>
        MappingStateProp == MapState.Mapped;
}