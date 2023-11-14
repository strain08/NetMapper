using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.Services.Toasts;

public class ToastDriveDisconnected : ToastBase
{
    private const string TAG = "INFO"; // toasts with same tag will be updated

    // CTOR
    public ToastDriveDisconnected(MapModel m, Action<MapModel, ToastActions> del) : base(m, del)
    {
        if (_previousMsg != null) // there is a visible notification, update
        {
            Update(ToastMessage, TAG);
            return;
        }

        var toastContent = new ToastContentBuilder()
            .AddArgument(TOAST_ACTION, ToastActions.ToastClicked)
            .AddVisualChild(new AdaptiveText
            {
                Text = new BindableString(MSG_CONTENT) // bound to ToastMessage prop
            })            
            .SetToastScenario(ToastScenario.Reminder);

        // base
        _toastNotification = new ToastNotification(toastContent.GetXml())
        {
            Tag = TAG,
            Data = new NotificationData()
        };
        _toastNotification.Data.Values[MSG_CONTENT] = _previousMsg = ToastMessage;
    }

    // 
    // Toast message format
    private string ToastMessage => $"{_mapModel.DriveLetterColon} [ {_mapModel.VolumeLabel} ] disconnected.";
}