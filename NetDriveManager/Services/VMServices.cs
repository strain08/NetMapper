using Avalonia.Controls;
using NetDriveManager.ViewModels;

namespace NetDriveManager.Services
{
    public static class VMServices
    {
        public static MainWindowViewModel MainWindowViewModel { get; set; } = new MainWindowViewModel();
        public static DriveListViewModel DriveListViewModel { get; set; } = new DriveListViewModel();
        public static Window mainWindow { get; set; }
        //public static DriveDetailViewModel? DriveDetailViewModel { get; set; }
    }
}
