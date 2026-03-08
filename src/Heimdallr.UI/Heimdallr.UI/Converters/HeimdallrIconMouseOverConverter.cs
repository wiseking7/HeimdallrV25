using System.Globalization;
using System.Windows.Media;

namespace Heimdallr.UI.Converters;
// <summary>
/// MultiBinding Converter: IsMouseOver가 True이면 MouseOverFill, 아니면 Fill 반환
/// </summary>
public class HeimdallrIconMouseOverConverter : BaseMultiValueConverter<HeimdallrIconMouseOverConverter>
{
  public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length < 3)
      return values[0] ?? Brushes.Silver;

    var normalFill = values[0] as Brush ?? Brushes.Silver;
    var mouseOverFill = values[1] as Brush ?? normalFill;
    var isMouseOver = values[2] as bool? ?? false;

    return isMouseOver ? mouseOverFill : normalFill;
  }

  public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    => throw new NotImplementedException();
}
