﻿using NetMapper.Models;

namespace NetMapper.Services.Static
{
    public static class StaticSettings
    {
        // saved to json
        public static AppSettingsModel? Settings { get; set; }
        
        public static bool WindowIsOpened { get; set; } = false;

        public static bool PositionOK()
        {
            if (Settings == null) return false;
            if (Settings.WinY > 0 && Settings.WinY > 0) return true;
            return false;
        }
    }
}