using System.Globalization;
using System.Windows;

namespace Heimdallr.UI.Converters;

public class ObjectToVisibilityConverter : BaseValueConverter<ObjectToVisibilityConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null ? Visibility.Collapsed : Visibility.Visible;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
