// See https://aka.ms/new-console-template for more information



using NetworkDriveConsole;
using System.Net.NetworkInformation;

using System.Security.Principal;
using System.Xml.Schema;


void ShellObjects1()
{
    string ThisPC = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
    var folder = ShellFolder.FromParsingName(ThisPC) as ShellFolder;
    foreach (var child in folder)
        Console.WriteLine($"{child.Name} = {child.ParsingName}");
}

static void ShellObjects2()
{
    var shellAppType = Type.GetTypeFromProgID("Shell.Application");
    object shell = Activator.CreateInstance(shellAppType);
    var ThisPC = (Folder)shellAppType.InvokeMember("NameSpace", BindingFlags.InvokeMethod, null, shell, new object[] { 0x11 });
    foreach (FolderItem child in ThisPC.Items())
        Console.WriteLine($"{child.Name} = {child.Path}");
}

static void Main(string[] args)
{
    Console.WriteLine("WindowsAPICodePack");
    ShellObjects1();
    Console.WriteLine();

    Console.WriteLine("Shell32");
    ShellObjects2();
    Console.WriteLine();
}

Console.ReadLine();

