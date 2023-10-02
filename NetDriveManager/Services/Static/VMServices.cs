using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.ViewModels;

namespace NetMapper.Services.Static
{
    public static class VMServices
    {
        public static MainWindowViewModel? MainWindowViewModel { get; set; }
        public static DriveListViewModel DriveListViewModel { get; set; } = new DriveListViewModel();
        public static ApplicationViewModel? ApplicationViewModel { get; set; }
    }
}
