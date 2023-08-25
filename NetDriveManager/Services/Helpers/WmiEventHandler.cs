using System;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace NetDriveManager.Services.Helpers
{
    public class WmiEventHandler
    {
        // Detects when drive connected or disconnected from Windows

        // CTOR
        public WmiEventHandler()
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
            var baseObject = e.NewEvent;

            if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_CREATION))
            {
                // Drive connected
                using var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                Debug.WriteLine("Drive type is {0}",
                                  logicalDisk.Properties["DriveType"].Value);
                foreach (PropertyData pd in logicalDisk.Properties)
                {
                    Debug.WriteLine(pd.Name + "\n" + pd.Value + "\n" + pd.Qualifiers);
                    
                }

            }
            else if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_DELETION))
            {
                //Drive removed

                var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                if (Convert.ToInt32(logicalDisk.Properties["DriveType"].Value) == (int)DriveType.Network)
                {
                    Debug.WriteLine("Network drive");
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
