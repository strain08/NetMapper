using NetDriveManager.Models;

namespace NetDriveManager.ViewModels
{
    public class DesignDriveListViewModel : DriveListViewModel
    {
        public DesignDriveListViewModel()
        {
            NetDrivesList = new()
            {
                new NDModel { DriveLetter = "X:", Provider = @"\\XOXO\mir1", ConnectionState="Test" },
                new NDModel { DriveLetter = "Y:", Provider = @"\\XOXO\mir2" },
                new NDModel { DriveLetter = "Z:", Provider = @"\\XOXO\mir2\share" }
            };


        }
    }
}
