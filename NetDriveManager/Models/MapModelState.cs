using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;
using NetMapper.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetMapper.Models
{
    public partial class MapModel
    {
        [JsonIgnore]
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ConnectCommandVisible))]
        [NotifyPropertyChangedFor(nameof(DisconnectCommandVisible))]
        ShareState shareStateProp = ShareState.Undefined;

        [JsonIgnore]
        [ObservableProperty]
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

        private void UpdateShareState()
        {
            ShareStateProp = Directory.Exists(NetworkPath) ? ShareState.Available : ShareState.Unavailable;
        }

        private void UpdateMappingState()
        {
            // if it is a network drive mapped to this path -> Mapped            
            if (Utility.GetActualPathForLetter(DriveLetter) == NetworkPath)
            {
                MappingStateProp = MappingState.Mapped;
                return;
            }
            // if letter available -> unmapped, else -> unavailable
            if (Utility.GetAvailableDriveLetters().Contains(DriveLetter))
            {
                MappingStateProp = MappingState.Unmapped;
            }
            else
            {
                MappingStateProp = MappingState.LetterUnavailable;
            }

        }

        public void UpdateProperties()
        {
            UpdateShareState();
            UpdateMappingState();
        }
    }
}
