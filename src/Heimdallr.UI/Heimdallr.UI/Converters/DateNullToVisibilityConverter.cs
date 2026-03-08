using System.Globalization;
using System.Windows;

namespace Heimdallr.UI.Converters;

/// <summary>
/// HeimdallrDatePacker 컨버터
/// </summary>
public class DateNullToVisibilityConverter : BaseValueConverter<DateNullToVisibilityConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    bool invert = (parameter as string)?.Equals("invert", StringComparison.OrdinalIgnoreCase) == true;

    bool isNull = value == null;
    if (value is DateTime dt && dt == DateTime.MinValue)
      isNull = true;

    if (invert)
      return isNull ? Visibility.Collapsed : Visibility.Visible;
    else
      return isNull ? Visibility.Visible : Visibility.Collapsed;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    => throw new NotImplementedException();
}
