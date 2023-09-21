using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.ViewModels;

namespace NetMapper.Services
{
    public static class VMServices
    {
        public static MainWindowViewModel? MainWindowViewModel { get; set; }
        public static DriveListViewModel DriveListViewModel { get; set; } = new DriveListViewModel();      
        public static Window? MainWindow {  get; set; }
    }
}
