using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using NetDriveManager.Enums;
using System;
using System.Globalization;

namespace NetDriveManager.Converters
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
                        return new SolidColorBrush() { Color = Colors.Transparent };
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
                        return new SolidColorBrush() { Color = Colors.Transparent };
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
