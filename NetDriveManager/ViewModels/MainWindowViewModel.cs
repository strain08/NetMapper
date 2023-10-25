using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Services;
using NetMapper.Services.Static;
using Serilog.Data;
using Splat;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetMapper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        ViewModelBase? content;

        public MainWindowViewModel()
        {
            var nav = Locator.Current.GetRequiredService<NavService>();
            nav.Navigate = (vm) => 
            { 
                Content = vm; 
            };
           
            nav.GoTo(new DriveListViewModel());
            
        }
    }
}