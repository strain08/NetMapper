using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.Services.Toasts;

public class ToastLoginFailure : ToastBase<ToastActionsSimple>
{
    public ToastLoginFailure(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
    {
        var toastContent = new ToastContentBuilder()
            .AddText(msg_line1)
            .AddText(msg_line2)
            .AddArgument(TOAST_ACTION, ToastActionsDisconnect.ShowWindow)
            .SetToastScenario(ToastScenario.Reminder);

        // base
        _previousMsg = null;
        _toastNotification = new ToastNotification(toastContent.GetXml());
    }

    private string msg_line1 => $"Login failure connecting to {_mapModel.NetworkPath}.";
    private string msg_line2 => "Please connect the share in windows or delete the mapping.";
}