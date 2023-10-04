using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using Splat;

namespace NetMapper.Services.Settings
{
    internal abstract class SettingBase : ISetting
    {
        protected AppSettingsModel settings;

        protected SettingBase(AppSettingsModel settings)
        {
            this.settings = settings;
        }
        public abstract void Apply();
        
    }
}
