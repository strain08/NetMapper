﻿using NetMapper.Models;

namespace NetMapper.Services.Settings
{
    internal class MinimizeTaskbar : SettingBase
    {       

        public MinimizeTaskbar(AppSettingsModel settings) : base(settings) { }

        public override void Apply()
        {
            
        }

        public override void Apply(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
