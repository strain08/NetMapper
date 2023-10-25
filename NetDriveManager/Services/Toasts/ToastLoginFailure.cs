using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastLoginFailure : ToastBase<ToastActionsSimple>
    {
        public ToastLoginFailure(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            var toastContent = new ToastContentBuilder()
               .AddArgument("A", ToastActionsDisconnect.ShowWindow)
               .AddText($"Login failure connecting to {m.NetworkPath}.")
               .AddText($"Please connect the share in windows or delete the mapping.")
               .SetToastScenario(ToastScenario.Reminder);

            var toast = new ToastNotification(toastContent.GetXml());
            previousMsg = null;
            Show(toast);
        }
    }
}
