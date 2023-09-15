using NetDriveManager.Models;
using NetDriveManager.Enums;

namespace NetDriveManager.ViewModels
{
    public class DesignDriveListViewModel : DriveListViewModel
    {
        public DesignDriveListViewModel()
        {
            DriveList = new()
            {
                new DriveModel { DriveLetter = 'X', NetworkPath = @"\\XOXO\mir1" , ShareStateProp=ShareState.Available, MappingStateProp=MappingState.Mapped },
                new DriveModel { DriveLetter = 'Y', NetworkPath = @"\\XOXO\mir2", ShareStateProp=ShareState.Unavailable, MappingStateProp=MappingState.Unmapped },
                new DriveModel { DriveLetter = 'Z', NetworkPath = @"\\XOXO\mir2\share" }
            };


        }
    }
}
