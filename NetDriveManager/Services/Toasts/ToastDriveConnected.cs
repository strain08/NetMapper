using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastDriveConnected : ToastBase<ToastActionsSimple>
    {
        private const string TAG = "INFO";
        private string ToastMessage => $"{thisModel.DriveLetterColon} [ {thisModel.VolumeLabel} ] connected.";

        public ToastDriveConnected(MapModel m, Action<MapModel, ToastActionsSimple> del) : base(m, del)
        {
            if (previousMsg != null) // toast still visible, update
            {
                Update(ToastMessage, TAG);
                return;
            }
            
            // toast not exist, create
            var toastContent = new ToastContentBuilder()
               .AddArgument("A", ToastActionsSimple.ShowWindow)
               .AddVisualChild(new AdaptiveText
               {
                   Text = new BindableString(MSG_CONTENT) // bound to ToastMessage prop
               })
               .SetToastScenario(ToastScenario.Reminder);

            var notificationData = new NotificationData();
            notificationData.Values[MSG_CONTENT] = previousMsg = ToastMessage;

            var Toast = new ToastNotification(toastContent.GetXml())
            {
                Tag = TAG,
                Data = notificationData
            };           

            Show(Toast);
        }
    }
}
