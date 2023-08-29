using NetDriveManager.Models;

namespace NetDriveManager.ViewModels
{
    public class DesignDriveListViewModel : DriveListViewModel
    {
        public DesignDriveListViewModel()
        {
            NetDrivesList = new()
            {
                new NDModel { DriveLetter = "X:", NetworkPath = @"\\XOXO\mir1", ConnectionState="Test" },
                new NDModel { DriveLetter = "Y:", NetworkPath = @"\\XOXO\mir2" },
                new NDModel { DriveLetter = "Z:", NetworkPath = @"\\XOXO\mir2\share" }
            };


        }
    }
}
