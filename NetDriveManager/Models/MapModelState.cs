using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Services.Helpers;
using System.Text.Json.Serialization;
using NetMapper.Messages;

namespace NetMapper.Models
{
    public partial class MapModel
    {
        [ObservableProperty]
        [property: JsonIgnore]
        [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
        [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
        ShareState shareStateProp = ShareState.Undefined;

        [ObservableProperty]
        [property: JsonIgnore]
        [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
        [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
        MappingState mappingStateProp = MappingState.Undefined;

        partial void OnMappingStatePropChanged(MappingState value)
        {
            WeakReferenceMessenger.Default.Send(new PropertyChangedMessage("MappingState"));

            if (value == MappingState.Mapped |
                value == MappingState.LetterUnavailable)
            {
                VolumeLabel = Interop.GetVolumeLabel(this);
            }
            if (value == MappingState.Unmapped)
            {
                VolumeLabel = "";
            }
        }

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
    }
}
