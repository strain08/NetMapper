﻿using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using NetMapper.Enums;

namespace NetMapper.Converters;

public class StateToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ShareState shareState && targetType.IsAssignableTo(typeof(string)))
            switch (shareState)
            {
                case ShareState.Available:
                    return "Available";
                case ShareState.Unavailable:
                    return "Unavailable";
                case ShareState.Undefined:
                    return "Updating..";
            }

        if (value is MappingState mappingState && targetType.IsAssignableTo(typeof(string)))
            switch (mappingState)
            {
                case MappingState.Mapped:
                    return "Mapped";
                case MappingState.Unmapped:
                    return "Unmapped";
                case MappingState.LetterUnavailable:
                    return "Letter unavailable";
                case MappingState.Undefined:
                    return "Updating..";
            }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}