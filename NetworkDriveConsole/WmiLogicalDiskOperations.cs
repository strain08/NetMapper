using System.Management;

namespace NetworkDriveConsole
{
    public class WmiLogicalDiskOperations
    {

        // CTOR
        public WmiLogicalDiskOperations()
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
            var baseObject = (ManagementBaseObject)e.NewEvent;

            if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_CREATION))
            {
                // Drive connected
                using var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                Console.WriteLine("Drive type is {0}",
                                  logicalDisk.Properties["DriveType"].Value);
                foreach (PropertyData pd in logicalDisk.Properties)
                {
                    Console.WriteLine(pd.Name + "\n" + pd.Value + "\n" + pd.Qualifiers);
                    Console.WriteLine();
                }

            }
            else if (baseObject.ClassPath.ClassName.Equals(WmiClass.INSTANCE_DELETION))
            {
                //Drive removed

                var logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                if (Convert.ToInt32(logicalDisk.Properties["DriveType"].Value) == (int)DriveType.NETWORK_DRIVE)
                {
                    Console.WriteLine("Network drive");
                }
            }
        }


    }
    public static class WmiClass
    {
        public static readonly string INSTANCE_CREATION = "__InstanceCreationEvent";
        public static readonly string INSTANCE_DELETION = "__InstanceDeletionEvent";
    }
    public enum DriveType
    {
        PHYSICAL_DRIVE = 2,
        NETWORK_DRIVE = 4
    }
}
