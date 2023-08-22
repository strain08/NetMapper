using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections;

namespace NetDriveManager.ViewModels
{
    public class ViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        private static readonly string[] NO_ERRORS = Array.Empty<string>();
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        
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
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        protected bool ValidateString(string propertyName, string? value, int minLength = 1, int maxLength = 1000)
        {
            if (string.IsNullOrEmpty(value))
            {
                //AddError(propertyName, $"The text must not be empty.");
                return false;
            }
            if (value.Length < minLength)
            {
                AddError(propertyName, $"The text must be at least {minLength} characters long.");
                return false;
            }
            if (value.Length > maxLength)
            {
                AddError(propertyName, $"The text must be at most {maxLength} characters long.");
                return false;
            }
            RemoveError(propertyName);
            return true;
        }
    }
}