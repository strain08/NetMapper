using NetDriveManager.Models;

namespace NetDriveManager.ViewModels
{
    public class DesignDriveListViewModel : DriveListViewModel
    {
        public DesignDriveListViewModel()
        {
            NetDrivesList = new()
            {
                new MappingModel { DriveLetter = "X:", NetworkPath = @"\\XOXO\mir1" , ConnectionState=Enums.ConnectionState.Connected },
                new MappingModel { DriveLetter = "Y:", NetworkPath = @"\\XOXO\mir2" },
                new MappingModel { DriveLetter = "Z:", NetworkPath = @"\\XOXO\mir2\share" }
            };


        }
    }
}
