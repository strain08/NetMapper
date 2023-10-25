using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Services.Static;
using NetMapper.Views;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace NetMapper.ViewModels
{
    public partial class ApplicationViewModel : ViewModelBase
    {
        public MainWindow? MainWindowView;

        readonly ISettingsService settings;
        readonly IDriveListService listService;
        [ObservableProperty]
        string tooltipText = string.Empty;
        public ApplicationViewModel() { }

        public ApplicationViewModel(ISettingsService settingsService,IDriveListService driveListService)
        {
            //if (Design.IsDesignMode) return; 
            settings = settingsService;
            listService = driveListService;
            InitializeApp();
        }

        private void InitializeApp()
        {
            settings.AddModule(new SetRunAtStartup());
            settings.AddModule(new SetMinimizeTaskbar());

            MainWindowView = new()
            {
                DataContext = new MainWindowViewModel()
            };
            settings.AddModule(new SetMainWindow(MainWindowView));

            settings.ApplyAll();

            listService.ModelPropertiesUpdated = UpdateTooltip;

            //VMServices.ApplicationViewModel = this;
        }

        private void UpdateTooltip()
        {
            TooltipText = string.Empty;

            foreach (var item in listService.DriveList) 
            {
                TooltipText += item.DriveLetterColon;
                switch (item.MappingStateProp)
                {
                    case Enums.MappingState.Mapped:
                        TooltipText += " connected.";
                        break;
                    case Enums.MappingState.Unmapped:
                        TooltipText += " disconnected.";
                        break;
                    case Enums.MappingState.LetterUnavailable:
                        TooltipText += " letter unavailable.";
                        break;
                }

                TooltipText += "\n";
            }     
        }
        
        // systray menu
        public void ShowWindowCommand()
        {
            ShowMainWindow();
        }

        // systray menu
        public static void ExitCommand()
        {
            App.AppContext((application) =>
            {
                application.Shutdown();                
            });
        }
        protected void ShowMainWindow()
        {
            App.AppContext((application) =>
            {
                if (MainWindowView != null)
                {
                    application.MainWindow ??= MainWindowView;
                    application.MainWindow.WindowState = WindowState.Normal;
                    application.MainWindow.Show();
                    application.MainWindow.BringIntoView();
                    application.MainWindow.Focus();
                }
            });
        }
    }
}
