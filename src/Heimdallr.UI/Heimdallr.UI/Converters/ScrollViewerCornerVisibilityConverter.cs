using System.Globalization;
using System.Windows;

namespace Heimdallr.UI.Converters;

public class ScrollViewerCornerVisibilityConverter : BaseMultiValueConverter<ScrollViewerCornerVisibilityConverter>
{
  public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 2)
      return Visibility.Collapsed;

    var vertical = values[0] as Visibility?;
    var horizontal = values[1] as Visibility?;

    return (vertical == Visibility.Visible && horizontal == Visibility.Visible)
      ? Visibility.Visible
      : Visibility.Collapsed;
  }

  public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    => throw new NotSupportedException();
}
