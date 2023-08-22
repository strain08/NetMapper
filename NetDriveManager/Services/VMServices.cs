using NetDriveManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Services
{
    public static class VMServices
    {
        public static MainWindowViewModel? MainWindowViewModel { get; set; }
        public static DriveListViewModel? DriveListViewModel { get; set; }
        public static DriveDetailViewModel? DriveDetailViewModel { get; set; }
    }
}
