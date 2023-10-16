using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;
using System.Diagnostics;
using Windows.UI.Notifications;

namespace NetMapper.Services.Toasts
{
    internal class ToastBase<T> :IDisposable where T : struct, Enum 
    {        
        protected ToastNotification? toastNotification;
        protected ToastNotifierCompat? toastNotifier;
        protected Action<MapModel, T> del;
        protected MapModel m;
        private bool disposedValue;

        public ToastBase(MapModel m, Action<MapModel, T> del)
        {
            this.del = del;           
            this.m = m;  
        }
        protected void Show(ToastContentBuilder toastContent) 
        {          
            toastNotification = new ToastNotification(toastContent.GetXml());
            toastNotification.Activated += Notif_Activated;
            toastNotification.Dismissed += ToastNotification_Dismissed;
            toastNotifier = ToastNotificationManagerCompat.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
            
        }

        private void ToastNotification_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            if (toastNotification == null) return;
            toastNotification.Activated -= Notif_Activated;
            toastNotification.Dismissed -= ToastNotification_Dismissed;
        }

        private void Notif_Activated(ToastNotification sender, object obj)
        {
            var eventArgs = obj as ToastActivatedEventArgs;
            ToastArguments args = ToastArguments.Parse(eventArgs?.Arguments);
            del.Invoke(m, args.GetEnum<T>("A"));

            if (toastNotification == null) return;
            toastNotification.Activated -= Notif_Activated;                
            toastNotification.Dismissed -= ToastNotification_Dismissed;            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (toastNotification != null)
                    {
                        toastNotification.Activated -= Notif_Activated;
                        toastNotification.Dismissed -= ToastNotification_Dismissed;
                        toastNotification = null;
                        toastNotifier = null;
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
