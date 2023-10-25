using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using NetMapper.Services.Toasts;
using System.Diagnostics;
using NetMapper.Services.Helpers;

namespace ConsoleTest
{
    internal class Program
    {        
        static void Main(string[] args)
        {

            Console.WriteLine(Interop.ConnectNetworkDrive('Z', @"\\XOXO\mir1\share", "jailman", "Meconium1980"));
            Console.ReadLine();
            //NewMethod();
        }

        private static void NewMethod()
        {
            MapModel m = new() { NetworkPath = @"\\XOXO\mir1", DriveLetter = 'X' };
            string? k = string.Empty;
            while (k != "a")
            {
                k = Console.ReadLine();
                var t = new ToastDriveConnected(m, (m, a) => { });
            }

            ToastNotificationManagerCompat.Uninstall();
        }
    }
}