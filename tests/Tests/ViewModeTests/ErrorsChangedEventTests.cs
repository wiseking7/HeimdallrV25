using Prism.Ioc;

namespace Tests.ViewModeTests;

#region === Event & HasErrorsBindable Tests ===

/// <summary>
/// 테스트 대상: ViewModelBase.ErrorsChanged 이벤트, HasErrorsBindable 속성
/// 테스트 목적:
/// - ErrorsChanged 이벤트 발생 여부
/// - UI 스레드로 이벤트 마샬링 여부
/// - HasErrorsBindable 속성 상태 반영 여부
/// </summary>
public class ErrorsChangedEventTests : ViewModelTestBase<TestErrorsViewModel>
{
  protected override TestErrorsViewModel CreateViewModel(IContainerProvider container)
      => new TestErrorsViewModel(container);

  [Fact]
  public async Task ErrorsChanged_Should_Fire_When_Error_Added()
  {
    // 🔹 ViewModelBase.OnErrorsChanged 테스트
    int eventCount = 0;
    string? changedProperty = null;

    VM.ErrorsChanged += (s, e) =>
    {
      eventCount++;
      changedProperty = e.PropertyName;
    };

    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name)); // ValidatePropertyFullAsync 테스트

    Assert.Equal(1, eventCount);
    Assert.Equal(nameof(VM.Name), changedProperty);
  }

  [Fact]
  public async Task HasErrorsBindable_Should_Reflect_Error_State()
  {
    // 🔹 ViewModelBase.HasErrorsBindable 테스트
    Assert.False(VM.HasErrorsBindable);

    VM.Name = "";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    Assert.True(VM.HasErrorsBindable);

    VM.Name = "Valid";
    await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    Assert.False(VM.HasErrorsBindable);
  }

  [Fact]
  public async Task ErrorsChanged_Should_Be_Raised_On_UI_Thread()
  {
    // 🔹 ViewModelBase.OnErrorsChanged + Dispatcher(UI 스레드) 테스트
    var uiContext = new TestSynchronizationContext();
    SynchronizationContext.SetSynchronizationContext(uiContext);

    int? eventThreadId = null;
    VM.ErrorsChanged += (s, e) => eventThreadId = Environment.CurrentManagedThreadId;

    await Task.Run(async () =>
    {
      VM.Name = "";
      await VM.ValidatePropertyAsyncPublic(nameof(VM.Name));
    });

    Assert.NotNull(eventThreadId);
    Assert.Equal(uiContext.ThreadId, eventThreadId);
  }
}

#endregion





