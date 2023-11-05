using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;

namespace NetMapper.Services.Toasts;

public class ToastBase<TAnswer> : IDisposable where TAnswer : struct, Enum
{
    // toastData binding key
    protected const string MSG_CONTENT = "MESSAGE";

    protected const string TOAST_ACTION = "A";

    // if not null, next toast should be merged with this message
    private protected static string? _previousMsg;
    protected MapModel _mapModel;

    protected ToastNotification? _toastNotification;
    private bool disposedValue;
    protected Action<MapModel, TAnswer> toastAction;


    public ToastBase(MapModel m, Action<MapModel, TAnswer> del)
    {
        toastAction = del;
        _mapModel = m;
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ToastBase()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ToastBase<TAnswer> Show()
    {
        if (_toastNotification != null)
        {
            _toastNotification.Activated += Activated;
            _toastNotification.Dismissed += Dismissed;
            ToastNotificationManagerCompat.CreateToastNotifier().Show(_toastNotification);
        }

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
        toastAction.Invoke(_mapModel, args.GetEnum<TAnswer>(TOAST_ACTION));

        if (_toastNotification == null) return;
        _toastNotification.Activated -= Activated;
        _toastNotification.Dismissed -= Dismissed;
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