using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Attributes;
using NetMapper.Interfaces;
using NetMapper.Services.Static;

namespace NetMapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavService nav;

    [ObservableProperty] private ViewModelBase? content;
#nullable disable
    public MainWindowViewModel()
    {
    }
#nullable restore
    [ResolveThis]
    public MainWindowViewModel(INavService navService)
    {
        nav = navService;

        nav.SetContentCallback(vm => { Content = vm; });

        nav.GoToNew<DriveListViewModel>();
    }

    public string Title => AppUtil.GetAppName();
}