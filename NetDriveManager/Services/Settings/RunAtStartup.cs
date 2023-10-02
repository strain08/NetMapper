using Microsoft.Win32;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using ReactiveUI;
using System;

namespace NetMapper.Services.Settings
{
    internal class RunAtStartup : ASettings<bool>
    {
        RegistryKey? rk;
        const string AppName = "NetMapper";
        const string HKCU = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public RunAtStartup() : base() { }

        public RunAtStartup(AppSettingsModel settings) : base(settings) { }

        private bool SetRunAtStartup()
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(HKCU, true) ?? throw new ArgumentNullException();
                rk.SetValue(AppName, AppStartupFolder.GetProcessFullPath());
            }
            catch
            {                
                return false;
            }
            return true;
        }
        public bool Validate() 
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(HKCU, true);
                if (rk == null) return false;
                string? startKey = rk.GetValue(AppName, null)?.ToString();
                if (startKey == null) return false;
                if (startKey != AppStartupFolder.GetProcessFullPath()) return false;
                return true;
                
            }
            catch 
            { 
                return false; 
            }
        }
        private bool RemoveRunAtStartup()
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(HKCU, true) ?? throw new ArgumentNullException();
                rk.DeleteValue(AppName, false);
            }
            catch
            {
                
            }
            return true;
        }

        public override void Apply()
        {
            if (settings.bLoadAtStartup)
            {                
                SetRunAtStartup();
            }
            else
            {
                RemoveRunAtStartup();
            }
        }

        public override bool Get()
        {
            return settings.bLoadAtStartup;
        }
    }
}
