namespace Tests.ViewModeTests;

/// <summary>
/// 동기 검증만 사용
/// ErrorsChanged / HasErrorsBindable 검증에 집중
/// </summary>
public class TestErrorsViewModel : ViewModelBase
{
  private string? _name;
  public string? Name
  {
    get => _name;
    set => SetPropertyAndValidate(ref _name, value);
  }

  private string? _code;
  public string? Code
  {
    get => _code;
    set => SetPropertyAndValidate(ref _code, value);
  }

  public TestErrorsViewModel(IContainerProvider container)
    : base(container)
  {
    // 동기 검증 규칙 등록
    AddValidationRule(nameof(Name), () =>
    {
      if (string.IsNullOrWhiteSpace(Name))
        return new[] { "Name은 필수입니다." };

      return Array.Empty<string>();
    });

    // Code 검증
    AddValidationRule(nameof(Code), () =>
    {
      if (string.IsNullOrWhiteSpace(Code))
        return new[] { "Code는 필수입니다." };

      return Array.Empty<string>();
    });
  }

  // 동기: 값 초기화 + 오류 제거
  public void ResetName()
  {
    ResetProperty(ref _name);
  }

  // 비동기: 검증 호출
  // ViewModelBase의 async 반환 메서드 호출 래퍼
  public Task ResetNameAndValidateAsync()
  {
    return ResetAndValidatePropertyAsync(() => Name);
  }
}
