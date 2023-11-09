using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts;

public class ToastLoginFailure : ToastBase
{
    private string msg_line1 => $"Login failure connecting to {_mapModel.NetworkPath}.";
    private string msg_line2 => "Please connect the share in windows or delete the mapping.";
    public ToastLoginFailure(MapModel m, Action<MapModel, ToastActions> del) : base(m, del)
    {
        var toastContent = new ToastContentBuilder()
            .AddText(msg_line1)
            .AddText(msg_line2)
            .AddArgument(TOAST_ACTION, ToastActions.ShowWindow)
            .SetToastScenario(ToastScenario.Reminder);

        // base
        _previousMsg = null;
        _toastNotification = new ToastNotification(toastContent.GetXml());
    }


}