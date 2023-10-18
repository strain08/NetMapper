using HarfBuzzSharp;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastCanNotRemoveDrive : ToastBase<ToastActionsDisconnect>
    {
        

        public ToastCanNotRemoveDrive(MapModel m, Action<MapModel, ToastActionsDisconnect> del) : base(m, del)
        {
           
            var toastContent = new ToastContentBuilder()
                .AddText($"Cannot remove network drive {m.DriveLetterColon}")
                .AddText($"Close all files in use on drive {m.DriveLetterColon} and retry.")
                .AddButton(new ToastButton()
                            .SetContent("Retry")
                            .AddArgument("A", ToastActionsDisconnect.Retry))
                .AddButton(new ToastButton()
                            .SetContent("Force")
                            .AddArgument("A", ToastActionsDisconnect.Force))
                .AddButton(new ToastButtonDismiss())
                .AddArgument("A", ToastActionsDisconnect.ShowWindow)
                .SetToastScenario(ToastScenario.Reminder);
            var toast = new ToastNotification(toastContent.GetXml());
            Show(toast);
            
        }
    }
}
