﻿using Microsoft.Win32;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using System;

namespace NetMapper.Services.Settings
{
    internal class SetRunAtStartup : SettingBase
    {
        RegistryKey? rk;
        private string AppName => AppUtil.GetAppName();
        const string RK_RUN = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";        

        private bool AddRunAtStartup()
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true) ?? throw new ArgumentNullException();
                rk.SetValue(AppName, AppUtil.GetProcessFullPath());
            }
            catch
            {                
                return false;
            }
            return true;
        }

        private bool RemoveRunAtStartup()
        {
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true) ?? throw new ArgumentNullException();
                rk.DeleteValue(AppName, false);
            }
            catch
            {

            }
            return true;
        }

        public override void Apply()
        {            
            
            if (GetAppSettings().bLoadAtStartup)
            {                
                AddRunAtStartup();
            }
            else
            {
                RemoveRunAtStartup();
            }
        }

        //public bool Validate() 
        //{
        //    try
        //    {
        //        rk = Registry.CurrentUser.OpenSubKey(RK_RUN, true);
        //        if (rk == null) return false;
        //        string? startKey = rk.GetValue(APP_NAME, null)?.ToString();
        //        if (startKey == null) return false;
        //        if (startKey != AppStartupFolder.GetProcessFullPath()) return false;
        //        return true;

        //    }
        //    catch 
        //    { 
        //        return false; 
        //    }
        //}        
    }
}
