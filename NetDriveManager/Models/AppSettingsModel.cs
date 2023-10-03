using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json.Serialization;

namespace NetMapper.Models
{
    public class AppSettingsModel
    {
        public bool bLoadAtStartup { get; set; }

        public bool bRemoveUnmanaged { get; set; }

        public bool bMinimizeToTaskbar { get; set; }

        public bool bInfoNotify { get; set; }

        
        public double WindowHeight { get; set; } = 400;
        public double WindowWidth { get; set; } = 225;
        public int WinX { get; set; }
        public int WinY { get; set; }     

        public AppSettingsModel Clone() => (AppSettingsModel)MemberwiseClone();
    }
}
