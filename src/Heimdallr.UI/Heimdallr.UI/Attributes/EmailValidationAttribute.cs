using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Heimdallr.UI.Attributes;

/// <summary>
/// AttributeTargets.Property: 속성에만 적용됩니다.
/// AttributeTargets.Field: 필드에만 적용됩니다
/// AllowMultiple = false: 한 속성에 대해 이 어트리뷰트를 여러 번 사용할 수 없음을 의미합니다
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class EmailValidationAttribute : ValidationAttribute
{
  // 이메일 형식의 유효성 검사 정규 표현식
  private static readonly Regex EmailRegex = new Regex(
      @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
      RegexOptions.IgnoreCase);

  public override bool IsValid(object? value)
  {
    if (value == null)
      return false;

    var email = value as string;
    if (email == null)
      return false;

    // 이메일 형식이 정규 표현식과 일치하는지 확인
    return EmailRegex.IsMatch(email);
  }
}
