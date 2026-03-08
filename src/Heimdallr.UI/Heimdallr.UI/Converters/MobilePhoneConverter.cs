using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

/// <summary>
/// 대한민국 휴대폰 하이픈 Converter (010, 011~019)
/// ✔ ViewModel에는 숫자만 저장
/// ✔ 정확한 길이 도달 시에만 최종 포맷 적용
/// </summary>
public class MobilePhoneConverter : BaseValueConverter<MobilePhoneConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return string.Empty;

    string input = Regex.Replace(value.ToString()!, @"[^\d]", "");

    if (string.IsNullOrEmpty(input))
      return string.Empty;

    // 010
    if (input.StartsWith("010"))
    {
      if (input.Length <= 3) return input;
      if (input.Length <= 7) return $"{input.Substring(0, 3)}-{input.Substring(3)}";
      return $"{input.Substring(0, 3)}-{input.Substring(3, 4)}-{input.Substring(7, Math.Min(4, input.Length - 7))}";
    }

    // 011, 016~019
    if (Regex.IsMatch(input, @"^(011|016|017|018|019)"))
    {
      if (input.Length <= 3) return input;
      if (input.Length <= 6) return $"{input.Substring(0, 3)}-{input.Substring(3)}";
      return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6, Math.Min(4, input.Length - 6))}";
    }

    return input;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string digits = Regex.Replace(value.ToString()!, @"[^\d]", "");

    // 숫자 길이 제한
    if (digits.StartsWith("010"))
      digits = digits.Length > 11 ? digits.Substring(0, 11) : digits;
    else if (Regex.IsMatch(digits, @"^(011|016|017|018|019)"))
      digits = digits.Length > 10 ? digits.Substring(0, 10) : digits;

    return digits;
  }
}


