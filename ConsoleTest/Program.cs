using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using NetMapper.Services.Toasts;
using System.Diagnostics;
using NetMapper.Services.Helpers;
using Windows.Storage.FileProperties;

namespace ConsoleTest
{
    internal class Program
    {        
        static void Main(string[] args)
        {

            //Console.WriteLine(Interop.ConnectNetworkDrive('Z', @"\\XOXO\mir1\share", "jailman", "Meconium1980"));
            //Console.ReadLine();
            ToastTest();
        }

        private static void ToastTest()
        {
            MapModel m = new() { NetworkPath = @"\\XOXO\mir1", DriveLetter = 'X' };
            string? k = string.Empty;
            while (k != "a")
            {
                k = Console.ReadLine();
                var x = ToastNotificationManagerCompat.History.GetHistory();
                Debug.WriteLine("element count:" + x.Count);
                foreach (var y in x)
                {
                    if (y != null)
                    {
                        Debug.WriteLine("Tag:" + y.Tag);
                    }
                }
          

                var t = new ToastDriveConnected(m, (m, a) => { });
                
            }

            ToastNotificationManagerCompat.Uninstall();
        }
    }
}