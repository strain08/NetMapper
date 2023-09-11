using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Models
{
    public partial class MappingSettingsModel:ObservableObject
    {
        [ObservableProperty]
        bool persistent;
        [ObservableProperty]
        bool unmapWhenUnavailable;
    }
}
