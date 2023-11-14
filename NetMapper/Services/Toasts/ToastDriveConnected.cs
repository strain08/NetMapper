using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts;

public class ToastDriveConnected : ToastBase
{
    private string ToastMessage => $"{_mapModel.DriveLetterColon} [ {_mapModel.VolumeLabel} ] connected.";
    public ToastDriveConnected(MapModel m, Action<MapModel, ToastActions> del, string Tag) : base(m, del, Tag)
    {
        if (_previousMsg != null) // toast still visible, update
        {
            Update(ToastMessage, Tag);
            return;
        }

        // toast not exist, create
        var toastContent = new ToastContentBuilder()
            .AddVisualChild(new AdaptiveText
            {
                Text = new BindableString(MSG_CONTENT) // bound to ToastMessage prop
            })
            
            .AddArgument(TOAST_ACTION, ToastActions.ToastClicked)
            .SetToastScenario(ToastScenario.Reminder);

        var notificationData = new NotificationData();
        notificationData.Values[MSG_CONTENT] = ToastMessage;
        _previousMsg = ToastMessage;

        //base
        _toastNotification = new ToastNotification(toastContent.GetXml())
        {
            Tag = Tag,
            Data = notificationData
        };
    }


}