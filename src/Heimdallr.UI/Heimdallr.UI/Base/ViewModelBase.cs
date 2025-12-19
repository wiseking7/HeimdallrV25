using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Heimdallr.UI.Base;
/// <summary>
/// MVVM용 기본 ViewModel
/// BindableBase, INotifyDataErrorInfo, IDestructible, Prism INavigationAware
/// </summary>
public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo, IDestructible, INavigationAware
{
  #region DI 컨테이너 서비스
  private IEventAggregator? _eventAggregator;
  private IRegionManager? _regionManager;

  /// <summary>
  /// Prism의 DI 컨테이너 인터페이스(컨테이너프로바이더)를 저장하는 필드, protected로 선언하여 상속받은 ViewModel에서 
  /// 직접 접근 가능 런타임에 필요한 서비스, 개체를 Resolve<T>제네릭 메서드로 꺼 낼 수 있음(사용)
  /// </summary>
  protected IContainerProvider ContainerProvider { get; private set; }
  #endregion

  #region 생성자
  public ViewModelBase(IContainerProvider containerProvider)
  {
    ContainerProvider = containerProvider ?? throw new ArgumentNullException(nameof(containerProvider));
    EventAggregator = ContainerProvider.Resolve<IEventAggregator>();
    RegionManager = ContainerProvider.Resolve<IRegionManager>();
  }
  #endregion

  #region DI 컨테이너 서비스 프로퍼티
  /// <summary>
  /// Prism의 이벤트 집합체인 이벤트에그리에터 인스턴스 (느슨한 결합을 위한 Pub/Sub 이벤트 통신) 여러 ViewModel 간 메시지 전달 및 구독에 활용
  /// private set으로 외부에서 수정 불가, 생성자에서 DI 컨테이너로부터 주입받음 null일 경우 예외 발생(런타임 안전성 확보)
  /// </summary>
  public IEventAggregator EventAggregator
  {
    get => _eventAggregator ?? throw new ArgumentNullException("_eventAggregator");
    private set => SetProperty(ref _eventAggregator, value, "EventAggregator"); // non-virtual – 재정의 불가

  }

  /// <summary>
  /// Prism의 RegionManager 인스턴스 (화면 내 여러 영역(Region)에 View를 동적으로 로드/교체) 메뉴 클릭 등으로 특정
  /// Region에 View를 전환할 때 사용 private set으로 외부 변경 제한, 생성자에서 DI 컨테이너로부터 초기화됨 null일 경우 예외 발생 런타임 오류 방지
  /// </summary>
  public IRegionManager RegionManager
  {
    get => _regionManager ?? throw new ArgumentNullException("_regionManager");
    private set => SetProperty(ref _regionManager, value, "RegionManager"); // non-virtual – 재정의 불가
  }
  #endregion

  #region RelolveLazy 제네릭 메서드 
  /// <summary>
  /// DI 컨테이너에서 지연 초기화(Lazy) 객체를 쉽게 생성할 수 있는 헬퍼 메서드입니다.
  /// </summary>
  /// <typeparam name="T">해당 서비스 타입</typeparam>
  /// <returns>Lazy로 감싼 서비스 인스턴스</returns>
  protected Lazy<T> ResolveLazy<T>() where T : class
  {
    return new Lazy<T>(() => ContainerProvider.Resolve<T>()); // 재정의 불가
  }
  #endregion

  #region RunOnUIThread 헬퍼 메서드
  /// <summary>
  /// 비동기 작업을 UI 스레드에서 실행하기 위한 헬퍼 메서드입니다. 
  /// 비동기 콜백 등에서 UI 스레드 접근 시 유용합니다 UI 요소
  /// (예: ObservableCollection,Text, ListView.Items 등)**는 UI 스레드에서만 접근 가능합니다.
  /// </summary>
  /// <param name="action"></param>
  /// <returns></returns>
  protected Task RunOnUiThreadAsync(Func<Task> action)  // 재정의 불가
  {
    if (Application.Current == null)
    {
      // Application.Current가 null인 경우 처리 (예: 예외를 던지거나 로깅)
      throw new InvalidOperationException("Application.Current null 입니다. UI thread 접근할 수 없습니다.");
    }

    return Application.Current.Dispatcher.CheckAccess()
        ? action()
        : Application.Current.Dispatcher.InvokeAsync(action).Task;
  }

  /// <summary>
  /// 동기 UI 스레드 실행
  /// </summary>
  /// <param name="action"></param>
  protected void RunOnUiThread(Action action)  // 재정의 불가
  {
    if (Application.Current == null)
    {
      // Application.Current가 null인 경우 처리 (예: 예외를 던지거나 로깅)
      throw new InvalidOperationException("Application.Current null 입니다. UI thread 접근할 수 없습니다.");
    }

    if (Application.Current.Dispatcher.CheckAccess())
    {
      action();
    }
    else
    {
      Application.Current.Dispatcher.Invoke(action);
    }
  }
  #endregion

  #region INotifyDataErrorInfo 관련 필드 및 컬렉션
  #region Dictionary 필드 및 컬렉션
  private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
  private readonly Dictionary<string, List<Func<IEnumerable<string>>>> _syncRules = new Dictionary<string, List<Func<IEnumerable<string>>>>();
  private readonly Dictionary<string, List<Func<Task<IEnumerable<string>>>>> _asyncRules = new Dictionary<string, List<Func<Task<IEnumerable<string>>>>>();
  #endregion

  #region HasErrorsBindable 필드 및 프로퍼티
  private bool _hasErrorsBindable;
  public bool HasErrorsBindable
  {
    get => _hasErrorsBindable;
    private set => SetProperty(ref _hasErrorsBindable, value, "HasErrorsBindable"); // non-virtual – 재정의 불가

  }
  #endregion

  #region AllErrors 프로퍼티
  /// <summary>
  /// 전체 오류 문자열 - 예: 모든 속성 오류를 TextBlock이나 메시지 창에 표시
  /// </summary>
  public string AllErrors
  {
    get => string.Join(Environment.NewLine, GetErrors(null)?.Cast<string>().ToList() ?? new List<string>()); // non-virtual
  }
  #endregion

  #region HasErrorsCount 프로퍼티
  /// <summary>
  /// 현재 ViewModel의 전체 오류 메시지 개수 반환 - UI에서 오류 개수 표시 또는 로깅용 - 예: "입력 오류 3건 발생" 표시
  /// </summary>
  public int HasErrorsCount => _errors.Sum(kv => kv.Value.Count); // non-virtual
  #endregion

  #region INotifyDataErrorInfo 구현
  /// <summary>
  /// 체 오류 존재 여부 확인 - _errors 딕셔너리가 비어있지 않으면 true 반환
  /// </summary>
  public bool HasErrors => _errors.Any(); // non-virtual – 재정의 불가

  /// <summary>
  /// INotifyDataErrorInfo 이벤트 - 속성의 오류가 변경될 때 UI에 알리기 위해 사용
  /// </summary>
  public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

  /// <summary>
  /// 특정 속성 또는 모든 속성의 오류 반환 - propertyName이 null이면 전체 속성의 오류를 반환 - propertyName이 지정되면 해당 속성 오류 반환
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <returns>오류문자열</returns>
  /// <exception cref="NotImplementedException"></exception>
  public IEnumerable GetErrors(string? propertyName) // 재정의 불가
  {
    if (_errors.Count == 0)
      return Enumerable.Empty<string>(); // 오류가 없으면 빈 문자열 컬렉션 반환

    if (string.IsNullOrEmpty(propertyName))
      return _errors.SelectMany(kv => kv.Value ?? Enumerable.Empty<string>()).ToList(); // 모든 오류 반환 

    // 특정 프로퍼티에 대한 오류 반환
    return _errors.TryGetValue(propertyName, out var value) ? value ?? Enumerable.Empty<string>() : Enumerable.Empty<string>();
  }
  #endregion

  #region GetErrorsTyped 재정의 헬퍼 메서드
  /// <summary>
  /// 상속용 강타입 헬퍼 추가
  /// </summary>
  /// <param name="propertyName"></param>
  /// <returns></returns>
  public virtual IEnumerable<string> GetErrorsTyped(string? propertyName) // 상속용 재정의
  {
    return GetErrors(propertyName)?.Cast<string>() ?? Enumerable.Empty<string>(); // IEnumerable<string>으로 변환하여 반환
  }
  #endregion
  #endregion

  #region 동기 검증 및 비동기 검증
  /// <summary>
  /// 동기 검증 규칙 추가 -> 속성별로 여러 검증 함수들 등록 가능
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <param name="rule">검증함수 IEnumerable </param>
  protected void AddValidationRule(string propertyName, Func<IEnumerable<string>> rule) // non-virtual – 재정의 불가
  {
    if (!_syncRules.TryGetValue(propertyName, out List<Func<IEnumerable<string>>>? value))
    {
      value = new List<Func<IEnumerable<string>>>();
      _syncRules[propertyName] = value;
    }

    value.Add(rule);
  }

  /// <summary>
  /// 비동기 검증 규칙 추가 -> 속성별로 여러 비동기 검증 함수들 등록 가능
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <param name="asyncRule">검증함스 IEnumerable</param>
  protected void AddValidationRuleAsync(string propertyName, Func<Task<IEnumerable<string>>> asyncRule) // non-virtual – 재정의 불가
  {
    if (asyncRule == null)
      throw new ArgumentNullException(nameof(asyncRule), "비동기 검증 함수는 null일 수 없습니다.");

    if (!_asyncRules.TryGetValue(propertyName, out List<Func<Task<IEnumerable<string>>>>? value))
    {
      value = new List<Func<Task<IEnumerable<string>>>>();
      _asyncRules[propertyName] = value;
    }

    value.Add(asyncRule);
  }
  #endregion

  #region ValidateProperty 헬퍼메서드
  /// <summary>
  /// 특정 속성에 대한 동기 검증 수행
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <returns>Task</returns>
  protected Task ValidateProperty(string propertyName) // 동기 검증
  {
    List<string> errors = new List<string>();

    if (_syncRules.TryGetValue(propertyName, out List<Func<IEnumerable<string>>>? value))
    {
      foreach (Func<IEnumerable<string>> item in value)
      {
        try
        {
          IEnumerable<string> enumerable = item();
          if (enumerable != null)
          {
            errors.AddRange(enumerable.Where((string s) => !string.IsNullOrWhiteSpace(s)));
          }
        }
        catch (Exception ex)
        {
          // 예외 로깅 또는 사용자 알림 처리
          Debug.WriteLine($"[{nameof(ViewModelBase)}.{MethodBase.GetCurrentMethod()?.Name}] {propertyName} 동기화 검증 오류: {ex.Message}");
        }
      }
    }

    SetErrors(propertyName, errors);

    return Task.CompletedTask;
  }
  #endregion

  #region ValidatePropertyAsync 헬퍼 메서드
  /// <summary>
  /// 특정 속성에 대한 비동기 검증 수행
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <returns>Task</returns>
  protected async Task ValidatePropertyAsync(string propertyName) // 비동기 검증
  {
    List<string> errors = new List<string>();

    if (_asyncRules.TryGetValue(propertyName, out var asyncRules))
    {
      var tasks = asyncRules.Select(rule => rule()).ToArray();

      var results = await Task.WhenAll(tasks);

      foreach (var result in results)
      {
        errors.AddRange(result);
      }
    }

    SetErrors(propertyName, errors);
  }
  #endregion

  #region ValidatePropertyCombinedAsync
  /// <summary>
  /// 특정 속성에 대해 동기 및 비동기 검증을 함께 실행
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <returns>Task</returns>
  protected async Task ValidatePropertyFullAsync(string propertyName) // 동기 + 비동기 검증
  {
    List<string> errors = new List<string>();

    // 1. 동기 검증을 먼저 실행
    if (_syncRules.TryGetValue(propertyName, out List<Func<IEnumerable<string>>>? syncRules))
    {
      foreach (var syncRule in syncRules)
      {
        try
        {
          // 동기 검증 결과를 가져와서 오류 리스트에 추가
          var syncErrors = syncRule() ?? Enumerable.Empty<string>();
          errors.AddRange(syncErrors.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
        catch (Exception ex)
        {
          Debug.WriteLine($"[{nameof(ViewModelBase)}.{MethodBase.GetCurrentMethod()?.Name}] {propertyName} 동기 검증 오류: {ex.Message}");
        }
      }
    }

    // 2. 비동기 검증을 이어서 실행
    if (_asyncRules.TryGetValue(propertyName, out var asyncRules))
    {
      // 비동기 검증 함수들을 실행하여 결과를 기다립니다.
      var tasks = asyncRules.Select(rule => rule()).ToArray();
      var results = await Task.WhenAll(tasks);

      // 비동기 검증 결과를 오류 리스트에 추가
      foreach (var result in results)
      {
        errors.AddRange(result.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
    }

    // 3. 중복된 오류 제거
    errors = errors.Distinct().ToList();  // 중복된 오류 메시지 제거

    // 4. 오류를 UI에 전달
    SetErrors(propertyName, errors);
  }
  #endregion

  #region ValidateAllAsync 헬퍼 메서드
  /// <summary>
  /// 모든 속성 검증 수행 - 등록된 모든 속성에 대해 ValidatePropertyAsync 실행
  /// </summary>
  /// <returns></returns>
  protected Task ValidateAllAsync() => Task.WhenAll(_syncRules.Keys.Union(_asyncRules.Keys).Distinct().Select(ValidatePropertyAsync)); // non-virtual
  #endregion

  #region ValidateAllAndReturnAsync 헬퍼 메서드
  /// <summary>
  /// 모든 속성 검증 수행 후 오류 여부 반환 - 예: 저장 버튼 클릭 전 검증 체크
  /// </summary>
  /// <returns>오류가 없으면 true</returns>
  public async Task<bool> ValidateAllAndReturnAsync() // non-virtual
  {
    await ValidateAllAsync();
    return !HasErrors;
  }
  #endregion

  #region SetErrors 헬퍼 메서드
  /// <summary>
  /// 속성별 오류 설정 - UI에 알리기 위해 ErrorsChanged 이벤트 호출 - 이전 오류와 비교하여 변경이 있을 때만 이벤트 발생
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  /// <param name="errors">오류 문자열 컬렉션</param>
  protected virtual void SetErrors(string propertyName, IEnumerable<string> errors) // non-virtual – 재정의 불가
  {
    bool flag = false;

    if (errors != null && errors.Any())
    {
      List<string> list = errors.ToList();

      if (!_errors.TryGetValue(propertyName, out List<string>? value) || !new HashSet<string>(value).SetEquals(list))
      {
        _errors[propertyName] = list;
        flag = true;
      }
    }
    else
    {
      flag = _errors.Remove(propertyName);
    }

    HasErrorsBindable = _errors.Any();

    if (flag)
    {
      OnErrorsChanged(propertyName);  // 오류가 변경되면 이벤트 호출
    }
  }
  #endregion

  #region OnErrorsChanged 헬퍼 메서드
  /// <summary>
  /// ErrorsChanged 이벤트 호출 - UI 스레드에서 안전하게 호출
  /// </summary>
  /// <param name="propertyName">속성이름</param>
  protected virtual void OnErrorsChanged(string propertyName)
  {
    if (Application.Current?.Dispatcher?.CheckAccess() == true)
    {
      // 이미 UI 스레드
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    else if (Application.Current?.Dispatcher != null)
    {
      // 백그라운드 -> UI 스레드 마샬링
      Application.Current.Dispatcher.InvokeAsync(() =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName)));
    }
    else
    {
      // 테스트 / 비-WPF 환경
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
  #endregion

  #region SetPropertyAndValidate 헬퍼 메서드
  /// <summary>
  /// 속성 값 변경 + 자동 검증 - 예: SetPropertyAndValidate(ref _name, value);
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="storage"></param>
  /// <param name="value"></param>
  /// <param name="propertyName"></param>
  /// <returns></returns>
  protected bool SetPropertyAndValidate<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null) // non-virtual – 재정의 불가
  {
    if (SetProperty(ref storage, value, propertyName))
    {
      if (!string.IsNullOrEmpty(propertyName))
      {
        _ = ValidatePropertyAsync(propertyName); // null 아님이 보장됨
      }

      return true;
    }

    return false;
  }
  #endregion

  #region ResetProperty 헬퍼 메서드
  /// <summary>
  /// 속성 초기화(기본값 설정) + 검증 + PropertyChanged 이벤트 발생 - 사용 예: ResetProperty(ref _name);
  ///  - UI 바인딩에서 값 변경과 오류 상태 모두 갱신됨
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="storage"></param>
  /// <param name="defaultValue"></param>
  /// <param name="propertyName"></param>
  protected void ResetProperty<T>(ref T storage, T? defaultValue = default, [CallerMemberName] string? propertyName = null)
  {
    storage = defaultValue!;   // 의도적 초기화
    if (!string.IsNullOrEmpty(propertyName))
    {
      ClearErrors(propertyName);
      RaisePropertyChanged(propertyName);
    }
  }
  #endregion

  #region ResetAndValidatePropertyAsync (비동기)
  /// <summary>
  /// 속성 검증 포함 async 초기화
  /// ref를 직접 사용하지 않고 propertyName 전달 초기화를 반환하는 패턴으로 변경
  /// </summary>
  protected async Task ResetAndValidatePropertyAsync<T>(Expression<Func<T?>> propertyExpr, T? defaultValue = default)
  {
    if (propertyExpr.Body is MemberExpression memberExpr)
    {
      var property = memberExpr.Member as PropertyInfo;
      if (property == null) return;

      var field = GetType()
          .GetField($"_{property.Name.ToLower()}", BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == null) return;

      field.SetValue(this, defaultValue);  // 필드 초기화
      ClearErrors(property.Name);
      RaisePropertyChanged(property.Name);
      await ValidatePropertyAsync(property.Name);
    }
  }
  #endregion

  #region ValidateAllPropertiesAsync 헬퍼 메서드
  /// <summary>
  /// ViewModel의 모든 속성 초기화 후 검증 - 등록된 모든 동기/비동기 검증 규칙을 기반으로 오류 상태 갱신 - 사용 예: ViewModel
  /// 초기화 시 전체 속성 초기화
  /// </summary>
  /// <returns></returns>
  protected async Task ValidateAllPropertiesAsync() // non-virtual – 재정의 불가
  {
    foreach (string item in _syncRules.Keys.Union(_asyncRules.Keys))
    {
      ClearErrors(item);
    }

    await ValidateAllAsync();
  }
  #endregion

  #region ClearErrors, ClearAllErrors, CancelCurrentTask, OnDestroying, Destroy (헬퍼메서드) CancellationTokenSource, CancellationToken, 

  #region ClearErrors 헬퍼 메서드
  /// <summary>
  /// 특정 속성의 오류를 제거합니다. 선택적으로 오류 메시지 조건을 설정하여 특정 오류만 제거할 수 있습니다.
  /// </summary>
  /// <param name="propertyName">속성 이름</param>
  /// <param name="condition">오류를 지울 조건 (옵션)</param>
  protected void ClearErrors(string propertyName, Func<string, bool>? condition = null) // non-virtual
  {
    if (_errors.ContainsKey(propertyName))
    {
      var errors = _errors[propertyName];

      // 조건이 주어졌다면 조건에 맞는 오류만 필터링해서 제거
      if (condition != null)
      {
        var filteredErrors = errors.Where(condition).ToList();
        if (filteredErrors.Any())
        {
          _errors[propertyName] = _errors[propertyName].Except(filteredErrors).ToList();
          if (_errors[propertyName].Count == 0)
          {
            _errors.Remove(propertyName);
          }
        }
      }
      else
      {
        // 조건 없이 모든 오류 제거
        _errors.Remove(propertyName);
      }

      HasErrorsBindable = _errors.Any();  // 오류 상태 갱신
      OnErrorsChanged(propertyName);      // 오류 상태 변경 이벤트 호출
    }
  }
  #endregion

  #region ClearErrorsAsync 헬퍼 메서드
  protected Task ClearErrorsAsync(string propertyName, Func<string, bool>? condition = null)
  {
    ClearErrors(propertyName, condition);
    return Task.CompletedTask;
  }
  #endregion

  #region ClearAllErrors 헬퍼 메서드
  /// <summary>
  /// 모든 오류를 제거합니다. 하지만 특정 조건을 만족하는 오류만 제거할 수 있도록 개선합니다.
  /// </summary>
  /// <param name="condition">오류를 지울 조건 (옵션)</param>
  protected void ClearAllErrors(Func<string, bool>? condition = null) // non-virtual
  {
    if (_errors.Count > 0)
    {
      if (condition != null)
      {
        // 조건이 주어졌다면 해당 조건에 맞는 오류만 제거
        foreach (var propertyName in _errors.Keys.ToList())
        {
          var filteredErrors = _errors[propertyName].Where(condition).ToList();
          if (filteredErrors.Any())
          {
            _errors[propertyName] = _errors[propertyName].Except(filteredErrors).ToList();
            if (_errors[propertyName].Count == 0)
            {
              _errors.Remove(propertyName);
            }
          }
        }
      }
      else
      {
        // 조건 없이 모든 오류를 제거
        _errors.Clear();
      }

      HasErrorsBindable = _errors.Any(); // 오류 상태 갱신
    }
  }
  #endregion

  #region ClearAllErrorsAsync 헬퍼 메서드
  protected Task ClearAllErrorsAsync(Func<string, bool>? condition = null)
  {
    ClearAllErrors(condition);
    return Task.CompletedTask;
  }
  #endregion

  private Dictionary<string, CancellationTokenSource> _cancellationTokens = new Dictionary<string, CancellationTokenSource>();

  #region GetCancellationToken 헬퍼 메서드
  /// <summary>
  /// 현재 ViewModel에서 사용되는 취소 토큰
  /// </summary>
  /// <param name="taskName">작업 이름</param>
  /// <returns>취소 토큰</returns>
  public CancellationToken GetCancellationToken(string taskName, TimeSpan? timeout = null)
  {
    if (!_cancellationTokens.ContainsKey(taskName))
    {
      var cts = new CancellationTokenSource();

      if (timeout.HasValue)
      {
        cts.CancelAfter(timeout.Value);  // 작업 취소를 위한 시간 초과 설정
      }
      _cancellationTokens[taskName] = cts;
    }

    return _cancellationTokens[taskName].Token;
  }
  #endregion

  #region CancelTask (동기)
  /// <summary>
  /// 현재 실행 중인 동기 작업을 취소합니다.
  /// </summary>
  /// <param name="taskName">작업 이름</param>
  public void CancelTask(string taskName)
  {
    if (_cancellationTokens.ContainsKey(taskName))
    {
      _cancellationTokens[taskName].Cancel();  // 작업 취소
      _cancellationTokens[taskName].Dispose(); // 자원 해제
      _cancellationTokens.Remove(taskName);    // 딕셔너리에서 삭제
    }
  }
  #endregion

  #region CancelTaskAsync (비동기)
  /// <summary>
  /// 현재 실행 중인 비동기 작업을 취소합니다.
  /// </summary>
  /// <param name="taskName"></param>
  /// <returns></returns>
  public Task CancelTaskAsync(string taskName)
  {
    if (_cancellationTokens.ContainsKey(taskName))
    {
      _cancellationTokens[taskName].Cancel();  // 작업 취소
      _cancellationTokens[taskName].Dispose(); // 자원 해제
      _cancellationTokens.Remove(taskName);
    }

    return Task.CompletedTask; // 단순히 Task 반환
  }
  #endregion

  #region CancelAllTasks (동기) 
  /// <summary>
  /// 모든 동기 작업을 취소합니다.
  /// </summary>
  public void CancelAllTasks()
  {
    foreach (var taskName in _cancellationTokens.Keys.ToList())
    {
      CancelTask(taskName);  // 모든 작업 취소
    }
  }
  #endregion

  #region CancelAllTasksAsync (비동기)
  /// <summary>
  /// 모든 비동기 작업을 취소합니다
  /// </summary>
  /// <returns></returns>
  public async Task CancelAllTasksAsync()
  {
    var tasks = new List<Task>();

    foreach (var taskName in _cancellationTokens.Keys.ToList())
    {
      tasks.Add(CancelTaskAsync(taskName));
    }

    await Task.WhenAll(tasks);
  }
  #endregion

  #region OnViewModelDestroyed (동기)
  /// <summary>
  /// 자식 ViewModel에서 필요한 경우 overrid EventAggregator 구독 해제, 타이머 중단, IDisposable 자원 해제, 내부 연결 또는 참조 정리
  /// </summary>
  protected virtual void OnViewModelDestroyed() { } // virtual – 재정의 가능
  #endregion

  #region OnViewModelDestroyedAsync (비동기)
  /// <summary>
  /// 자식 ViewModel에서 필요한 경우 이벤트 구독 해제, 타이머 중단, IDisposable 자원 해제, 내부 연결 정리 등을 비동기 처리
  /// </summary>
  protected virtual Task OnViewModelDestroyedAsync()
  {
    // 기본 구현은 아무 작업 없음
    return Task.CompletedTask;
  }
  #endregion

  #region IDestructible 구현 (동기)
  public void Destroy() // non-virtual – 재정의 불가
  {
    CancelAllTasks(); // 모든 작업 취소

    OnViewModelDestroyed();  // 자식 클래스에서 추가적인 작업을 할 수 있도록 호출

    // 여기에 다른 자원 해제 로직을 추가할 수 있습니다.
  }
  #endregion

  #region IDestructible 구현 (비동기)
  public async Task DestroyAsync()
  {
    // 1. 모든 작업 취소
    await CancelAllTasksAsync();

    // 2. 자식 ViewModel에서 필요한 비동기 자원 해제
    if (OnViewModelDestroyedAsync != null)
      await OnViewModelDestroyedAsync();

    // 3. 기타 비동기 자원 해제 (예: IDisposable 비동기 해제 등)
  }
  #endregion
  #endregion

  #region Prism INavigationAware 재정의 전부 재정의 가능
  public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;

  public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

  public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

  #endregion


  #region ValidatePropertyPublic (테스트 / 외부 호출용)


  public Task ValidatePropertyAsyncPublic(string propertyName) => ValidatePropertyFullAsync(propertyName);
  public void ClearErrorsPublic(string propertyName) => ClearErrors(propertyName);
  public void ClearAllErrorsPublic() => ClearAllErrors();

  #endregion
}









