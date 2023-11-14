using System.Reflection;
using ConsoleTest.Builder;
using NetMapper.Enums;
using NetMapper.Services.Toasts;
using NetMapper.Services.Toasts.Implementations;
using NetMapper.Services.Toasts.Interfaces;

namespace ConsoleTest;

internal class Program
{    
    private static void Main(string[] args)
    {
        
        ToastFactory tf = new();        
        IToastPresenter toast = tf.CreateToast();

        var toastArgs = new ToastArgsRecord(
            ToastType: ToastType.INF_CONNECT,
            DriveLetter: "C:",
            NetworkPath: @"\\sdfsdf\sdf\\");

        IToastType tt = tf.CreateToastType<RetryForceToast>("TAG",toastArgs);
        tt.SetTextLine1(new("Line1", false));
        tt.TextLine2 = new("Line2", false);

        while (true)
        {
            Console.ReadLine();
            toast.Show(tt);
            
        }
        
    }
    static void ToastBuilder(IToastBuilder t)
    {          
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