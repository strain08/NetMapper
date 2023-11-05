using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using NetMapper.Enums;

namespace NetMapper.Converters;

public class StateToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ShareState shareState && targetType.IsAssignableTo(typeof(IBrush)))
            return shareState switch
            {
                ShareState.Available => new SolidColorBrush { Color = Colors.DarkGreen },
                ShareState.Unavailable => new SolidColorBrush { Color = Colors.Chocolate },
                _ => new SolidColorBrush { Color = Colors.Chocolate }
            };
        if (value is MappingState mappingState && targetType.IsAssignableTo(typeof(IBrush)))
            return mappingState switch
            {
                MappingState.Mapped => new SolidColorBrush { Color = Colors.DarkGreen },
                MappingState.Unmapped => new SolidColorBrush { Color = Colors.Chocolate },
                MappingState.LetterUnavailable => new SolidColorBrush { Color = Colors.DarkRed },
                _ => new SolidColorBrush { Color = Colors.Chocolate }
            };
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}