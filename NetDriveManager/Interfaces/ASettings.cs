using NetMapper.Models;
using NetMapper.Services;
using Splat;

namespace NetMapper.Interfaces
{
    internal abstract class ASettings<T>
    {
        protected AppSettingsModel settings;
        protected ASettings()
        {
            settings = Locator.Current.GetRequiredService<AppSettingsModel>();
        }
        protected ASettings(AppSettingsModel settings)
        {
            this.settings = settings;
        }
        public abstract void Apply();
        public abstract T Get();
    }
}
