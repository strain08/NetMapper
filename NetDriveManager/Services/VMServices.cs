using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetDriveManager.ViewModels;

namespace NetDriveManager.Services
{
    public static class VMServices
    {
        public static MainWindowViewModel? MainWindowViewModel { get; set; }
        public static DriveListViewModel DriveListViewModel { get; set; } = new DriveListViewModel();      
        public static Window mainWindow {  get; set; }
    }
}
