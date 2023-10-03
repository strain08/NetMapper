using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace NetMapper.Models
{
    public partial class MappingSettingsModel:ObservableObject
    {
        [ObservableProperty]
        bool autoConnect;
        [ObservableProperty]
        bool autoDisconnect;

        public MappingSettingsModel Clone()
        {
            return (MappingSettingsModel)MemberwiseClone();
        }
    }
}
