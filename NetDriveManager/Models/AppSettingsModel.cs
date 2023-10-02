using CommunityToolkit.Mvvm.ComponentModel;

namespace NetMapper.Models
{
    public class AppSettingsModel
    {
        public bool bLoadAtStartup { get; set; }

        public bool bRemoveUnmanaged { get; set; }

        public bool bMinimizeToTaskbar { get; set; }

        public bool bInfoNotify { get; set; }

        public Coords Coords { get; set; }
    }
}
