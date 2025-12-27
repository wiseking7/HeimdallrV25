
namespace Tests.ViewModeTests;

/// <summary>
/// INavigationAware 테스트용 ViewModel
/// - Prism ViewModelBase 상속
/// - Navigation 메서드 호출 기록
/// </summary>
public class TestNavigationViewModel : ViewModelBase
{
  public bool OnNavigatedToCalled { get; private set; }
  public bool OnNavigatedFromCalled { get; private set; }
  public NavigationContext? LastNavigationContext { get; private set; }

  public TestNavigationViewModel(IContainerProvider container)
      : base(container)
  {
  }

  public override void OnNavigatedTo(NavigationContext navigationContext)
  {
    OnNavigatedToCalled = true;
    LastNavigationContext = navigationContext;
  }

  public override void OnNavigatedFrom(NavigationContext navigationContext)
  {
    OnNavigatedFromCalled = true;
    LastNavigationContext = navigationContext;
  }

  public override bool IsNavigationTarget(NavigationContext navigationContext) => true;

  // 테스트용 초기화
  public void ResetFlags()
  {
    OnNavigatedToCalled = false;
    OnNavigatedFromCalled = false;
    LastNavigationContext = null;
  }
}

