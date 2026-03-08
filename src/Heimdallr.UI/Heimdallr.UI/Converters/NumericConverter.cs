using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;
public class NumericConverter : BaseValueConverter<NumericConverter>
{
  private static readonly Regex _numberOnlyRegex = new Regex("[^0-9]", RegexOptions.Compiled);
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value?.ToString() ?? string.Empty;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
    {
      return string.Empty;
    }

    string input = value.ToString()!;

    // 숫자 이외 제거 (영문, 한글, 특수문자 모두 제거)
    string numericOnly = _numberOnlyRegex.Replace(input, "");

    return numericOnly;
  }
}

