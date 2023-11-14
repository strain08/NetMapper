using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Services.Toasts.Interfaces;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts.Implementations;

public class ToastPresenter : IToastPresenter
{
    private static string? previousText1, previousText2;
    private Queue<ToastNotification> toastQueue = new();
    private NotificationData notificationData = new();

    public void Show(IToast ToastData)
    {
        notificationData.SequenceNumber = 0;
        if (CanUpdate(ToastData) &&
            ToastNotificationManagerCompat
                  .CreateToastNotifier()
                  .Update(notificationData, ToastData.Tag) == NotificationUpdateResult.Succeeded)
            return;

        notificationData.Values[ToastLines.LINE1.ToString()] = ToastData.TextLine1.Text;
        notificationData.Values[ToastLines.LINE2.ToString()] = ToastData.TextLine2.Text;

        if (ToastData.TextLine1.Update)
            previousText1 = ToastData.TextLine1.Text;
        if (ToastData.TextLine2.Update)
            previousText2 = ToastData.TextLine2.Text;

        var _xml = ToastData.GetToastContent().GetXml();
        var _notification = new ToastNotification(_xml)
        {
            Tag = ToastData.Tag,
            Data = notificationData
        };

        _notification.Dismissed += _notification_Dismissed;

        toastQueue.Enqueue(_notification);
        if (toastQueue.Count > 5)
            toastQueue.Dequeue();
        ToastNotificationManagerCompat.CreateToastNotifier().Show(_notification);

    }

    private bool CanUpdate(IToast ToastData)
    {
        bool willUpdate = false;
        if (ToastData.TextLine1.Update && previousText1 is not null)
        {
            previousText1 += "\n" + ToastData.TextLine1.Text;
            notificationData.Values[ToastLines.LINE1.ToString()] = previousText1;
            willUpdate |= true;
        }

        if (ToastData.TextLine2.Update && previousText2 is not null)
        {
            previousText2 += "\n" + ToastData.TextLine2.Text;
            notificationData.Values[ToastLines.LINE2.ToString()] = previousText2;
            willUpdate |= true;
        }

        return willUpdate;
    }

    private void _notification_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
    {
        previousText1 = null;
        previousText2 = null;
    }
}
