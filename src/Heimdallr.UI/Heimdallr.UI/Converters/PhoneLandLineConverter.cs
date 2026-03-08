using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

/// <summary>
/// 대한민국 일반전화 하이픈 Converter
/// (02, 031~ , 070 포함)
/// </summary>
public class PhoneLandLineConverter : BaseValueConverter<PhoneLandLineConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string input = Regex.Replace(value.ToString()!, @"[^\d]", "");
    if (string.IsNullOrEmpty(input)) return string.Empty;

    // 서울 번호 (02)
    if (input.StartsWith("02"))
    {
      string rest = input.Substring(2);

      // 7자리까지만 입력 허용
      if (rest.Length > 8) rest = rest.Substring(0, 8);

      if (rest.Length <= 3)
        return $"02-{rest}";
      if (rest.Length <= 4)
        return $"02-{rest.Substring(0, rest.Length - 2)}-{rest.Substring(rest.Length - 2)}";
      if (rest.Length <= 7)
        return $"02-{rest.Substring(0, rest.Length - 4)}-{rest.Substring(rest.Length - 4)}";
      return $"02-{rest.Substring(0, 4)}-{rest.Substring(4)}"; // 8자리
    }

    // 070 인터넷전화
    if (input.StartsWith("070"))
    {
      string rest = input.Substring(3);
      if (rest.Length > 8) rest = rest.Substring(0, 8);

      if (rest.Length <= 4)
        return $"070-{rest}";
      return $"070-{rest.Substring(0, 4)}-{rest.Substring(4)}"; // 실시간 하이픈
    }

    // 기타 3자리 지역번호 (031, 032 등)
    if (input.Length >= 3)
    {
      string area = input.Substring(0, 3);
      string rest = input.Substring(3);
      if (rest.Length > 8) rest = rest.Substring(0, 8);

      if (rest.Length <= 3)
        return $"{area}-{rest}";
      if (rest.Length <= 4)
        return $"{area}-{rest.Substring(0, rest.Length - 2)}-{rest.Substring(rest.Length - 2)}";
      if (rest.Length <= 7)
        return $"{area}-{rest.Substring(0, rest.Length - 4)}-{rest.Substring(rest.Length - 4)}";
      return $"{area}-{rest.Substring(0, 4)}-{rest.Substring(4)}"; // 8자리
    }

    return input;
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string digits = Regex.Replace(value.ToString()!, @"[^\d]", "");

    // 최대 길이 제한
    if (digits.StartsWith("02"))
      digits = digits.Length > 10 ? digits.Substring(0, 10) : digits; // 02 + 8자리
    else if (digits.StartsWith("070"))
      digits = digits.Length > 11 ? digits.Substring(0, 11) : digits; // 070 + 8자리
    else if (digits.Length >= 3)
      digits = digits.Length > 11 ? digits.Substring(0, 11) : digits; // 3자리 + 8자리

    return digits;
  }
}










