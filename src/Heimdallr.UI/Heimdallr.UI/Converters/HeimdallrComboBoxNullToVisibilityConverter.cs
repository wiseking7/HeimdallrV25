using System.Globalization;
using System.Windows;

namespace Heimdallr.UI.Converters;

public class HeimdallrComboBoxNullToVisibilityConverter : BaseValueConverter<HeimdallrComboBoxNullToVisibilityConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null ? Visibility.Visible : Visibility.Collapsed;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    => throw new NotImplementedException();
}
