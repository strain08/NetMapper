// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

namespace WMISample
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine(IsNetworkPath(@"\\sdsd\*"));
            Console.ReadLine();
        }
    }
 
}


