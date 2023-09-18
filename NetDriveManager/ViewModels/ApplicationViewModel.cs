using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using NetDriveManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDriveManager.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetDriveManager.ViewModels
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
                ((Window)s).Hide();
                e.Cancel = true;
            };
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

            VMServices.mainWindow = _mainWindow;

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


        public static void ExitCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }
        }
    }
}
