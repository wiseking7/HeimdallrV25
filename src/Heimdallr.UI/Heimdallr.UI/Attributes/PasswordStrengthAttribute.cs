using System.ComponentModel.DataAnnotations;

namespace Heimdallr.UI.Attributes;

/// <summary>
/// AttributeTargets.Property: 속성에만 적용됩니다.
/// AttributeTargets.Field: 필드에만 적용됩니다
/// AllowMultiple = false: 한 속성에 대해 이 어트리뷰트를 여러 번 사용할 수 없음을 의미합니다
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PasswordStrengthAttribute : ValidationAttribute
{
  /// <summary>
  /// 최소 8자, 0~9 숫자가 하나 이상 포함, 소문자 알파벳 하나 이상 포함, 대문자 알파벳 하나 이상 포함
  /// 특수 문자 하나 이상 포함(@, #, $, %)
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public override bool IsValid(object? value)
  {
    // value가 string 타입인지를 확인하고, 타입이 맞으면 해당 변수에 저장
    if (value is string password)
    {
      // 최소 길이 8자 이상, 숫자 포함, 특수문자 포함, 대소문자 포함
      return password.Length >= 8 &&
             password.Any(char.IsDigit) &&
             password.Any(char.IsLower) &&
             password.Any(char.IsUpper) &&
             password.Any(ch => !char.IsLetterOrDigit(ch));
    }

    // value가 string이 아닌 경우에는 유효하지 않다고 간주
    return false;
  }
}
