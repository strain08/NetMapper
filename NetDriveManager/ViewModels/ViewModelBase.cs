using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Services.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetMapper.ViewModels
{
    public class ViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        private static readonly string[] NO_ERRORS = Array.Empty<string>();
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errorsByPropertyName.Count > 0;

        public virtual IEnumerable GetErrors(string? propertyName)
        {
            if (_errorsByPropertyName.TryGetValue(propertyName!, out var errorList))
            {
                return errorList;
            }
            return NO_ERRORS;

        }

        protected void AddError(string propertyName, string error)
        {
            if (_errorsByPropertyName.TryGetValue(propertyName, out var errorList))
            {
                if (!errorList.Contains(error))
                {
                    errorList.Add(error);
                }
            }
            else
            {
                _errorsByPropertyName.Add(propertyName, new List<string> { error });
            }
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void RemoveError(string propertyName)
        {

            _errorsByPropertyName.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        }

        protected bool ValidateNetworkPath(string propertyName, string? value, int maxLength = 255)
        {
            if (string.IsNullOrEmpty(value))
            {
                AddError(propertyName, $"Path must not be empty.");
                return false;
            }
           
            if (value.Length > maxLength)
            {
                AddError(propertyName, $"Path must be at most {maxLength} characters long.");
                return false;
            }
            if (!Utility.IsNetworkPath(value))
            {
                AddError(propertyName, $"Not a valid network path.");
                return false;
            }
            RemoveError(propertyName);
            return true;
        }
    }
}