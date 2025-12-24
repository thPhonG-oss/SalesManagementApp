using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace SalesManagement.WinUI.Converters;

public class BooleanConverters : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isVisible = value is bool boolValue && boolValue;
        bool isInverse = parameter?.ToString()?.ToLower() == "inverse";

        if (isInverse)
        {
            isVisible = !isVisible;
        }

        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class BoolNegationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return !(value is bool boolValue && boolValue);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return !(value is bool boolValue && boolValue);
    }
}