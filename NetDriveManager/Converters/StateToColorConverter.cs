using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using NetMapper.Enums;
using System;
using System.Globalization;

namespace NetMapper.Converters
{
    public class StateToColorConverter : IValueConverter
    {

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ShareState shareState && targetType.IsAssignableTo(typeof(IBrush)))
            {
                switch (shareState)
                {
                    case ShareState.Available:
                        return new SolidColorBrush() { Color = Colors.DarkGreen };
                    case ShareState.Unavailable:
                        return new SolidColorBrush() { Color = Colors.Chocolate };
                    default:
                        return new SolidColorBrush() { Color = Colors.Chocolate };
                }
            }
            if (value is MappingState mappingState && targetType.IsAssignableTo(typeof(IBrush)))
            {
                switch (mappingState)
                {
                    case MappingState.Mapped:
                        return new SolidColorBrush() { Color = Colors.DarkGreen };
                    case MappingState.Unmapped:
                        return new SolidColorBrush() { Color = Colors.Chocolate };
                    default:
                        return new SolidColorBrush() { Color = Colors.Chocolate };
                }
            }
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
