using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Heimdallr.Utility.ViewModels;
/// <summary>
/// MVVM용 기본 ViewModel
/// INotifyDataErrorInfo, Prism Navigation, IDestructible 통합
/// </summary>
public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo, IDestructible, INavigationAware
{
  // DI Container
  protected IContainerProvider Container { get; }
  protected IEventAggregator EventAggregator { get; }
  protected IRegionManager RegionManager { get; }

  // 오류 관리 변수
  private readonly Dictionary<string, ObservableCollection<string>> _propertyErrors = new();
  private readonly Dictionary<string, List<Func<IEnumerable<string>>>> _syncRules = new();
  private readonly Dictionary<string, List<Func<Task<IEnumerable<string>>>>> _asyncRules = new();

  // INotifyDataErrorInfo 이벤트
  public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

  // 오류 여부 프로퍼티
  public bool HasErrors => _propertyErrors.Any(p => p.Value.Any());
  private bool _hasErrorsBindable;
  public bool HasErrorsBindable
  {
    get => _hasErrorsBindable;
    private set => SetProperty(ref _hasErrorsBindable, value);
  }

  #region Constructor
  // 생성자: DI 컨테이너를 통한 의존성 주입
  protected ViewModelBase(IContainerProvider container)
  {
    Container = container ?? throw new ArgumentNullException(nameof(container));
    RegionManager = container.Resolve<IRegionManager>();
    EventAggregator = container.Resolve<IEventAggregator>();
  }
  #endregion

  #region Validation

  /// <summary>속성별 오류 반환</summary>
  public IEnumerable GetErrors(string? propertyName)
      => string.IsNullOrEmpty(propertyName)
          ? _propertyErrors.SelectMany(p => p.Value).ToList()
          : _propertyErrors.TryGetValue(propertyName, out var col) ? col : Array.Empty<string>();

  /// <summary>동기 검증 규칙 추가</summary>
  protected void AddValidationRule(string propertyName, Func<IEnumerable<string>> rule)
  {
    if (!_syncRules.TryGetValue(propertyName, out var list))
      _syncRules[propertyName] = list = new List<Func<IEnumerable<string>>>();
    list.Add(rule);
  }

  /// <summary>비동기 검증 규칙 추가</summary>
  protected void AddValidationRuleAsync(string propertyName, Func<Task<IEnumerable<string>>> asyncRule)
  {
    if (!_asyncRules.TryGetValue(propertyName, out var list))
      _asyncRules[propertyName] = list = new List<Func<Task<IEnumerable<string>>>>();
    list.Add(asyncRule);
  }

  /// <summary>속성 검증 실행 (동기 + 비동기)</summary>
  protected virtual async Task ValidatePropertyAsync(string propertyName)
  {
    var errors = new List<string>();

    // 동기 규칙 실행 및 비동기 규칙 실행
    errors.AddRange(await ExecuteRules(propertyName));

    // UI 스레드에서 오류 컬렉션 업데이트
    await UpdateErrorsAsync(propertyName, errors);
  }

  // 동기 및 비동기 규칙 실행 메서드
  private async Task<IEnumerable<string>> ExecuteRules(string propertyName)
  {
    var errors = new List<string>();

    // 동기 규칙 처리
    if (_syncRules.TryGetValue(propertyName, out var syncList))
    {
      foreach (var rule in syncList)
      {
        errors.AddRange(rule()?.Where(s => !string.IsNullOrWhiteSpace(s)) ?? Array.Empty<string>());
      }
    }

    // 비동기 규칙 처리
    if (_asyncRules.TryGetValue(propertyName, out var asyncList))
    {
      try
      {
        var asyncErrors = await Task.WhenAll(asyncList.Select(rule => rule()));
        errors.AddRange(asyncErrors.SelectMany(errors => errors ?? Array.Empty<string>()));
      }
      catch (Exception ex)
      {
        // 비동기 검증 중 예외 발생 시 메시지 추가
        errors.Add($"비동기 검증 중 오류 발생: {ex.Message}");
      }
    }

    return errors;
  }

  #endregion

  #region Error Management

  /// <summary>속성 오류 업데이트 (UI 스레드에서 실행)</summary>
  private Task UpdateErrorsAsync(string propertyName, IEnumerable<string> errors)
  {
    ExecuteOnUiThread(() =>
    {
      // 오류 컬렉션 초기화 및 갱신
      if (!_propertyErrors.TryGetValue(propertyName, out var col))
      {
        col = new ObservableCollection<string>();
        _propertyErrors[propertyName] = col;
      }

      col.Clear();
      foreach (var error in errors)
      {
        col.Add(error);
      }

      HasErrorsBindable = _propertyErrors.Any(p => p.Value.Any());
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    });

    return Task.CompletedTask;
  }

  /// <summary>값이 설정된 후, 해당 속성에 대해 검증을 자동으로 호출</summary>
  protected bool SetPropertyAndValidate<T>(ref T storage, T value, [CallerMemberName] string propertyName = null!)
  {
    if (SetProperty(ref storage, value, propertyName))
    {
      _ = ValidatePropertyAsync(propertyName);  // 비동기 검증 호출
      return true;
    }
    return false;
  }

  /// <summary>속성 오류 초기화</summary>
  protected void ClearErrors(string propertyName)
  {
    _ = UpdateErrorsAsync(propertyName, Array.Empty<string>());
  }

  #endregion

  #region UI Thread Helpers

  /// <summary>비동기 UI 스레드 실행</summary>
  protected Task ExecuteOnUiThreadAsync(Func<Task> action)
  {
    if (Application.Current?.Dispatcher == null)
    {
      return action(); // 예외 처리나 작업 없을 시 동기적으로 실행
    }

    // UI 스레드에서 작업을 실행
    return Application.Current.Dispatcher.CheckAccess() ? action() : Application.Current.Dispatcher.InvokeAsync(action).Task;
  }

  /// <summary>동기 UI 스레드 실행</summary>
  protected void ExecuteOnUiThread(Action action)
  {
    if (Application.Current?.Dispatcher == null)
    {
      action();
    }
    else if (Application.Current.Dispatcher.CheckAccess())
    {
      action(); // UI 스레드에서 실행
    }
    else
    {
      Application.Current.Dispatcher.Invoke(action); // 동기적으로 UI 스레드에서 실행
    }
  }

  #endregion

  #region Prism / Navigation / Cleanup

  // Prism의 네비게이션 관련 메서드들
  public virtual void Destroy() { }

  #region INavigationAware 구현
  public virtual void OnNavigatedTo(NavigationContext navigationContext) { }
  public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;
  public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
  #endregion

  #endregion
}









