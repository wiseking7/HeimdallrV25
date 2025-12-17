using Prism.Ioc;

namespace Tests.ViewModeTests;

#region === Reset / ClearErrors Tests ===

/// <summary>
/// 테스트 대상: ViewModelBase.ResetProperty, ClearErrors
/// 테스트 목적:
/// - ResetProperty 시 값 초기화, 오류 제거, HasErrorsBindable 상태 반영
/// - ClearErrors 시 특정 속성 오류 제거 및 상태 반영
/// </summary>
public class ErrorsResetTests : ViewModelTestBase<TestErrorsViewModel>
{
  protected override TestErrorsViewModel CreateViewModel(IContainerProvider container)
      => new TestErrorsViewModel(container);

  [Fact]
  public async Task ResetProperty_Should_Clear_Value_And_Errors()
  {
    var vm = CreateViewModel(ContainerMock.Object);

    // 오류 상태 만들기
    vm.Name = "";
    await vm.ValidatePropertyAsyncPublic(nameof(vm.Name));
    Assert.True(vm.HasErrorsBindable);

    // ResetProperty + 검증 수행
    await vm.ResetNameAndValidateAsync();

    // 상태 검증
    Assert.Null(vm.Name);
    Assert.False(vm.HasErrorsBindable);
    Assert.Empty(vm.GetErrors(nameof(vm.Name)).Cast<string>().ToList());
  }

  [Fact]
  public async Task ResetProperty_Then_Revalidate_Should_Have_Errors()
  {
    // 🔹 Reset 후 재검증 시 ValidatePropertyFullAsync 테스트
    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    Assert.True(VM.HasErrorsBindable);

    VM.ResetName();
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));

    Assert.True(VM.HasErrorsBindable);
  }

  [Fact]
  public async Task ClearErrors_Should_Remove_Specific_Property_Errors()
  {
    // 🔹 ViewModelBase.ClearErrors 테스트
    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    Assert.True(VM.HasErrorsBindable);

    VM.ClearErrorsPublic(nameof(VM.Name));

    Assert.Equal("", VM.Name);
    Assert.False(VM.HasErrorsBindable);
    Assert.Empty(VM.GetErrors(nameof(VM.Name)).Cast<string>().ToList());
  }
}

#endregion
