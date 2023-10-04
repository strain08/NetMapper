using Avalonia;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    internal class SettingsService
    {
        public IStore<AppSettingsModel> SettingsStore { get; set; }

        public AppSettingsModel Settings;
        
        public PixelPoint WindowPosition
        {
            get
            {
                return new PixelPoint(Settings.WinX, Settings.WinY);
            }
            set 
            { 
                Settings.WinX = value.X;
                Settings.WinY = value.Y;
            }
        }

        private RunAtStartup runAtStartup;

        public SettingsService(IStore<AppSettingsModel> store)
        {
            SettingsStore = store;
            Settings = store.GetAll();
            runAtStartup = new (Settings);
            AppIsStarting();
        }
        public void AppIsStarting()
        {
            runAtStartup.Apply();

        }
        public void AppIsClosing()
        {
            SettingsStore.Update(Settings);
        }
    }
}
