using System.Text.Json.Serialization;
using NetMapper.Enums;

namespace NetMapper.Models;

public partial class MapModel
{
    [JsonIgnore]
    public bool ConnectCommandVisible =>
        ShareStateProp == ShareState.Available &&
        MappingStateProp == MappingState.Unmapped;

    [JsonIgnore]
    public bool DisconnectCommandVisible =>
        MappingStateProp == MappingState.Mapped;
}