using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace GodOfWarCSharpSE.Converters;

public class IntToRadioConverter : IValueConverter
{
    public int TargetValue { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int i)
            return i == TargetValue;
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
            return TargetValue;
        return AvaloniaProperty.UnsetValue;
    }
}

