using System.Reflection;
using ConsoleTest.Builder;
using NetMapper.Services.Helpers;

namespace ConsoleTest;

internal class Program
{
    private static void Main(string[] args)
    {
        //ToastTest.ToastTest1();

        Console.WriteLine(Interop.IsNetworkPath(@"\\ddd"));
        Console.ReadLine();

        
    }
    static void ToastBuilder()
    {
        IToastBuilder t = new ConcreteBuilder<string>();        
        var d = t.GetDirector();
        d.ToastA();
    }

    static void ConstructorTest()
    {
        var constructors = new List<ConstructorInfo>();
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types) constructors.AddRange(type.GetConstructors());

        foreach (var items in constructors)
        {
            var Params = items.GetParameters();
            foreach (var itema in Params) Console.WriteLine(itema.ParameterType + " " + itema.Name);
        }
    }
}