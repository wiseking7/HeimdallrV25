using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

public class NumericWithThousandsConverter : BaseValueConverter<NumericWithThousandsConverter>
{
  private static readonly Regex _commaRegex = new Regex(",", RegexOptions.Compiled);
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return string.Empty;

    // 숫자 파싱 시도
    if (!decimal.TryParse(value.ToString(), out decimal number))
      return value.ToString()!;

    // 천 단위 콤마 포맷
    return number.ToString("#,0", culture);
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return string.Empty;

    string text = value.ToString()!;

    // 콤마만 제거
    string raw = _commaRegex.Replace(text, "");

    return raw;
  }
}
