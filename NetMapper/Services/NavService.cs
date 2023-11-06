using System;
using System.Collections.Concurrent;
using NetMapper.Services.Interfaces;
using NetMapper.ViewModels;
using Splat;

namespace NetMapper.Services;

public class NavService : INavService
{
    private readonly ConcurrentDictionary<Type, ViewModelBase> viewModelDictionary = new();

    private Action<ViewModelBase>? NavigateAction;

    public void SetContentCallback(Action<ViewModelBase> action)
    {
        NavigateAction = action;
    }

    public void AddViewModel(ViewModelBase viewModel)
    {
        if (!viewModelDictionary.TryAdd(viewModel.GetType(), viewModel))
            throw new InvalidOperationException($"{ToString}: Duplicate ViewModel: {viewModel.GetType()}.");
    }

    public T GetViewModel<T>() where T : ViewModelBase
    {
        if (viewModelDictionary.TryGetValue(typeof(T), out var viewModel))
            return viewModel as T ??
                   throw new ArgumentNullException(
                       $"{ToString}: Type mismatch: {viewModel.GetType()} should be {nameof(T)}.");

        throw new InvalidOperationException($"{ToString}: Can not find {nameof(T)} in dictionary.");
    }

    public void GoTo<T>() where T : ViewModelBase
    {
        if (NavigateAction == null)
            throw new NullReferenceException(
                $"{ToString}: Can not navigate to {nameof(T)}. \"Navigate\" property null.");

        NavigateAction.Invoke(GetViewModel<T>());
    }

    public T GoToNew<T>() where T : ViewModelBase
    {
        if (NavigateAction == null)
            throw new NullReferenceException(
                $"{ToString}: Can not navigate to {nameof(T)}. \"Navigate\" property null.");

        try
        {
            GetViewModel<T>(); // should throw if vm does not exist in dict
            // vm exists, replace
            viewModelDictionary[typeof(T)] = Locator.Current.CreateWithConstructorInjection<T>();
        }
        catch (InvalidOperationException) // vm does not exist
        {
            AddViewModel(Locator.Current.CreateWithConstructorInjection<T>());
        }

        NavigateAction?.Invoke(GetViewModel<T>());
        return GetViewModel<T>();
    }

    public void GoTo(ViewModelBase newViewModel)
    {
        if (viewModelDictionary.TryAdd(newViewModel.GetType(), newViewModel))
        {
            NavigateAction?.Invoke(newViewModel);
            return;
        }

        // viewModel exists in dict, replace
        viewModelDictionary[newViewModel.GetType()] = newViewModel;
        NavigateAction?.Invoke(newViewModel);
    }
}