using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

/// <summary>
/// 숫자(int 또는 숫자 문자열)를 천 단위 콤마 형식 문자열로 변환하는 WPF IValueConverter입니다.
/// 예: 12345 → "12,345"
/// - Convert: int 또는 string 값을 "12,345" 형식의 문자열로 변환
/// - ConvertBack: 콤마가 포함된 문자열을 정수(int)로 역변환
/// - 값이 0 또는 null이면 빈 문자열을 반환함
/// </summary>
public class NumberToCommaConverter : BaseValueConverter<NumberToCommaConverter>
{
  /// <summary>
  /// 숫자(int 또는 숫자 문자열)를 천 단위 콤마 문자열로 변환합니다.
  /// </summary>
  /// <param name="value">입력 값 (int 또는 string)</param>
  /// <param name="targetType">변환 대상 형식 (보통 string)</param>
  /// <param name="parameter">추가 파라미터 (사용하지 않음)</param>
  /// <param name="culture">문화권 정보 (콤마/마침표 등 지역 포맷에 영향)</param>
  /// <returns>천 단위 콤마가 포함된 문자열, 또는 빈 문자열</returns>
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null) return string.Empty;

    string? numeric = value.ToString();
    if (string.IsNullOrEmpty(numeric)) return string.Empty;

    // 천 단위 콤마 추가
    if (long.TryParse(numeric, out long lng))
      return lng.ToString("N0", culture);  // 소수점 없이 천 단위 콤마 추가

    return numeric;

  }

  /// <summary>
  /// 콤마가 포함된 문자열을 정수(int)로 역변환합니다.
  /// 예: "12,345" → 12345
  /// </summary>
  /// <param name="value">콤마 포함 문자열</param>
  /// <param name="targetType">변환 대상 타입 (보통 int)</param>
  /// <param name="parameter">추가 파라미터 (사용하지 않음)</param>
  /// <param name="culture">문화권 정보 (사용자 지역 설정)</param>
  /// <returns>정수 값, 또는 null</returns>
  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    // input이 null인 경우를 처리
    if (value == null) return string.Empty;

    string? input = value.ToString();
    // 숫자만 반환 (콤마 제거)
    return string.IsNullOrEmpty(input) ? string.Empty : Regex.Replace(input, @"[^\d]", "");
  }
}
