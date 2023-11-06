using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Services.Helpers;
using NetMapper.Services.Interfaces;

namespace NetMapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavService nav;

    [ObservableProperty] private ViewModelBase? content;

    public MainWindowViewModel()
    {
    }

    [ResolveThis]
    public MainWindowViewModel(INavService navService)
    {
        nav = navService;

        nav.SetContentCallback(vm => { Content = vm; });

        nav.GoToNew<DriveListViewModel>();
    }

    public string Title => AppUtil.GetAppName();
}