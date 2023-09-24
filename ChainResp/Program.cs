using System;
using ChainResp.Approvers;

namespace Chain.RealWorld
{
    /// <summary>
    /// Chain of Responsibility Design Pattern
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup Chain of Responsibility
            Notification notif = new Toast().Next(new MessageBox()) ;
            
            // Generate and process purchase requests
            DriveModel p = new()
            {
                letter = "A",
                name = "diskette",
            };

            notif.NotifyDriveAdded(p);
            // Wait for user
            Console.ReadKey();
        }
    }
}