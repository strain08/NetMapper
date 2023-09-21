using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Models
{
    public partial class MappingSettingsModel:ObservableObject
    {
        [ObservableProperty]
        bool autoConnect;
        [ObservableProperty]
        bool autoDisconnect;
    }
}
