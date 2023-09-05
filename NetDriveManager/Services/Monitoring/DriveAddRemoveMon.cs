using System;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace NetDriveManager.Services.Monitoring
{
    public delegate void DriveConnected(string driveLetter);
    public delegate void DriveDisconnected(string driveLetter);

    public class DriveAddRemoveMon
    {
        // Detects when drive added or removed

        public DriveConnected? DriveConnectedDelegate;
        public DriveDisconnected? DriveDisconnectedDelegate;
        // CTOR
        public DriveAddRemoveMon()
        {

            WqlEventQuery wqlEventQuery;
            //var observer = new ManagementOperationObserver();
            var scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            wqlEventQuery = new WqlEventQuery
            {
                EventClassName = "__InstanceOperationEvent",
                WithinInterval = new TimeSpan(0, 0, 5),
                Condition = @"TargetInstance ISA 'Win32_LogicalDisk' "
            };

            var w = new ManagementEventWatcher(scope, wqlEventQuery);
            w.EventArrived += new EventArrivedEventHandler(w_EventArrived);
            w.Start();
        }

        private void w_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (DriveConnectedDelegate == null) return;
            if (DriveDisconnectedDelegate == null) return;

            var baseObject = e.NewEvent;

            if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_CREATION))
            {
                // Drive connected
                using var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                if (Convert.ToInt32(logicalDisk.Properties["DriveType"].Value) == (int)DriveType.Network)
                {
                    Debug.WriteLine($"Network drive {logicalDisk.Properties["Name"].Value} added.");
                    
                    DriveConnectedDelegate((string)logicalDisk.Properties["Name"].Value);
                }

            }
            else if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_DELETION))
            {
                //Drive removed

                var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                if (Convert.ToInt32(logicalDisk.Properties["DriveType"].Value) == (int)DriveType.Network)
                {
                    Debug.WriteLine($"Network drive {logicalDisk.Properties["Name"].Value} removed");
                    DriveDisconnectedDelegate((string)logicalDisk.Properties["Name"].Value);
                }
            }
        }


    }
    public static class WmiClass
    {
        public static readonly string INSTANCE_CREATION = "__InstanceCreationEvent";
        public static readonly string INSTANCE_DELETION = "__InstanceDeletionEvent";
    }

}
