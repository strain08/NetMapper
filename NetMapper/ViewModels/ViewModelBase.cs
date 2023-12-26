using CommunityToolkit.Mvvm.ComponentModel;
using NetMapper.Extensions;
using NetMapper.Interfaces;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetMapper.ViewModels;

public class ViewModelBase : ObservableObject, INotifyDataErrorInfo
{
    private static readonly string[] NO_ERRORS = Array.Empty<string>();
    private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errorsByPropertyName.Count > 0;

    public virtual IEnumerable GetErrors(string? propertyName)
    {
        if (_errorsByPropertyName.TryGetValue(propertyName!, out var errorList)) return errorList;
        return NO_ERRORS;
    }

    protected void AddError(string propertyName, string error)
    {
        if (_errorsByPropertyName.TryGetValue(propertyName, out var errorList))
        {
            if (errorList.Contains(error) == false)
                errorList.Add(error);
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

    protected void RemoveErrorFromProperty(string propertyName, string error)
    {
        if (_errorsByPropertyName.TryGetValue(propertyName, out var errorList)
            && errorList.Contains(error))
        {
            errorList.Remove(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    protected bool ValidateNetworkPath(string propertyName, string value, int maxLength = 255)
    {

        var pathEmptyError = "Path must not be empty.";
        if (string.IsNullOrEmpty(value))
        {
            AddError(propertyName, pathEmptyError);
            return false;
        }
        else RemoveErrorFromProperty(propertyName, pathEmptyError);


        var maxLengthError = $"Path must be at most {maxLength} characters long.";
        if (value.Length > maxLength)
        {
            AddError(propertyName, maxLengthError);
            return false;
        }
        else RemoveErrorFromProperty(propertyName, maxLengthError);
        
        var netPathError = "Not a valid network path.";        
        if (!Locator.Current.GetRequiredService<IInterop>().IsNetworkPath(value))
        {
            AddError(propertyName, netPathError);
            return false;
        }
        else RemoveErrorFromProperty(propertyName, netPathError);

        RemoveError(propertyName);
        return true;
    }

    protected bool ValidateDriveLetter(string propertyName, char? value)
    {
        var nullLetter = "Drive letter null.";
        if (value is null)
        {
            AddError(propertyName, nullLetter);
            return false;
        }
        else RemoveErrorFromProperty(propertyName, nullLetter);

        var notLetter = "Select a drive letter.";
        char notNullChar = (char)value;
        if (!char.IsLetter(notNullChar))
        {
            AddError(propertyName, notLetter);
            return false;
        }
        else RemoveErrorFromProperty(propertyName, notLetter);

        RemoveError(propertyName);
        return true;
        
    }
}