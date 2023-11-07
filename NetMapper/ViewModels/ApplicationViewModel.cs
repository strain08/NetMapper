using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Attributes;
using NetMapper.Enums;
using NetMapper.Messages;
using NetMapper.Services;
using NetMapper.Services.Interfaces;
using NetMapper.Services.Settings;
using NetMapper.Views;
using Splat;

namespace NetMapper.ViewModels;

public partial class ApplicationViewModel : ViewModelBase, IRecipient<PropertyChangedMessage>
{
    private readonly IDriveListService listService;
    private readonly INavService nav;

    private readonly ISettingsService settings;
    public MainWindow? MainWindowView;

    [ObservableProperty] 
    private string tooltipText = string.Empty;

#nullable disable
    public ApplicationViewModel() { } // preview does not work w/o empty ctor
#nullable enable

    [ResolveThis]
    public ApplicationViewModel(
        ISettingsService settingsService,
        IDriveListService driveListService,
        INavService navService)
    {
        
        nav = navService;
        settings = settingsService;
        listService = driveListService;

        InitializeApp();
    }

    public void Receive(PropertyChangedMessage message)
    {
        UpdateTooltip();
    }

    private void InitializeApp()
    {
        WeakReferenceMessenger.Default.Register(this);

        settings.AddModule(new SetRunAtStartup());
        settings.AddModule(new SetMinimizeTaskbar());

        MainWindowView = new MainWindow
        {
            DataContext = Locator.Current.CreateWithConstructorInjection<MainWindowViewModel>()
        };
        settings.AddModule(new SetMainWindow(MainWindowView));

        settings.ApplyAll();

        nav.AddViewModel(this);
    }

    private void UpdateTooltip()
    {
        TooltipText = string.Empty;

        foreach (var item in listService.DriveCollection)
        {
            TooltipText += item.DriveLetterColon;
            switch (item.MappingStateProp)
            {
                case MappingState.Mapped:
                    TooltipText += " connected.";
                    break;
                case MappingState.Unmapped:
                    TooltipText += " disconnected.";
                    break;
                case MappingState.LetterUnavailable:
                    TooltipText += " letter unavailable.";
                    break;
                default:
                    TooltipText += " updating...";
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
    public void ExitCommand()
    {
        App.AppContext(application => { application.Shutdown(); });
    }

    public void ShowMainWindow()
    {
        App.AppContext(application =>
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