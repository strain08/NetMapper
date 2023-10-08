using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using NetMapper.ViewModels;
using System.Collections.Generic;

namespace NetMapper.Services.Static
{
    public static class VMServices
    {
        public static MainWindowViewModel? MainWindowViewModel { get; set; }
        public static DriveListViewModel DriveListViewModel { get; set; } = new();
        public static ApplicationViewModel? ApplicationViewModel { get; set; }

        public static List<ViewModelBase> a = new();
    }
}
