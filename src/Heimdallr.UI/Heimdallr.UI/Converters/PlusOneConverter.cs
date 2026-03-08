using System.Globalization;
using System.Windows.Data;

namespace Heimdallr.UI.Converters;

/// <summary>
/// ListBox, ListView, DataGrid, ItemsControl 에서 1부터 시작하는 컨버트
/// </summary>
public class PlusOneConverter : BaseValueConverter<PlusOneConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is int index)
      return index + 1;

    return value;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
   => Binding.DoNothing;
}
