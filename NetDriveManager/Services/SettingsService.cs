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
        public IStore<AppSettingsModel> settingsStore { get; set; }

        public AppSettingsModel settings;
        
        public PixelPoint WindowPosition
        {
            get
            {
                return new PixelPoint(settings.WinX, settings.WinY);
            }
            set 
            { 
                settings.WinX = value.X;
                settings.WinY = value.Y;
            }
        }

        private RunAtStartup runAtStartup;

        public SettingsService(IStore<AppSettingsModel> store)
        {
            settingsStore = store;
            settings = store.GetAll();
            runAtStartup = new (settings);
            AppIsStarting();
        }
        public void AppIsStarting()
        {
            runAtStartup.Apply();

        }
        public void AppIsClosing()
        {
            settingsStore.Update(settings);
        }
    }
}
