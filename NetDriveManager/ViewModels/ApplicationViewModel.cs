using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Services;
using NetMapper.Services.Settings;
using NetMapper.Views;
using NetMapper.Messages;
using Splat;

namespace NetMapper.ViewModels
{
    public partial class ApplicationViewModel : ViewModelBase, IRecipient<PropertyChangedMessage>
    {
        public MainWindow? MainWindowView;

        readonly ISettingsService settings;
        readonly IDriveListService listService;
        readonly NavService navService;
        [ObservableProperty]
        string tooltipText = string.Empty;

        public ApplicationViewModel() { }

        public ApplicationViewModel(ISettingsService settingsService, IDriveListService driveListService)
        {
            //if (Design.IsDesignMode) return; 
            navService = Locator.Current.GetRequiredService<NavService>();
            settings = settingsService;
            listService = driveListService;
            WeakReferenceMessenger.Default.Register(this);
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

            //listService.ModelPropertiesUpdated = UpdateTooltip;

            navService.AddViewModel(this);
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
        public void ExitCommand()
        {
            App.AppContext((application) =>
            {
                application.Shutdown();
            });
        }

        public void ShowMainWindow()
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

        public void Receive(PropertyChangedMessage message)
        {
            UpdateTooltip();
        }
    }
}
