using Microsoft.Win32;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System;

namespace NetMapper.Services.Settings
{
    internal class RunAtStartup : SettingBase
    {
        RegistryKey? rk;
        const string APP_NAME = "NetMapper";
        const string RK_RUN = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";       

        public RunAtStartup(AppSettingsModel settings) : base(settings) { }

        private bool SetRunAtStartup()
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true) ?? throw new ArgumentNullException();
                rk.SetValue(APP_NAME, AppStartupFolder.GetProcessFullPath());
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
                rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true);
                if (rk == null) return false;
                string? startKey = rk.GetValue(APP_NAME, null)?.ToString();
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
                rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true) ?? throw new ArgumentNullException();
                rk.DeleteValue(APP_NAME, false);
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

    }
}
