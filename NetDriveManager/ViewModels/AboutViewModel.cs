using NetMapper.Services.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace NetMapper.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        //const string APP_NAME = "NetMapper";
        public string AppNameAndVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string? AppName = assembly.GetName().Name ?? "";
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string versionMajor = fvi.FileMajorPart.ToString();
                string versionMinor = fvi.FileMinorPart.ToString();
                string result = AppName + " v" + versionMajor + "." + versionMinor;
                return result;
            } 
        }
        public string GitDisplayLink => "github.com/strain08/NetMapper";
        public string GitFullLink => "https://github.com/strain08/NetMapper";

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
