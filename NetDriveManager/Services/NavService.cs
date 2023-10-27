using NetMapper.ViewModels;
using System;
using System.Collections.Generic;

namespace NetMapper.Services
{
    public class NavService
    {
        private readonly Dictionary<Type, ViewModelBase> viewModelDictionary = new();

        private Action<ViewModelBase>? Navigate;

        public NavService()  { }

        public void SetNavigateCallback(Action<ViewModelBase> action)
        {
            Navigate = action;
        }

        public void AddViewModel(ViewModelBase viewModel)
        {
            try
            {
                viewModelDictionary.Add(viewModel.GetType(), viewModel);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"{ToString}: Duplicate ViewModel: {viewModel.GetType()}.");
            }
        }

        public T GetViewModel<T>() where T : ViewModelBase
        {

            if (viewModelDictionary.TryGetValue(typeof(T), out var viewModel))
            {
                return viewModel as T ?? 
                    throw new ArgumentNullException($"{ToString}: Type mismatch: {viewModel.GetType()} should be {nameof(T)}.");
            }

            throw new InvalidOperationException($"{ToString}: Can not find {nameof(T)} in dictionary.");

        }

        public void GoTo<T>() where T : ViewModelBase
        {
            if (Navigate == null) 
                throw new NullReferenceException($"{ToString}: Can not navigate to {nameof(T)}. \"Navigate\" property null.");

            Navigate.Invoke(GetViewModel<T>());
        }

        public void GoTo(ViewModelBase newViewModel)
        {
            if (viewModelDictionary.TryAdd(newViewModel.GetType(), newViewModel))
            {
                Navigate?.Invoke(newViewModel);
                return;
            }
            // viewModel exists in dict, replace
            viewModelDictionary[newViewModel.GetType()] = newViewModel;
            Navigate?.Invoke(newViewModel);
        }


    }

}
