using NetMapper.Extensions;
using NetMapper.Interfaces;
using NetMapper.ViewModels;
using Splat;
using System;
using System.Collections.Concurrent;

namespace NetMapper.Services;

public class NavService : INavService
{
    private readonly ConcurrentDictionary<Type, ViewModelBase> viewModelDictionary = new();
    private readonly IReadonlyDependencyResolver r;
    private Action<ViewModelBase>? NavigateAction;

    public NavService(IReadonlyDependencyResolver r)
    {
        this.r = r;
    }
    public void SetContentCallback(Action<ViewModelBase> action)
    {
        NavigateAction = action;
    }

    public void AddViewModel(ViewModelBase viewModel)
    {
        if (!viewModelDictionary.TryAdd(viewModel.GetType(), viewModel))
            throw new InvalidOperationException($"{ToString}: Duplicate ViewModel: {viewModel.GetType()}.");
    }
    /// <summary>
    /// Retrieves a ViewModel from dictionary.
    /// <br>If it does not exist, creates a new one.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="addToDict">If true, also add view model to dictionary</param>
    /// <returns>ViewModel</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public T GetViewModel<T>(bool addToDict = false) where T : ViewModelBase
    {
        if (viewModelDictionary.TryGetValue(typeof(T), out var viewModel))
            return viewModel as T ??
                   throw new ArgumentNullException(
                       $"{ToString}: Type mismatch: {viewModel.GetType()} should be {nameof(T)}.");
        
        T t = r.CreateWithConstructorInjection<T>();
        
        if (addToDict)
        {            
            AddViewModel(t);
        }
        
        return t;       
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

        T t = GetViewModel<T>();

        NavigateAction?.Invoke(t);
        return t;
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