using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Enums;

namespace NetMapper.Models
{
    public class AppSettingsModel
    {
        public bool LoadAtStartup { get; set; }

        public bool RemoveUnmanaged { get; set; }

        public bool InfoNotify { get; set; }

        public Coords Coords { get; set; }
    }
}
