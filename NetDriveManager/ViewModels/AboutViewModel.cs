using NetMapper.Services.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.ViewModels
{
    public class AboutViewModel:ViewModelBase
    {
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
