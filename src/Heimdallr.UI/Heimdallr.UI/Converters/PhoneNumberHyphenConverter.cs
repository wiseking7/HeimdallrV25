using System.Globalization;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Converters;

/// <summary>
/// 대한민국 전화번호 실시간 하이픈 처리용 WPF ValueConverter
/// ✔ 입력 중 Caret 멈춤 현상 완전 방지
/// ✔ ViewModel에는 숫자만 저장
/// ✔ 정확한 자리 수 도달 시에만 최종 포맷 적용
/// </summary>
public class PhoneNumberHyphenConverter : BaseValueConverter<PhoneNumberHyphenConverter>
{
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return string.Empty;

    // 숫자만 추출
    string input = Regex.Replace(value.ToString()!, @"[^\d]", "");

    if (string.IsNullOrEmpty(input))
      return string.Empty;

    /* ===============================
     * 서울 번호 (02)
     * 총 길이: 9 or 10
     * =============================== */
    if (input.StartsWith("02"))
    {
      if (input.Length <= 2)
        return input;

      // 입력 중: 02-XXXX...
      if (input.Length < 10)
        return $"02-{input.Substring(2)}";

      // ✔ 정확히 10자리 → 최종 포맷
      if (input.Length == 10)
        return $"02-{input.Substring(2, 4)}-{input.Substring(6, 4)}";

      // 초과 입력 방지 (그대로 반환)
      return input;
    }

    /* ===============================
     * 휴대폰 (010)
     * 총 길이: 11
     * =============================== */
    if (input.StartsWith("010"))
    {
      if (input.Length <= 3)
        return input;

      if (input.Length < 11)
        return $"010-{input.Substring(3)}";

      if (input.Length == 11)
        return $"010-{input.Substring(3, 4)}-{input.Substring(7, 4)}";

      return input;
    }

    /* ===============================
     * 기타 이동통신 (011, 016~019)
     * 총 길이: 10
     * =============================== */
    if (Regex.IsMatch(input, @"^(011|016|017|018|019)"))
    {
      if (input.Length <= 3)
        return input;

      if (input.Length < 10)
        return $"{input.Substring(0, 3)}-{input.Substring(3)}";

      if (input.Length == 10)
        return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6, 4)}";

      return input;
    }

    /* ===============================
     * 인터넷 전화 (070)
     * 총 길이: 11
     * =============================== */
    if (input.StartsWith("070"))
    {
      if (input.Length <= 3)
        return input;

      if (input.Length < 11)
        return $"070-{input.Substring(3)}";

      if (input.Length == 11)
        return $"070-{input.Substring(3, 4)}-{input.Substring(7, 4)}";

      return input;
    }

    /* ===============================
     * 기타 지역 번호 (031, 032 등)
     * 총 길이: 10
     * =============================== */
    if (input.Length >= 3)
    {
      if (input.Length <= 3)
        return input;

      if (input.Length < 10)
        return $"{input.Substring(0, 3)}-{input.Substring(3)}";

      if (input.Length == 10)
        return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6, 4)}";

      return input;
    }

    return input;
  }

  /// <summary>
  /// ViewModel에는 숫자만 저장
  /// </summary>
  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return string.Empty;

    return Regex.Replace(value.ToString()!, @"[^\d]", "");
  }
}







