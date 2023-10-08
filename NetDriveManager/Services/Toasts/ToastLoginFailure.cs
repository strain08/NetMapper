using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services.Toasts
{
    internal class ToastLoginFailure : ToastBase<ToastActionsSimple>
    {
        public ToastLoginFailure(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            var toastContent = new ToastContentBuilder()
               .AddArgument("A", ToastActionsDisconnect.ShowWindow)
               .AddText($"Login failure connecting to {m.NetworkPath}.")
               .AddText($"Please connect the share in windows or delete the mapping.")
               .SetToastScenario(ToastScenario.Reminder);
            Show(toastContent);
        }
    }
}
