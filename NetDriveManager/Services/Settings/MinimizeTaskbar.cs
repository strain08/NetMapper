using NetMapper.Interfaces;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services.Settings
{
    internal class MinimizeTaskbar : ASettings<bool>
    {       

        public MinimizeTaskbar(AppSettingsModel settings) : base(settings) { }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override bool Get()
        {
            return settings.bMinimizeToTaskbar;
        }
    }
}
