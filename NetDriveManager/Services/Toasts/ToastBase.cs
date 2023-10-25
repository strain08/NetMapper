using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using System.Diagnostics;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    public class ToastBase<T> :IDisposable where T : struct, Enum 
    {
        private protected static string? previousMsg;
        protected const string MSG_CONTENT = "MESSAGE";

        protected ToastNotification? toastNotification;
        protected Action<MapModel, T> thisDel;
        protected MapModel thisModel;        
        private bool disposedValue;
        

        public ToastBase(MapModel m, Action<MapModel, T> del)
        {
            thisDel = del;           
            thisModel = m;  
        }

        public void Show(ToastNotification toast) 
        {
            toastNotification = toast;
            toastNotification.Activated += Activated;
            toastNotification.Dismissed += Dismissed;
            ToastNotificationManagerCompat.CreateToastNotifier().Show(toastNotification);
        }        
        
        protected void Update(string newMessage, string tag)
        {
            var data = new NotificationData() { SequenceNumber = 0 };
            data.Values[MSG_CONTENT] = previousMsg += "\n" + newMessage;            
            ToastNotificationManagerCompat.CreateToastNotifier().Update(data, tag);
        }

        private void Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            if (toastNotification == null) return;
            previousMsg = null; // toast became invisible or dismissed, do not update anymore            
            toastNotification.Activated -= Activated;
            toastNotification.Dismissed -= Dismissed;
        }

        private void Activated(ToastNotification sender, object obj)
        {
            previousMsg = null;
            var eventArgs = obj as ToastActivatedEventArgs;
            ToastArguments args = ToastArguments.Parse(eventArgs?.Arguments);
            thisDel.Invoke(thisModel, args.GetEnum<T>("A"));

            if (toastNotification == null) return;
            toastNotification.Activated -= Activated;                
            toastNotification.Dismissed -= Dismissed;            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (toastNotification != null)
                    {
                        toastNotification.Activated -= Activated;
                        toastNotification.Dismissed -= Dismissed;
                        toastNotification = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
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
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
