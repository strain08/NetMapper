using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetDriveManager.Services
{
    public class NetMonitor
    {
        public event EventHandler<EventArgs>? ConnectionOnline;
        public event EventHandler<EventArgs>? ConnectionClosed;

        public NetMonitor()
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        }
        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                OnConnectionOnline();
            }
            else
            {
                OnConnectionClosed();
            }
        }
        ~NetMonitor()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }
        protected virtual void OnConnectionOnline()
        {
            ConnectionOnline?.Invoke(this, new EventArgs());
        }
        protected virtual void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, new EventArgs());
        }
    }
}
