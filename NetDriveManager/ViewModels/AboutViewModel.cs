using NetMapper.Services.Helpers;
using NetMapper.Services.Static;
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
                string result = AppName + " v" + versionMajor + "." + versionMinor;
                return result;
            } 
        }
        public static string GitDisplayLink => "github.com/strain08/NetMapper";
        public static string GitFullLink => "https://github.com/strain08/NetMapper";

        public AboutViewModel()
        {

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
            VMServices.MainWindowViewModel!.Content = VMServices.DriveListViewModel;
        }
    }
}
