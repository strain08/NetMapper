using NetMapper.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Extensions
{
    internal static class Extensions
    {
        public static ISetting? GetSetting(this List<ISetting> settings, Type settingType)
        {
            return settings.Find((s)=>s.GetType() == settingType);
        }
    }
}
