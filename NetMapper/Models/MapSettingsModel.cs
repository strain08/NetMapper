using CommunityToolkit.Mvvm.ComponentModel;

namespace NetMapper.Models;

public partial class MapSettingsModel : ObservableObject
{
    [ObservableProperty] bool autoConnect;

    [ObservableProperty] bool autoDisconnect;

    public MapSettingsModel Clone()
    {
        return (MapSettingsModel)MemberwiseClone();
    }
}