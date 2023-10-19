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
        public MainWindow MainWindowView;
        readonly SettingsService settingsService;
        readonly DriveListService listService;
        [ObservableProperty]
        string tooltipText = string.Empty;

        public ApplicationViewModel()     
        {
            if (Design.IsDesignMode) return; 
            settingsService = Locator.Current.GetRequiredService<SettingsService>();
            settingsService.AddSetting(new SetRunAtStartup());
            settingsService.AddSetting(new SetMinimizeTaskbar());

            MainWindowView = new()
            {
                DataContext = VMServices.MainWindowViewModel = new MainWindowViewModel()
            };
            settingsService.AddSetting(new SetMainWindow(MainWindowView));

            settingsService.ApplyAll();

            listService = Locator.Current.GetRequiredService<DriveListService>();
            listService.ModelPropertiesUpdated = UpdateTooltip;

            VMServices.ApplicationViewModel = this;
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
            App.AppContext((application) =>
            {
                application.MainWindow ??= MainWindowView;
                application.MainWindow.WindowState = WindowState.Normal;
                application.MainWindow.Show();
                application.MainWindow.BringIntoView();
                application.MainWindow.Focus();
            });
        }
        
        // systray menu
        public static void ExitCommand()
        {
            App.AppContext((application) =>
            {
                application.Shutdown();                
            });
        }
    }
}
