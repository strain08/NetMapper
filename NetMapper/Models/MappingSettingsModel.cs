using CommunityToolkit.Mvvm.ComponentModel;

namespace NetMapper.Models;

public partial class MappingSettingsModel : ObservableObject
{
    [ObservableProperty] private bool autoConnect;

    [ObservableProperty] private bool autoDisconnect;

    public MappingSettingsModel Clone()
    {
        return (MappingSettingsModel)MemberwiseClone();
    }
}