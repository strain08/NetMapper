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
    public partial class MappingModel
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

        private void UpdateShareState()
        {
            ShareStateProp = Directory.Exists(NetworkPath) ? ShareState.Available : ShareState.Unavailable;
        }

        private void UpdateMappingState()
        {
            // if it is a network drive mapped to this path -> Mapped
            string testPath = Utility.GetPathForLetter(DriveLetter);
            if (testPath == NetworkPath)
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
