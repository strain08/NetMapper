using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Services.Helpers;
using System.IO;
using System.Text.Json.Serialization;

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
        [NotifyPropertyChangedFor(nameof(VolumeLabel))]
        MappingState mappingStateProp = MappingState.Undefined;


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

        public void UpdateProperties()
        {
            UpdateShareState();
            UpdateMappingState();
        }

        private void UpdateShareState()
        {
            ShareStateProp = Directory.Exists(NetworkPath) ? 
                ShareState.Available : ShareState.Unavailable;
        }

        private void UpdateMappingState()
        {
            // if it is a network drive mapped to this path -> Mapped            
            if (Interop.GetActualPathForLetter(DriveLetter) == NetworkPath)
            {
                volumeLabel = null;
                MappingStateProp = MappingState.Mapped;
                return;
            }
            // if letter available -> unmapped, else -> unavailable
            if (Interop.GetAvailableDriveLetters().Contains(DriveLetter))
            {
                MappingStateProp = MappingState.Unmapped;
            }
            else
            {
                volumeLabel = null;
                MappingStateProp = MappingState.LetterUnavailable;

            }

        }


    }
}
