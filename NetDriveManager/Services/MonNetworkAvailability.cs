using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetDriveManager.Services
{
    public delegate void ConnectionOnlineDelegate();
    public delegate void ConnectionOfflineDelegate();

    public class MonNetworkAvailability
    {
        public ConnectionOfflineDelegate? OnConnectionOffline;
        public ConnectionOnlineDelegate? OnConnectionOnline;

        public MonNetworkAvailability()
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        }
        private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (OnConnectionOnline == null) return;
            if (OnConnectionOffline == null) return;

            if (e.IsAvailable)
            {
                OnConnectionOnline();
            }
            else
            {
                OnConnectionOffline();
            }
        }
        ~MonNetworkAvailability()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }

    }
}
