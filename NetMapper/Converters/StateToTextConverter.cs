using Avalonia.Data;
using Avalonia.Data.Converters;
using NetMapper.Enums;
using System;
using System.Globalization;

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

        if (value is MapState mappingState && targetType.IsAssignableTo(typeof(string)))
            switch (mappingState)
            {
                case MapState.Mapped:
                    return "Mapped";
                case MapState.Unmapped:
                    return "Unmapped";
                case MapState.LetterUnavailable:
                    return "Letter unavailable";
                case MapState.Undefined:
                    return "Updating..";
            }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}