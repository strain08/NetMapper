using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Svg.Skia;
using NetMapper.Services.Helpers;
using Serilog;
using System;
using System.IO;
using Windows.Foundation.Diagnostics;

namespace NetMapper
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            string startupFolder = AppStartupFolder.GetStartupFolder();
            string logFile=Path.Combine(startupFolder, "NetMapper.log");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFile)
                .CreateLogger();
            Log.Information("Application started in: " + AppStartupFolder.GetStartupFolder());
            try
            {
                BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
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
            
            return   AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
            
        }
    }
}