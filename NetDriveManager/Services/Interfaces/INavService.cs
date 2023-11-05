﻿using System;
using NetMapper.ViewModels;

namespace NetMapper.Services.Interfaces;

public interface INavService
{
    void AddViewModel(ViewModelBase viewModel);
    T GetViewModel<T>() where T : ViewModelBase;
    void GoTo(ViewModelBase newViewModel);
    void GoTo<T>() where T : ViewModelBase;
    T GoToNew<T>() where T : ViewModelBase;
    void SetContentCallback(Action<ViewModelBase> action);
}