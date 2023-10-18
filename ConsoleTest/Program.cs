using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using NetMapper.Services.Toasts;
using System.Diagnostics;

namespace ConsoleTest
{
    internal class Program
    {        
        static void Main(string[] args)
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