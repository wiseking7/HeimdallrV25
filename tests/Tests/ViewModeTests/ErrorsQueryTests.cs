using Prism.Ioc;

namespace Tests.ViewModeTests;

#region === Errors Query Tests ===

/// <summary>
/// 테스트 대상: ViewModelBase.GetErrors, AllErrors
/// 테스트 목적:
/// - 특정 속성 오류 조회(GetErrors(propertyName))
/// - 모든 속성 오류 조회(GetErrors(null))
/// - AllErrors 문자열 집계 확인
/// </summary>
public class ErrorsQueryTests : ViewModelTestBase<TestErrorsViewModel>
{
  protected override TestErrorsViewModel CreateViewModel(IContainerProvider container)
      => new TestErrorsViewModel(container);

  [Fact]
  public async Task GetErrors_Should_Return_Errors_For_Property()
  {
    // 🔹 ViewModelBase.GetErrors(propertyName) 테스트
    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));

    var errors = VM.GetErrors(nameof(VM.Name)).Cast<string>().ToList();

    Assert.Single(errors);
    Assert.Equal("Name은 필수입니다.", errors[0]);
  }

  [Fact]
  public async Task GetErrors_Null_Should_Return_All_Errors()
  {
    // 🔹 ViewModelBase.GetErrors(null) 테스트
    VM.Name = "";
    VM.Code = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Code));

    var allErrors = VM.GetErrors(null).Cast<string>().ToList();

    Assert.Equal(2, allErrors.Count);
    Assert.Contains("Name은 필수입니다.", allErrors);
    Assert.Contains("Code는 필수입니다.", allErrors);
  }

  [Fact]
  public async Task AllErrors_Should_Contain_All_Property_Errors()
  {
    // 🔹 ViewModelBase.AllErrors 테스트
    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));

    Assert.False(string.IsNullOrWhiteSpace(VM.AllErrors));
    Assert.Contains("Name은 필수입니다.", VM.AllErrors);
  }
}

#endregion
