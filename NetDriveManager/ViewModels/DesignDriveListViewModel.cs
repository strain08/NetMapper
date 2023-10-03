using NetMapper.Models;
using NetMapper.Enums;

namespace NetMapper.ViewModels
{
    public class DesignDriveListViewModel : DriveListViewModel
    {
        public DesignDriveListViewModel()
        {
            DriveList = new()
            {
                new MapModel { DriveLetter = 'X', NetworkPath = @"\\XOXO\mir1" , ShareStateProp=ShareState.Available, MappingStateProp=MappingState.Mapped },
                new MapModel { DriveLetter = 'Y', NetworkPath = @"\\XOXO\mir2", ShareStateProp=ShareState.Unavailable, MappingStateProp=MappingState.Unmapped },
                new MapModel { DriveLetter = 'Z', NetworkPath = @"\\XOXO\mir2\share" }
            };


        }
    }
}
