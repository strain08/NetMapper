using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Avalonia.Controls;

namespace NetMapper.Services.Static;

public class AppUtil
{
    public static string GetProcessFullPath()
    {
        var strExeFile = Process.GetCurrentProcess()?.MainModule?.FileName;
        return strExeFile ?? throw new ArgumentNullException();
    }

    public static string GetStartupFolder()
    {
        var strWorkPath = Path.GetDirectoryName(GetProcessFullPath());
        return strWorkPath ?? throw new ArgumentNullException();
    }

    public static FileVersionInfo GetVersionInfo()
    {
        return FileVersionInfo.GetVersionInfo(GetProcessFullPath());
    }

    public static string GetAppName()
    {
        if (Design.IsDesignMode) return "AppName";
        return GetVersionInfo().ProductName ?? GetVersionInfo().FileName;
    }

    public static DateTime BuildTime()
    {
        return GetLinkerTime(Assembly.GetEntryAssembly() ?? throw new ArgumentNullException());
    }

    public static DateTime GetLinkerTime(Assembly assembly)
    {
        const string BuildVersionMetadataPrefix = "+build";
        const string dateFormat = "yyyy-MM-ddTHH:mm:ss:fffZ";

        var attribute = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        if (attribute?.InformationalVersion != null)
        {
            var value = attribute.InformationalVersion;
            var index = value.IndexOf(BuildVersionMetadataPrefix);
            if (index > 0)
            {
                value = value[(index + BuildVersionMetadataPrefix.Length)..];

                return DateTime.ParseExact(
                    value,
                    dateFormat,
                    CultureInfo.InvariantCulture);
            }
        }

        return default;
    }
}