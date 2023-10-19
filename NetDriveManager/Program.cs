using Avalonia;
using NetMapper.Services.Helpers;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace NetMapper
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        private static readonly Mutex Mutex = new Mutex(true, "bc5bb7ee-6999-41d2-a41e-546417e43fa0");

        [STAThread]
        public static void Main(string[] args)
        {
            string startupFolder = AppUtil.GetStartupFolder();
            string logFile = Path.Combine(startupFolder, AppUtil.GetAppName() + ".log");

            if (!Mutex.WaitOne(TimeSpan.Zero, true)) return; // check if app aleady started

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFile)
                .CreateLogger();

            Log.Information("Application started in: " + AppUtil.GetStartupFolder());

            try
            {
                BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
                Mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal error.");

            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {

            // SVG Preview
            //GC.KeepAlive(typeof(SvgImageExtension).Assembly);
            //GC.KeepAlive(typeof(Avalonia.Svg.Skia.Svg).Assembly);

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();

        }
    }
}