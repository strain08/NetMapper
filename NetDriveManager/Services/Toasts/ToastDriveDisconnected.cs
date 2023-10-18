using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastDriveDisconnected : ToastBase<ToastActionsSimple>
    {
        const string TAG = "INFO"; // toasts with same tag will be updated

        private string ToastMessage => $"Drive {thisModel.DriveLetterColon} disconnected.";

        public ToastDriveDisconnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            if (previousMsg != null) // there is a visible notification, update
            {
                UpdateToast(ToastMessage, TAG);
                return;
            }

            var toastContent = new ToastContentBuilder()
               .AddArgument("A", ToastActionsSimple.ShowWindow)
               .AddVisualChild(new AdaptiveText
               {
                   Text = new BindableString(MSG_BIND) // bound to ToastMessage prop
               })
               .SetToastScenario(ToastScenario.Reminder);

            var Toast = new ToastNotification(toastContent.GetXml())
            {
                Tag = TAG,
                Data = new NotificationData()
            };
            Toast.Data.Values[MSG_BIND] = previousMsg = ToastMessage;

            Show(Toast);

        }

    }
}
