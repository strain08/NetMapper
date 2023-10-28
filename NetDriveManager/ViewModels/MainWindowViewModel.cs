using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Services;
using NetMapper.Services.Helpers;
using Splat;

namespace NetMapper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        ViewModelBase? content;

        public string Title => AppUtil.GetAppName();

        public MainWindowViewModel()
        {
            var nav = Locator.Current.GetRequiredService<NavService>();
            nav.SetNavigateCallback((vm) =>
            {
                Content = vm;
            });

            nav.GoTo(new DriveListViewModel());

        }
    }
}