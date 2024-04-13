using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Services.Static;
using System;
using System.Diagnostics;

namespace NetMapper.ViewModels;

public partial class AboutViewModel : ViewModelBase
{
    private readonly DateTime _buildTime;
    private readonly INavService _nav;

#nullable disable
    public AboutViewModel() { }
#nullable restore

    [ResolveThis]
    public AboutViewModel(INavService navService)
    {
        // get BuildTime from assembly
        _buildTime = AppUtil.BuildTime();
        _nav = navService;
    }

    public static string AppNameAndVersion
    {
        get
        {
            var fvi = AppUtil.GetVersionInfo();
            var appName = fvi.ProductName ?? fvi.FileName;
            var versionMajor = fvi.FileMajorPart.ToString();
            var versionMinor = fvi.FileMinorPart.ToString();
            var result = appName + " " + versionMajor + "." + versionMinor + "b";
            return result;
        }
    }

    public static string GitDisplayLink => "github.com/strain08/NetMapper";
    public static string GitFullLink => "https://github.com/strain08/NetMapper";

    public string BuildTime =>
        "build: " +
        _buildTime.Year +
        "." +
        _buildTime.Month +
        "." +
        _buildTime.Day;

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
        _nav.GoTo<DriveListViewModel>();

    }
}