using System.Collections.ObjectModel;
using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.ViewModels;

public class DesignDriveListViewModel : DriveListViewModel
{
    public DesignDriveListViewModel()
    {
        DriveList = new ObservableCollection<MapModel>
        {
            new MapModel
            {
                DriveLetter = 'X', 
                NetworkPath = @"\\XOXO\mir1", 
                ShareStateProp = ShareState.Available, 
                VolumeLabel="Volume label",
                MappingStateProp = MapState.Mapped
            },
            new MapModel
            {
                DriveLetter = 'Y', 
                NetworkPath = @"\\XOXO\mir2", 
                ShareStateProp = ShareState.Unavailable,
                MappingStateProp = MapState.Unmapped
            },
            new MapModel 
            { 
                DriveLetter = 'Z', 
                NetworkPath = @"\\XOXO\mir2\share" 
            }
        };
    }
}