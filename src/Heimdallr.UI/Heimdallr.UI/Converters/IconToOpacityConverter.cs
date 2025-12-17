using Heimdallr.UI.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Heimdallr.UI.Converters;

public class IconToOpacityConverter : BaseValueConverter<IconToOpacityConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null || value.Equals(IconType.None) ? 0.0 : 1.0;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    => Binding.DoNothing;
}
