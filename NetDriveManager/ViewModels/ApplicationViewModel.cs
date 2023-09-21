using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using NetMapper.Views;
using NetMapper.Services;

namespace NetMapper.ViewModels
{
    public class ApplicationViewModel:ViewModelBase
    {
        private readonly MainWindow _mainWindow;

        public ApplicationViewModel()
        {
            _mainWindow = new MainWindow
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
            };            
            // hide window on close
            _mainWindow.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
            
            VMServices.MainWindow = _mainWindow;
            //_mainWindow.PropertyChanged += (s, e) => 
            //{  
            //    if (e.NewValue is WindowState windowState)
            //    {
            //        switch (windowState)
            //        {
            //            case WindowState.Minimized:
            //                _mainWindow.Hide();
            //                _mainWindow.ShowInTaskbar = false;
            //                break;
            //            default:
            //                _mainWindow.ShowInTaskbar= true; 
            //                break;
            //        }
            //    }
            //};



        }

       
        public void ShowWindowCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow ??= _mainWindow;
            }
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Show();
            _mainWindow.BringIntoView();
            _mainWindow.Focus();

        }


        public void ExitCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }
        }
    }
}
