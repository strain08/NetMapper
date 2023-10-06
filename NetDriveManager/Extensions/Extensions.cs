using NetMapper.Services.Settings;
using System;
using System.Collections.Generic;

namespace NetMapper.Extensions
{
    internal static class Extensions
    {
        public static ISetting? _GetSetting(this List<ISetting> settings, Type settingType)
        {
            return settings.Find((s)=>s.GetType() == settingType);
        }
    }
}
