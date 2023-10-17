using NetMapper.Services.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.ViewModels
{
    public class AboutViewModel:ViewModelBase
    {
        public AboutViewModel()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductMajorPart.ToString();
            string versionMajor = fvi.FileMajorPart.ToString();
            string versionMinor = fvi.FileMinorPart.ToString();
            string versionFix = fvi.FileBuildPart.ToString();
            Debug.Print(versionMajor + " " + versionMinor + " " + versionFix);
        }
        public void HandleLinkClicked() 
        {
            // UriBuilder builder = new UriBuilder{ Path = @"https:\\github.com" };
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = @"https:\\github.com"
            };
            Process.Start(psi);
            
        }
        public void OkCommand()
        {
            VMServices.MainWindowViewModel!.Content = VMServices.DriveListViewModel;
        }
    }
}
