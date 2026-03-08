using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

public class NumberToCommaWithDecimalConverter : BaseValueConverter<NumberToCommaWithDecimalConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string? numeric = value.ToString();

    if (string.IsNullOrEmpty(numeric)) return string.Empty;

    // 소수점 및 숫자만 남기도록 정리
    numeric = Regex.Replace(numeric, @"[^\d.]", "");  // 숫자와 소수점만 남김

    string integerPart = numeric;
    string decimalPart = string.Empty;

    // 소수점 처리
    int dotIndex = numeric.IndexOf('.');
    if (dotIndex >= 0)
    {
      integerPart = numeric.Substring(0, dotIndex);  // 소수점 앞 부분
      decimalPart = numeric.Substring(dotIndex);     // 소수점 및 뒤의 숫자
    }

    // 천 단위 콤마 추가
    if (long.TryParse(integerPart, out long lng))
      integerPart = lng.ToString("N0", culture);  // 정수 부분에 천 단위 콤마 추가

    return integerPart + decimalPart;  // 소수점 포함 문자열 반환
  }

  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string? input = value.ToString();

    // 숫자 및 소수점만 허용 (숫자 및 소수점 외의 문자는 제거)
    string? numericOnly = Regex.Replace(input!, @"[^\d.]", "");

    // 소수점이 두 번 이상 있을 경우 처리
    int dotCount = numericOnly.Count(c => c == '.');
    if (dotCount > 1)
    {
      // 소수점이 두 개 이상일 경우, 마지막 소수점 이후의 값을 제거
      int lastDotIndex = numericOnly.LastIndexOf('.');
      numericOnly = numericOnly.Substring(0, lastDotIndex + 1) + numericOnly.Substring(lastDotIndex + 1).Replace(".", string.Empty);
    }

    // 유효한 숫자값으로 변환
    if (decimal.TryParse(numericOnly, out decimal result))
    {
      return result.ToString("G", culture);  // 다시 원래 숫자로 반환 (소수점 포함한 포맷)
    }

    return string.Empty; // 파싱 실패 시 빈 문자열 반환
  }
}



