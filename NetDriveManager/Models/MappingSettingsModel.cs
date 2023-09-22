﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace NetMapper.Models
{
    public partial class MappingSettingsModel:ObservableObject
    {
        [ObservableProperty]
        bool autoConnect;
        [ObservableProperty]
        bool autoDisconnect;
        
        Coords coords;
    }
}
