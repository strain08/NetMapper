using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetDriveManager.Services.Monitoring
{
    public delegate void ConnectionOnline();
    public delegate void ConnectionOffline();

    public class NetworkAvailabilityMon
    {
       public ConnectionOffline? OnConnectionOffline;
       public ConnectionOnline? OnConnectionOnline;

        public NetworkAvailabilityMon()
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
        ~NetworkAvailabilityMon()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
        }
        
    }
}
