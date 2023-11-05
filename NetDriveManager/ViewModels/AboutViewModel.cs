using System;
using System.Diagnostics;
using NetMapper.Attributes;
using NetMapper.Services.Helpers;
using NetMapper.Services.Interfaces;

namespace NetMapper.ViewModels;

public partial class AboutViewModel : ViewModelBase
{
    private readonly DateTime buildTime;

    private readonly INavService nav;

    public AboutViewModel() { }

    [ResolveThis]
    public AboutViewModel(INavService navService)
    {
        // get BuildTime from assembly
        buildTime = AppUtil.BuildTime();
        nav = navService;
    }

    public static string AppNameAndVersion
    {
        get
        {
            var fvi = AppUtil.GetVersionInfo();
            var AppName = fvi.ProductName ?? fvi.FileName;
            var versionMajor = fvi.FileMajorPart.ToString();
            var versionMinor = fvi.FileMinorPart.ToString();
            var result = AppName + " " + versionMajor + "." + versionMinor + "b";
            return result;
        }
    }

    public static string GitDisplayLink => "github.com/strain08/NetMapper";
    public static string GitFullLink => "https://github.com/strain08/NetMapper";

    public string BuildTime =>
        "build: " +
        buildTime.Year +
        "." +
        buildTime.Month +
        "." +
        buildTime.Day;

    public void HandleLinkClicked()
    {
        ProcessStartInfo psi = new()
        {
            UseShellExecute = true,
            FileName = GitFullLink
        };
        Process.Start(psi);
    }

    public void OkCommand()
    {
        nav.GoTo<DriveListViewModel>();
    }
}