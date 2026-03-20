using System.Globalization;

namespace TaskManager.AppUI.Converter;
/// <summary>
/// Converts integer progress values from 0 to 100 into values from 0.0 to 1.0
/// </summary>
public class ProgressLineConv : IValueConverter
{
    /// <summary>
    /// Converts an integer progress value into a normalized double value.
    /// </summary>
    /// <param name="value">The source progress value.</param>
    /// <param name="targetType">The target binding type.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture information.</param>
    /// <returns>A normalized progress value in the range from 0.0 to 1.0.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int progress)
            return progress / 100.0;

        return 0.0;
    }

    /// <summary>
    /// Converts a normalized value back to the source type.
    /// </summary>
    /// <param name="value">The value produced by the binding target.</param>
    /// <param name="targetType">The target type for the source property.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture information.</param>
    /// <returns>This method does not return a value because reverse conversion is not supported.</returns>
    /// <exception cref="NotImplementedException">
    /// Thrown because reverse conversion is not implemented.
    /// </exception>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}