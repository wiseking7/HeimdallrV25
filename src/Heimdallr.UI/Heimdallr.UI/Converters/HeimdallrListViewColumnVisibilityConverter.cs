using Heimdallr.UI.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Converters;

public class HeimdallrListViewColumnVisibilityConverter : BaseValueConverter<HeimdallrListViewColumnVisibilityConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    // value는 GridViewColumnHeader, parameter는 해당 Column의 Header 이름
    if (value is GridViewColumnHeader header && parameter is string columnHeaderName)
    {
      // ColumnVisibility 딕셔너리에서 해당 컬럼의 가시성 값을 찾는다.
      var listView = VisualTreeHelper.GetParent(header) as HeimdallrListView;
      if (listView?.ColumnVisibility?.TryGetValue(columnHeaderName, out var isVisible) ?? false)
      {
        return isVisible ? Visibility.Visible : Visibility.Collapsed;
      }
    }
    return Visibility.Collapsed;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    => throw new NotImplementedException();
}
