using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;

namespace NetMapper.Services.Toasts
{
    internal class ToastDriveConnected : ToastBase<ToastActionsSimple>
    {
        public ToastDriveConnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            var toastContent = new ToastContentBuilder()
                .AddArgument("A", ToastActionsSimple.ShowWindow)
                .AddText($"Drive {m.DriveLetterColon} connected.")
                .SetToastScenario(ToastScenario.Default);
            Show(toastContent);
        }
    }
}
