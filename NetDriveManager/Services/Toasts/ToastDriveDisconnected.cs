using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    internal class ToastDriveDisconnected : ToastBase<ToastActionsSimple>
    {
        public ToastDriveDisconnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base (m,del)
        {
            var toastContent = new ToastContentBuilder()
                .AddArgument("A", ToastActionsSimple.ShowWindow)
                .AddText($"Drive {m.DriveLetterColon} disconnected.")
                .SetToastScenario(ToastScenario.Default);
            
            Show(toastContent);
        }        

    }
}
