using System.Reflection;
using ConsoleTest.Builder;

namespace ConsoleTest;

internal class Program
{
    private static void Main(string[] args)
    {
        //ToastTest.ToastTest1();
        IToastBuilder t = new ConcreteBuilder();
        var d = t.GetDirector();
        d.ToastA();


        var constructors = new List<ConstructorInfo>();
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types) constructors.AddRange(type.GetConstructors());

        foreach (var items in constructors)
        {
            var Params = items.GetParameters();
            foreach (var itema in Params) Console.WriteLine(itema.ParameterType + " " + itema.Name);
        }

        Console.ReadLine();
    }
}