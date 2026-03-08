using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

public class NumericWithDecimalConverter : BaseValueConverter<NumericWithDecimalConverter>
{
  private static readonly Regex _nonNumericRegex =
      new Regex(@"[^0-9.]", RegexOptions.Compiled);

  private static readonly Regex _multiDotRegex =
      new Regex(@"\.(?=.*\.)", RegexOptions.Compiled);

  // VM → View (표시용)
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
      return string.Empty;

    if (!decimal.TryParse(value.ToString(),
        NumberStyles.Any,
        CultureInfo.InvariantCulture,
        out var number))
      return string.Empty;

    // 항상 천단위 + 소수점 2자리
    return number.ToString("#,##0.00", culture);
  }

  // View → VM (저장용)
  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string text = value.ToString()!;
    if (string.IsNullOrWhiteSpace(text)) return string.Empty;

    // 콤마 제거 + 숫자/점만 허용
    text = text.Replace(",", "");
    text = _nonNumericRegex.Replace(text, "");
    text = _multiDotRegex.Replace(text, "");

    // 소수점 2자리 제한
    if (text.Contains('.'))
    {
      int dotIndex = text.IndexOf('.');
      if (text.Length > dotIndex + 3)
        text = text.Substring(0, dotIndex + 3);
    }

    // 중간 입력 상태 허용 (예: "12.", "12.3")
    if (text.EndsWith(".") || text.EndsWith(".0"))
      return text;

    if (!decimal.TryParse(text,
        NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture,
        out var number))
      return string.Empty;

    // VM에는 콤마 없는 숫자만
    return number.ToString("0.##", CultureInfo.InvariantCulture);
  }
}





