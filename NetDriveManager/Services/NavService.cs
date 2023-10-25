using NetMapper.ViewModels;
using System;
using System.Collections.Generic;

namespace NetMapper.Services
{
    internal class NavService
    {
        private readonly List<ViewModelBase> viewModelList;

        public Action<ViewModelBase>? Navigate;

        public NavService()
        {
            viewModelList = new();
        }

        public void AddViewModel(ViewModelBase viewModel)
        {
            if (GetViewModel(viewModel.GetType()) == null)
                viewModelList.Add(viewModel);
        }

        public ViewModelBase? GetViewModel(Type type) => viewModelList.Find((vm) => vm.GetType() == type);

        private void RemoveViewModel(Type type)
        {
            var i = viewModelList.FindIndex((vm) => vm.GetType() == type);
            viewModelList.RemoveAt(i);
        }

        public void GoTo(Type t)
        {
            var vm = GetViewModel(t);
            if (vm == null) throw new InvalidOperationException($"Can not navigate. Viewmodel {t.Name} not present in list.");
            Navigate?.Invoke(vm);
        }

        public void GoTo(ViewModelBase newViewModel)
        {
            var existingVM = GetViewModel(newViewModel.GetType());
            if (existingVM != null)
            {
                RemoveViewModel(existingVM.GetType());
            }
            AddViewModel(newViewModel);
            Navigate?.Invoke(newViewModel);
        }
    }

}
