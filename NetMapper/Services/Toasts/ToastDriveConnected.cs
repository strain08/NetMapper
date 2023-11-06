using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.Services.Toasts;

public class ToastDriveConnected : ToastBase<ToastActionsSimple>
{
    private const string TAG = "INFO";

    public ToastDriveConnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
    {
        if (_previousMsg != null) // toast still visible, update
        {
            Update(ToastMessage, TAG);
            return;
        }

        // toast not exist, create
        var toastContent = new ToastContentBuilder()
            .AddVisualChild(new AdaptiveText
            {
                Text = new BindableString(MSG_CONTENT) // bound to ToastMessage prop
            })
            .AddArgument(TOAST_ACTION, ToastActionsSimple.ShowWindow)
            .SetToastScenario(ToastScenario.Reminder);

        var notificationData = new NotificationData();
        notificationData.Values[MSG_CONTENT] = _previousMsg = ToastMessage;

        //base
        _toastNotification = new ToastNotification(toastContent.GetXml())
        {
            Tag = TAG,
            Data = notificationData
        };
    }

    private string ToastMessage => $"{_mapModel.DriveLetterColon} [ {_mapModel.VolumeLabel} ] connected.";
}