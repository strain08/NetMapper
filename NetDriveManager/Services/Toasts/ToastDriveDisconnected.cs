using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastDriveDisconnected : ToastBase<ToastActionsSimple>
    {
        private const string TAG = "INFO"; // toasts with same tag will be updated
        
        // Toast message format
        private string ToastMessage => $"{thisModel.DriveLetterColon} [ {thisModel.VolumeLabel} ] disconnected.";

        // CTOR
        public ToastDriveDisconnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            if (previousMsg != null) // there is a visible notification, update
            {
                Update(ToastMessage, TAG);
                return;
            }

            var toastContent = new ToastContentBuilder()
               .AddArgument("A", ToastActionsSimple.ShowWindow)
               .AddVisualChild(new AdaptiveText
               {
                   Text = new BindableString(MSG_CONTENT) // bound to ToastMessage prop
               })
               .SetToastScenario(ToastScenario.Reminder);

            var Toast = new ToastNotification(toastContent.GetXml())
            {
                Tag = TAG,
                Data = new NotificationData()
            };
            Toast.Data.Values[MSG_CONTENT] = previousMsg = ToastMessage;

            Show(Toast);

        }

    }
}
