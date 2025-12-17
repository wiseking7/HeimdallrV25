using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System.Collections;
using System.ComponentModel;

namespace Heimdallr.Utility;

public abstract class BaseViewModel : BindableBase, INotifyDataErrorInfo, IDestructible, INavigationAware
{
  // DI 서비스, UI 작업, 검증, 내비게이션, 취소 토큰 관리 서비스 선언
  protected ServiceLocator ServiceLocator { get; }
  protected UIThreadExecutor UIThreadExecutor { get; }
  protected ValidationService ValidationService { get; }
  protected NavigationService NavigationService { get; }
  protected CancellationManager CancellationManager { get; }

  public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

  // 생성자에서 서비스 초기화
  public BaseViewModel(IContainerProvider containerProvider)
  {
    // ServiceLocator로 의존성 주입을 해결
    ServiceLocator = new ServiceLocator(containerProvider);
    UIThreadExecutor = new UIThreadExecutor();
    ValidationService = new ValidationService();
    NavigationService = new NavigationService(ServiceLocator.Resolve<IRegionManager>());
    CancellationManager = new CancellationManager();
  }

  // 여기에 기존의 ViewModelBase에서 제공하던 메서드를 필요에 따라 사용하여 
  // 각 기능을 서비스에 위임하거나 호출하도록 구현
  // 예: 검증, UI 작업, 내비게이션 등을 각 서비스로 위임

  public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;
  public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
  public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

  // IDestructible 인터페이스 구현
  public void Destroy()
  {
    // 작업 취소 및 자원 해제
    CancellationManager.CancelCurrentTask();
  }

  private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>(); // 오류 저장
  private bool _hasErrors;
  // INotifyDataErrorInfo 구현 - HasErrors
  public bool HasErrors
  {
    get => _hasErrors;
    private set => SetProperty(ref _hasErrors, value); // SetProperty로 변경 사항 추적
  }

  // INotifyDataErrorInfo 구현 - GetErrors
  public IEnumerable GetErrors(string? propertyName)
  {
    if (string.IsNullOrEmpty(propertyName))
    {
      return _errors.Values.SelectMany(e => e); // 모든 오류를 반환
    }

    return _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<string>();
  }

  // 오류 상태 설정 (내부적으로 호출하여 오류 상태를 업데이트)
  protected void SetErrors(string propertyName, IEnumerable<string> errors)
  {
    bool errorsChanged = false;
    if (errors.Any())
    {
      var errorList = errors.ToList();
      if (!_errors.ContainsKey(propertyName) || !_errors[propertyName].SequenceEqual(errorList))
      {
        _errors[propertyName] = errorList;
        errorsChanged = true;
      }
    }
    else
    {
      if (_errors.Remove(propertyName)) errorsChanged = true;
    }

    // 오류 상태가 변경되었으면 이벤트 발생
    HasErrors = _errors.Any();
    if (errorsChanged)
    {
      OnErrorsChanged(propertyName);
    }
  }

  // 오류가 변경될 때 호출되는 이벤트 트리거
  protected virtual void OnErrorsChanged(string propertyName)
  {
    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
  }
}

