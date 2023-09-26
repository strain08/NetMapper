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
            Approvers();
           // Procedures();
        }

     

        static void Approvers()
        {
            // Setup Chain of Responsibility
            Notification notif = new Toast().Next(new MessageBox().Next(new Toast()));
            // Generate and process purchase requests
            Model p = new() { name = "dasds" };
            notif.NotifyDriveAdded(p);
            // Wait for user
            Console.ReadKey();
        }
    }
}