using System.Globalization;

namespace TaskManager.AppUI.Converter;
/// <summary>
/// converts integer progress values from 0 to 100 into values from 0.0 to 1.0
/// </summary>
public class ProgressLineConv : IValueConverter
{
    /// <summary>
    /// converts an integer progress value into a double
    /// </summary>
    /// <param name="value">source progress val</param>
    /// <param name="targetType">target binding type</param>
    /// <param name="parameter">converter param</param>
    /// <param name="culture">culture info</param>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int progress)
            return progress / 100.0;

        return 0.0;
    }

    /// <summary>
    /// converts a value back to the source type
    /// </summary>
    /// <param name="value">val produced by the binding target</param>
    /// <param name="targetType">target type for the source property</param>
    /// <param name="parameter">converter param</param>
    /// <param name="culture">culture info</param>
    /// <exception cref="NotImplementedException">
    /// thrown because reverse conversion is not implemented
    /// </exception>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}