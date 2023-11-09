using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts;

public class ToastCanNotRemoveDrive : ToastBase
{
    private string msg_line1 => $"Cannot remove network drive {_mapModel.DriveLetterColon}";
    private string msg_line2 => $"Close all files in use on drive {_mapModel.DriveLetterColon} and retry.";
    public ToastCanNotRemoveDrive(MapModel m, Action<MapModel, ToastActions> del) : base(m, del)
    {
        var toastContent = new ToastContentBuilder()
            .AddText(msg_line1)
            .AddText(msg_line2)
            .AddButton(new ToastButton()
                .SetContent("Retry")
                .AddArgument(TOAST_ACTION, ToastActions.Retry))
            .AddButton(new ToastButton()
                .SetContent("Force")
                .AddArgument(TOAST_ACTION, ToastActions.Force))
            .AddButton(new ToastButtonDismiss())
            .AddArgument(TOAST_ACTION, ToastActions.ShowWindow)
            .SetToastScenario(ToastScenario.Reminder);

        // base
        _toastNotification = new ToastNotification(toastContent.GetXml());
        _previousMsg = null;
    }
}