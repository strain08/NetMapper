using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using NetMapper.Services.Toasts;

namespace ConsoleTest;

internal class ToastTest
{
    public static void ToastTest1()
    {
        MapModel m = new() { NetworkPath = @"\\XOXO\mir1", DriveLetter = 'X' };
        var k = string.Empty;
        while (k != "a")
        {
            k = Console.ReadLine();
            var x = ToastNotificationManagerCompat.History.GetHistory();
            Debug.WriteLine("element count:" + x.Count);
            foreach (var y in x)
                if (y != null)
                    Debug.WriteLine("Tag:" + y.Tag);


            var t = new ToastDriveConnected(m, (m, a) => { });
        }

        ToastNotificationManagerCompat.Uninstall();
    }
}