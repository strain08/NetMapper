using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Models;
using System;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts;


public class ToastBase : IDisposable
{
    // toastData binding key
    protected const string MSG_CONTENT = "MESSAGE";
    protected const string TOAST_ACTION = "A";

    // if not null, next toast should be merged with this message
    private protected static string? _previousMsg;
    protected MapModel _mapModel;

    protected ToastNotification? _toastNotification;
    private bool disposedValue;
    protected Action<MapModel, ToastActions> toastAction;

    public ToastBase(MapModel m, Action<MapModel, ToastActions> del)
    {
        toastAction = del;
        _mapModel = m;
    }

    public ToastBase Show()
    {
        if (_toastNotification == null)
            throw new ArgumentNullException(nameof(_toastNotification), "Toast notification not set.");
        _toastNotification.Activated += Activated;
        _toastNotification.Dismissed += Dismissed;
        ToastNotificationManagerCompat.CreateToastNotifier().Show(_toastNotification);

        return this;
    }

    protected void Update(string newMessage, string tag)
    {
        var data = new NotificationData { SequenceNumber = 0 };
        data.Values[MSG_CONTENT] = _previousMsg += "\n" + newMessage;
        ToastNotificationManagerCompat.CreateToastNotifier().Update(data, tag);
    }

    private void Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
    {
        if (_toastNotification == null) return;
        _previousMsg = null; // toast became invisible or dismissed, do not update anymore            
        _toastNotification.Activated -= Activated;
        _toastNotification.Dismissed -= Dismissed;
    }

    private void Activated(ToastNotification sender, object obj)
    {
        _previousMsg = null;
        var eventArgs = obj as ToastActivatedEventArgs;
        var args = ToastArguments.Parse(eventArgs?.Arguments);
        try
        {
            var answer = args.GetEnum<ToastActions>(TOAST_ACTION);
            toastAction.Invoke(_mapModel, answer);
        }
        catch { }

        if (_toastNotification == null) return;
        _toastNotification.Activated -= Activated;
        _toastNotification.Dismissed -= Dismissed;
    }
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                if (_toastNotification != null)
                {
                    _toastNotification.Activated -= Activated;
                    _toastNotification.Dismissed -= Dismissed;
                    _toastNotification = null;
                }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }
}