using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastDriveConnected : ToastBase<ToastActionsSimple>
    {
        private const string TAG = "INFO";
        private string ToastMessage => $"Drive {thisModel.DriveLetterColon} connected.";


        public ToastDriveConnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            if (previousMsg != null) // toast still visible, update
            {
                UpdateToast(ToastMessage, TAG);
                return;
            }
            
            // toast not exist, create
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
