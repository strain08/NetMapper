using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json.Serialization;

namespace NetMapper.Models
{
    public class AppSettingsModel
    {
        public bool SetLoadAtStartup { get; set; }
        public bool SetRemoveUnmanaged { get; set; }
        public bool SetMinimizeToTaskbar { get; set; }
        public bool SetInfoNotify { get; set; }
        
        public double WindowHeight { get; set; } = 400;
        public double WindowWidth { get; set; } = 225;
        public int WinX { get; set; }
        public int WinY { get; set; }

        [JsonIgnore]
        public PixelPoint WindowPosition
        {
            get
            {
                return new PixelPoint(WinX, WinY);
            }
            set
            {
                WinX = value.X;
                WinY = value.Y;
            }
        }
        public bool PositionOK()
        {
            if (WinY > 0 && WinY > 0) return true;
            return false;
        }
        
        [JsonIgnore]
        public bool WindowIsOpened = false;
        [JsonIgnore]
        public bool EventsInitialized = false;

        public AppSettingsModel Clone() => (AppSettingsModel)MemberwiseClone();
    }
}
