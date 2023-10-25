using NetMapper.Services;
using NetMapper.Services.Helpers;
using Splat;
using System;
using System.Diagnostics;

namespace NetMapper.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public static string AppNameAndVersion
        {
            get
            {
                FileVersionInfo fvi = AppUtil.GetVersionInfo();
                string AppName = fvi.ProductName ?? fvi.FileName;
                string versionMajor = fvi.FileMajorPart.ToString();
                string versionMinor = fvi.FileMinorPart.ToString();
                string result = AppName + " " + versionMajor + "." + versionMinor + "b";
                return result;
            }
        }
        public static string GitDisplayLink => "github.com/strain08/NetMapper";
        public static string GitFullLink => "https://github.com/strain08/NetMapper";

        public string BuildTime =>
            "build: " +
            buildTime.Year.ToString() +
            "." +
            buildTime.Month.ToString() +
            "." +
            buildTime.Day.ToString();

        private readonly DateTime buildTime;

        private readonly NavService navService;

        public AboutViewModel()
        {
            // get BuildTime from assembly
            buildTime = AppUtil.BuildTime();
            navService = Locator.Current.GetRequiredService<NavService>();

        }
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
            //VMServices.MainWindowViewModel!.Content = VMServices.DriveListViewModel;
            navService.GoTo<DriveListViewModel>();
        }
    }
}
