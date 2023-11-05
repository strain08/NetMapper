using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.Services.Toasts;

public class ToastCanNotRemoveDrive : ToastBase<ToastActionsDisconnect>
{
    public ToastCanNotRemoveDrive(MapModel m, Action<MapModel, ToastActionsDisconnect> del) : base(m, del)
    {
        var toastContent = new ToastContentBuilder()
            .AddText(msg_line1)
            .AddText(msg_line2)
            .AddButton(new ToastButton()
                .SetContent("Retry")
                .AddArgument(TOAST_ACTION, ToastActionsDisconnect.Retry))
            .AddButton(new ToastButton()
                .SetContent("Force")
                .AddArgument(TOAST_ACTION, ToastActionsDisconnect.Force))
            .AddButton(new ToastButtonDismiss())
            .AddArgument(TOAST_ACTION, ToastActionsDisconnect.ShowWindow)
            .SetToastScenario(ToastScenario.Reminder);

        // base
        _toastNotification = new ToastNotification(toastContent.GetXml());
        _previousMsg = null;
    }

    private string msg_line1 => $"Cannot remove network drive {_mapModel.DriveLetterColon}";
    private string msg_line2 => $"Close all files in use on drive {_mapModel.DriveLetterColon} and retry.";
}