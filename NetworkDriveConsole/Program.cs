// See https://aka.ms/new-console-template for more information


using Microsoft.WindowsAPICodePack.Net;
using NetworkDriveConsole;
using System.Net.NetworkInformation;

using System.Security.Principal;
using System.Xml.Schema;


WmiLogicalDiskOperations c = new();
//System.Threading.Thread.Sleep(10000);

var networksConnected = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
var ifcs = NetworkInterface.GetAllNetworkInterfaces();


string sAdapter = string.Empty;

//foreach (NetworkInterface i in ifcs)
//{
//    Console.WriteLine(i.Name + " " + i.Id);
//}

var networksDisconnected = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Disconnected);
Console.WriteLine("Connected networks:");

foreach (Network network in networksConnected)
{
    //Name property corresponds to the name I originally asked about
    Console.WriteLine("[" + network.Name + "]");

    Console.WriteLine("\t[NetworkConnections]");
    foreach (NetworkConnection conn in network.Connections)
    {
        //Print network interface's GUID
        Console.WriteLine("\t\t" + conn.AdapterId.ToString());
        string sAdapterFormatted = "{" + conn.AdapterId.ToString().ToUpper() + "}";
        sAdapter = ifcs.First(o => o.Id.ToUpper().Equals(sAdapterFormatted)).Name;
        Console.WriteLine("\t\t adapter name:" + sAdapter);
    }
}
Console.WriteLine("Disconnected networks:");
foreach (Network network in networksDisconnected)
{
    //Name property corresponds to the name I originally asked about
    Console.WriteLine("[" + network.Name + "]");

    Console.WriteLine("\t[NetworkConnections]");
    foreach (NetworkConnection conn in network.Connections)
    {
        //Print network interface's GUID
        Console.WriteLine("\t\t" + conn.AdapterId.ToString());
        
        sAdapter = ifcs.First(o => o.Id.ToUpper().Equals( conn.AdapterId.ToString().ToUpper()) ).Name;
        Console.WriteLine("\t\t adapter name" + sAdapter);

    }
}


Console.ReadLine();

