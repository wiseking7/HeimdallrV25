using Heimdallr.App.Enums;
using Heimdallr.UI.Controls;
using Heimdallr.UI.Enums;
using Heimdallr.UI.Extensions;
using Heimdallr.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Heimdallr.App.ViewModel;

public class PrismMainViewModel : ViewModelBase
{
  private readonly Lazy<ResourceManager> _resourceManager;
  private readonly Lazy<IDialogService> _dialogService;

  #region 생성자
  public PrismMainViewModel(IContainerProvider container) : base(container)
  {
    _resourceManager = ResolveLazy<ResourceManager>();
    _dialogService = ResolveLazy<IDialogService>();

    ValidationMethod();
    _ = RunOnUiThreadAsync(async () => await RunTests());

    SetupEnumComboBox();
    LoadMenuItems();

    InitializeTimer();

    HeimdallrTreeViewInitialize();

    // ViewModelBase 테스트
    AddValidationRule(nameof(Name), () =>
    {
      var list = new List<string>();
      if (string.IsNullOrWhiteSpace(Name)) list.Add("이름을 입력해야 합니다.");
      if (Name!.Length > 10) list.Add("이름은 10자 이하만 가능합니다.");
      return list;
    });

    AddValidationRuleAsync(nameof(Name), async () =>
    {
      await Task.Delay(50); // 예: 서버 중복 체크
      if (Name == "Admin") return new List<string> { "사용할 수 없는 이름입니다." };
      return new List<string>();
    });

    NameErrors = new ObservableCollection<string>();
  }
  #endregion

  private string? _name = string.Empty;
  public string? Name
  {
    get => _name;
    set => SetPropertyAndValidate(ref _name, value);
  }

  private string? _busyMessage;
  public string? BusyMessage
  {
    get => _busyMessage;
    set => SetProperty(ref _busyMessage, value);
  }

  public async Task ResetAllPropertiesAsync()
  {
    // Name 초기화
    await ResetAndValidatePropertyAsync(() => Name, defaultValue: string.Empty);

    // BusyMessage 초기화
    await ResetAndValidatePropertyAsync(() => BusyMessage, defaultValue: null);

    await ResetAndValidatePropertyAsync(() => ViewModel, defaultValue: null);

    // 필요하면 다른 속성들도 추가
    // await ResetAndValidatePropertyAsync(() => AnotherProperty, defaultValue: someValue);
  }

  private DelegateCommand? _messageCommand;
  public DelegateCommand? MessageCommand => _messageCommand ??= new DelegateCommand(async () =>
  {
    // 1. HeimdallrMessageBox 호출 (UI 스레드 안전)
    var result = UIHelper.ShowMessageBox(
        "삭제하시겠습니까",
        "CHECK",
        UI.Enums.HeimdallrMessageBoxButtonEnum.YesNo,
        IconType.Success_Flag,
        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF810CA8"))
    );

    // 2. 결과에 따라 BusyMessage 표시
    switch (result)
    {
      case MessageBoxResult.Yes:
        await UIHelper.ShowBusyMessageAsync(this, "처리되었습니다", 3000);
        break;

      case MessageBoxResult.No:
        await UIHelper.ShowBusyMessageAsync(this, "취소되었습니다", 3000);
        break;
    }
  });

  #region HeimdallrComboBox 테스트
  public ObservableCollection<HeimdallrComboBoxItem>? EmployeePosition { get; set; } = new();
  /// <summary>
  /// DB 저장용 Nullable int 프로퍼티 (-1을 null로 변환)
  /// </summary>
  public int? SelectedEmployeePositionDb => SelectedEmployeePosition.ToDbValue();

  private int _selectedEmployeePosition;
  public int SelectedEmployeePosition
  {
    get => _selectedEmployeePosition;
    set => SetProperty(ref _selectedEmployeePosition, value);
  }

  private void SetupEnumComboBox()
  {
    EnumComboBoxExtensions.InitializeEnumComboBox<EmployeePosition>(
        comboBoxItems => EmployeePosition = comboBoxItems,
        selectedValue => SelectedEmployeePosition = selectedValue
        // defaultValue 생략하면 Placeholder 보임
    );
  }
  #endregion

  #region AnimatedContentMenu
  private bool _menuVisible;

  public bool MenuVisible
  {
    get => _menuVisible;
    set { _menuVisible = value; RaisePropertyChanged(); }
  }

  public ObservableCollection<MenuItemViewModel>? MenuItems { get; set; }
  private DelegateCommand? _toggleMenuCommand;
  public DelegateCommand? ToggleMenuCommand => _toggleMenuCommand ??= new DelegateCommand(() =>
  {
    MenuVisible = !MenuVisible;
  });

  private DelegateCommand<MenuItemViewModel>? _toggleSubMenuCommand;

  public DelegateCommand<MenuItemViewModel> ToggleSubMenuCommand =>
      _toggleSubMenuCommand ??= new DelegateCommand<MenuItemViewModel>(menu =>
      {
        if (menu == null) return;

        // 상위 메뉴 클릭 시 하위 메뉴 열기/닫기
        menu.IsExpanded = !menu.IsExpanded;

        // 다른 상위 메뉴 닫기 (필요 시)
        foreach (var item in MenuItems!)
        {
          if (item != menu)
            item.IsExpanded = false;
        }
      });

  private void LoadMenuItems()
  {
    MenuItems = new ObservableCollection<MenuItemViewModel>
    {
        new MenuItemViewModel
        {
            Title = "Home",
            IsChecked = true,
            SubItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { Title = "Dashboard" },
                new MenuItemViewModel { Title = "Activity" }
            }
        },
        new MenuItemViewModel
        {
            Title = "Settings",
            SubItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { Title = "General" },
                new MenuItemViewModel { Title = "Security" },
                new MenuItemViewModel { Title = "Privacy" }
            }
        },
        new MenuItemViewModel
        {
            Title = "Profile",
            SubItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { Title = "Edit Profile" },
                new MenuItemViewModel { Title = "Preferences" }
            }
        },
        new MenuItemViewModel { Title = "About" }
    };
  }
  #endregion

  #region ViewModelBase Test
  private string? _viewmodel = string.Empty;
  public string? ViewModel
  {
    get => _viewmodel;
    set => SetPropertyAndValidate(ref _viewmodel, value);
  }

  public DelegateCommand ResetCommand => new DelegateCommand(async () =>
  {
    await ResetAllPropertiesAsync();
  });

  public ObservableCollection<string> NameErrors { get; set; } = new();

  public string NameErrorsText => string.Join(Environment.NewLine, NameErrors);

  private void ValidationMethod()
  {
    // 동기 검증 규칙 추가
    AddValidationRule("ViewModel", () => ValidateName());

    // 비동기 검증 규칙 추가
    AddValidationRuleAsync("ViewModel", async () => await ValidateNameAsync());
  }

  // 동기 검증 함수
  private IEnumerable<string> ValidateName()
  {
    if (string.IsNullOrEmpty(ViewModel))
    {
      return new[] { "ViewModel 은 필수 입력입니다." };
    }

    return Enumerable.Empty<string>();
  }

  // 비동기 검증 함수
  private async Task<IEnumerable<string>> ValidateNameAsync()
  {
    await Task.Delay(500); // 비동기 검증을 시뮬레이션합니다.

    if (string.IsNullOrEmpty(ViewModel))
    {
      // 값이 없으면 동기 검증과 동일한 메시지 반환
      return Enumerable.Empty<string>(); // 또는 ["ViewModel 은 필수 입력입니다."] 반환
    }

    if (ViewModel.Length < 3)
    {
      return new[] { "ViewModel 은 3자 이상이어야 합니다." };
    }

    return Enumerable.Empty<string>();
  }

  // 오류 메세지를 NameErrors 에 반영햐는 메서드
  protected override void SetErrors(string propertyName, IEnumerable<string> errors)
  {
    base.SetErrors(propertyName, errors);

    // 오류 메시지를 NameErrors에 반영
    if (propertyName == "ViewModel")
    {
      NameErrors.Clear(); // 기존 오류를 지우고 새 오류만 추가
      foreach (var error in errors)
      {
        NameErrors.Add(error);
      }

      RaisePropertyChanged(nameof(NameErrorsText));
    }
  }

  public async Task RunTests()
  {
    // ViewModel의 오류 테스트 실행
    //await TestGetErrors();
    //await TestValidateProperty();
    //await TestValidatePropertyCombinedAsync();
    //await TestValidateAllAsync();
    //await TestValidateAllAndReturnAsync();
    //await TestClearErrors();
    //await TestClearAllErrors();
    //await TestRunOnUiThreadAsyncMethods();

    // UI 스레드에서 안전하게 실행
    //await RunOnUiThreadAsync(async () =>
    //{
    //  TestResolveLazy();
    //  BusyMessage += "\n테스트 완료";

    //  await Task.CompletedTask;
    //});

    //await TestClearErrorsAsync(
    //    propertyName: "ViewModel",
    //    initialErrors: new[]
    //    {
    //        "ViewModel은 필수 입력 항목입니다.",
    //        "ViewModel은 3자 이상이어야 합니다."
    //    },
    //    removeCondition: error => error.Contains("필수")
    //);

    //await TestCancellationAsync();
    //await TestDestroy();
    await TestDestroyAsync();
  }

  public async Task TestGetErrors()
  {
    var stringBuilder = new StringBuilder();  // StringBuilder를 사용하여 메시지 결합

    // 1단계: ViewModel에 빈 값을 설정하여 오류 발생
    ViewModel = string.Empty;

    // GetErrorsTyped 호출하여 오류 확인
    var errorsForViewModelEmpty = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Errors for empty ViewModel:");  // 오류 메시지 앞에 설명 추가
    if (errorsForViewModelEmpty.Any())
    {
      foreach (var error in errorsForViewModelEmpty)
      {
        stringBuilder.AppendLine(error);  // 오류 메시지 추가
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for empty ViewModel.");
    }

    // 2단계: ViewModel에 3자 미만 값을 설정하여 오류 발생
    ViewModel = "AB";

    // GetErrorsTyped 호출하여 오류 확인
    var errorsForViewModelShort = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Errors for ViewModel with short value:");
    if (errorsForViewModelShort.Any())
    {
      foreach (var error in errorsForViewModelShort)
      {
        stringBuilder.AppendLine(error);  // 오류 메시지 추가
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for short ViewModel.");
    }

    // 3단계: 정상적인 값으로 설정하여 오류가 없음을 확인
    ViewModel = "ValidValue";

    // GetErrorsTyped 호출하여 오류 확인
    var errorsForValidViewModel = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Errors for valid ViewModel:");
    if (errorsForValidViewModel.Any())
    {
      foreach (var error in errorsForValidViewModel)
      {
        stringBuilder.AppendLine(error);  // 오류 메시지 추가
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for valid ViewModel.");
    }

    // BusyMessage에 최종 결과 할당
    BusyMessage = stringBuilder.ToString();  // StringBuilder의 내용을 문자열로 변환하여 BusyMessage에 할당

    await Task.CompletedTask;
  }

  public async Task TestValidateProperty()
  {
    var stringBuilder = new StringBuilder();  // StringBuilder를 사용하여 메시지 결합

    // 1단계: ViewModel에 빈 값을 설정하여 오류 발생
    ViewModel = string.Empty;

    // 동기 검증과 비동기 검증을 병렬로 실행
    var syncErrorsTask = ValidateProperty("ViewModel");
    var asyncErrorsTask = ValidatePropertyAsync("ViewModel");

    // 두 검증이 모두 끝날 때까지 기다리기
    await Task.WhenAll(syncErrorsTask, asyncErrorsTask);

    // 동기 검증 결과 가져오기
    var syncErrorsForViewModelEmpty = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Sync Errors for empty ViewModel:");
    foreach (var error in syncErrorsForViewModelEmpty)
    {
      stringBuilder.AppendLine(error);
    }

    // 비동기 검증 결과 가져오기
    var asyncErrorsForViewModelEmpty = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Async Errors for empty ViewModel:");
    foreach (var error in asyncErrorsForViewModelEmpty)
    {
      stringBuilder.AppendLine(error);
    }

    // 2단계: ViewModel에 3자 미만 값을 설정하여 오류 발생
    ViewModel = "AB";

    // 동기 검증과 비동기 검증을 병렬로 실행
    syncErrorsTask = ValidateProperty("ViewModel");
    asyncErrorsTask = ValidatePropertyAsync("ViewModel");

    // 두 검증이 모두 끝날 때까지 기다리기
    await Task.WhenAll(syncErrorsTask, asyncErrorsTask);

    // 동기 검증 결과 가져오기
    var syncErrorsForViewModelShort = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Sync Errors for ViewModel with short value:");
    if (syncErrorsForViewModelShort.Any())
    {
      foreach (var error in syncErrorsForViewModelShort)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("No Errors for short ViewModel.");
    }

    // 비동기 검증 결과 가져오기
    var asyncErrorsForViewModelShort = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Async Errors for ViewModel with short value:");
    if (asyncErrorsForViewModelShort.Any())
    {
      foreach (var error in asyncErrorsForViewModelShort)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for short ViewModel.");
    }

    // 3단계: 정상적인 값으로 설정하여 오류가 없음을 확인
    ViewModel = "ValidValue";

    // 동기 검증과 비동기 검증을 병렬로 실행
    syncErrorsTask = ValidateProperty("ViewModel");
    asyncErrorsTask = ValidatePropertyAsync("ViewModel");

    // 두 검증이 모두 끝날 때까지 기다리기
    await Task.WhenAll(syncErrorsTask, asyncErrorsTask);

    // 동기 검증 결과 가져오기
    var syncErrorsForValidViewModel = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Sync Errors for valid ViewModel:");
    if (syncErrorsForValidViewModel.Any())
    {
      foreach (var error in syncErrorsForValidViewModel)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for valid ViewModel.");
    }

    // 비동기 검증 결과 가져오기
    var asyncErrorsForValidViewModel = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("Async Errors for valid ViewModel:");
    if (asyncErrorsForValidViewModel.Any())
    {
      foreach (var error in asyncErrorsForValidViewModel)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("No errors for valid ViewModel.");
    }

    // 최종 결과를 BusyMessage에 할당
    BusyMessage = stringBuilder.ToString();
  }

  public async Task TestValidatePropertyCombinedAsync()
  {
    var stringBuilder = new StringBuilder();

    // 1단계: ViewModel에 빈 값을 설정하여 오류 발생
    ViewModel = string.Empty;

    // 동기 + 비동기 검증 결합 실행
    await ValidatePropertyFullAsync("ViewModel");

    var errorsForViewModelEmpty = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("빈 ViewModel에 대한 오류:");
    if (errorsForViewModelEmpty.Any())
    {
      foreach (var error in errorsForViewModelEmpty)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("빈 ViewModel에 대한 오류가 없습니다.");
    }

    // 2단계: ViewModel에 3자 미만 값을 설정하여 오류 발생
    ViewModel = "AB";

    // 동기 + 비동기 검증 결합 실행
    await ValidatePropertyFullAsync("ViewModel");

    var errorsForViewModelShort = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("3자 미만 ViewModel에 대한 오류:");
    if (errorsForViewModelShort.Any())
    {
      foreach (var error in errorsForViewModelShort)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("3자 미만 ViewModel에 대한 오류가 없습니다.");
    }

    // 3단계: 정상적인 값으로 설정하여 오류가 없음을 확인
    ViewModel = "ValidValue";

    // 동기 + 비동기 검증 결합 실행
    await ValidatePropertyFullAsync("ViewModel");

    var errorsForValidViewModel = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("유효한 ViewModel에 대한 오류:");
    if (errorsForValidViewModel.Any())
    {
      foreach (var error in errorsForValidViewModel)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("유효한 ViewModel에는 오류가 없습니다.");
    }

    // 최종 결과를 BusyMessage 또는 로그에 반영
    BusyMessage = stringBuilder.ToString();
  }

  public async Task TestValidateAllAsync()
  {
    var stringBuilder = new StringBuilder();

    // 1단계: ViewModel에 빈 값을 설정하여 모든 검증 실행
    ViewModel = string.Empty;

    // 모든 속성 검증 실행
    await ValidateAllAsync();

    var errorsEmpty = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("모든 속성 검증 - 빈 ViewModel:");
    if (errorsEmpty.Any())
    {
      foreach (var error in errorsEmpty)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("오류 없음");
    }

    // 2단계: 3자 미만 값 설정 후 검증
    ViewModel = "AB";

    await ValidateAllAsync();

    var errorsShort = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("모든 속성 검증 - 3자 미만 ViewModel:");
    if (errorsShort.Any())
    {
      foreach (var error in errorsShort)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("오류 없음");
    }

    // 3단계: 정상 값 설정 후 검증
    ViewModel = "ValidValue";

    await ValidateAllAsync();

    var errorsValid = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine("모든 속성 검증 - 정상 ViewModel:");
    if (errorsValid.Any())
    {
      foreach (var error in errorsValid)
      {
        stringBuilder.AppendLine(error);
      }
    }
    else
    {
      stringBuilder.AppendLine("오류 없음");
    }

    // 최종 결과를 BusyMessage 또는 로그에 반영
    BusyMessage = stringBuilder.ToString();
  }

  public async Task TestValidateAllAndReturnAsync()
  {
    var stringBuilder = new StringBuilder();

    // 1단계: ViewModel 빈 값
    ViewModel = string.Empty;
    bool resultEmpty = await ValidateAllAndReturnAsync();
    stringBuilder.AppendLine($"모든 속성 검증 후 반환 - 빈 ViewModel: {(resultEmpty ? "오류 없음" : "오류 있음")}");

    // 2단계: 3자 미만 값
    ViewModel = "AB";
    bool resultShort = await ValidateAllAndReturnAsync();
    stringBuilder.AppendLine($"모든 속성 검증 후 반환 - 3자 미만 ViewModel: {(resultShort ? "오류 없음" : "오류 있음")}");

    // 3단계: 정상 값
    ViewModel = "ValidValue";
    bool resultValid = await ValidateAllAndReturnAsync();
    stringBuilder.AppendLine($"모든 속성 검증 후 반환 - 정상 ViewModel: {(resultValid ? "오류 없음" : "오류 있음")}");

    BusyMessage = stringBuilder.ToString();
  }

  public async Task TestClearErrors()
  {
    var stringBuilder = new StringBuilder();

    // 1단계: 검증 규칙 등록
    ValidationMethod();

    // 2단계: ViewModel에 빈 값 설정 → 오류 발생
    ViewModel = string.Empty;
    await ValidatePropertyFullAsync("ViewModel");

    stringBuilder.AppendLine("초기 오류 상태:");
    stringBuilder.AppendLine(string.Join(Environment.NewLine, GetErrorsTyped("ViewModel")));

    // 3단계: 특정 속성 오류 제거
    ClearErrors("ViewModel");
    stringBuilder.AppendLine("\nClearErrors 호출 후 ViewModel 오류 상태:");
    var errorsAfterClear = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine(errorsAfterClear.Any() ? string.Join(Environment.NewLine, errorsAfterClear) : "오류 없음");

    BusyMessage = stringBuilder.ToString();
  }

  public async Task TestClearAllErrors()
  {
    var stringBuilder = new StringBuilder();

    // 검증 규칙 등록
    ValidationMethod();

    // 여러 단계의 오류 발생
    ViewModel = string.Empty;
    await ValidatePropertyFullAsync("ViewModel");

    stringBuilder.AppendLine("초기 오류 상태:");
    stringBuilder.AppendLine(string.Join(Environment.NewLine, GetErrorsTyped("ViewModel")));

    // 모든 오류 제거
    ClearAllErrors();
    stringBuilder.AppendLine("\nClearAllErrors 호출 후 모든 오류 상태:");
    var errorsAfterClearAll = GetErrorsTyped("ViewModel");
    stringBuilder.AppendLine(errorsAfterClearAll.Any() ? string.Join(Environment.NewLine, errorsAfterClearAll) : "오류 없음");

    BusyMessage = stringBuilder.ToString();
  }

  private string UiThreadTestResult = string.Empty;

  public async Task TestRunOnUiThreadAsyncMethods()
  {
    var stringBuilder = new StringBuilder();

    // 1. 동기 작업 테스트
    RunOnUiThread(() =>
    {
      UiThreadTestResult = $"동기 작업 실행됨 - ThreadId: {Thread.CurrentThread.ManagedThreadId}";
    });

    stringBuilder.AppendLine(UiThreadTestResult);

    // 2. 비동기 작업 테스트
    await RunOnUiThreadAsync(async () =>
    {
      await Task.Delay(100); // 비동기 시뮬레이션
      UiThreadTestResult = $"비동기 작업 실행됨 - ThreadId: {Thread.CurrentThread.ManagedThreadId}";
    });

    stringBuilder.AppendLine(UiThreadTestResult);

    // 최종 결과 BusyMessage에 반영
    BusyMessage = stringBuilder.ToString();
  }

  public void TestResolveLazy()
  {
    _ = RunOnUiThreadAsync(async () =>
    {
      var stringBuilder = new StringBuilder();

      // 1. Lazy 객체 생성 (실제 인스턴스는 아직 생성되지 않음)
      Lazy<IExampleService> lazyService = ResolveLazy<IExampleService>();
      stringBuilder.AppendLine("Lazy 객체 생성 완료, 아직 Value 접근 전");

      // BusyMessage 업데이트 (접근 전 상태)
      BusyMessage = stringBuilder.ToString();

      // 잠깐 대기 → UI가 업데이트될 시간 확보
      await Task.Delay(100);

      // 2. Value 접근 → 실제 인스턴스 생성
      IExampleService service = lazyService.Value;
      stringBuilder.AppendLine($"Lazy Value 접근 후 메시지: {service.GetMessage()}");

      // 최종 BusyMessage 업데이트 (접근 후 상태)
      BusyMessage = stringBuilder.ToString();
    });
  }

  private async Task TestClearErrorsAsync(
    string propertyName,
    IEnumerable<string> initialErrors,
    Func<string, bool>? removeCondition = null)
  {
    var sb = new StringBuilder();

    sb.AppendLine("[ClearErrors 테스트 시작]");

    // 1️⃣ 테스트용 오류 강제 주입
    SetErrors(propertyName, initialErrors);

    sb.AppendLine($"초기 오류 개수: {HasErrorsCount}");

    // 2️⃣ 오류 제거
    ClearErrors(propertyName, removeCondition);

    sb.AppendLine($"ClearErrors 후 오류 개수: {HasErrorsCount}");

    BusyMessage = sb.ToString();

    await Task.CompletedTask;
  }

  private async Task TestCancellationAsync()
  {
    const string taskName = "LongRunningTest";

    var sb = new StringBuilder();
    sb.AppendLine("[Cancellation 테스트 시작]");

    try
    {
      // 1️⃣ CancellationToken 발급
      var token = GetCancellationToken(taskName, timeout: TimeSpan.FromSeconds(5));
      sb.AppendLine("CancellationToken 생성 완료");

      // 2️⃣ 취소 가능한 장기 작업 시작
      var longTask = Task.Run(async () =>
      {
        sb.AppendLine("작업 시작");

        for (int i = 1; i <= 10; i++)
        {
          token.ThrowIfCancellationRequested();

          sb.AppendLine($"작업 진행 중... {i}/10");
          await Task.Delay(500, token);
        }

        sb.AppendLine("작업 정상 완료 (❌ 취소 안됨)");
      }, token);

      // 3️⃣ 잠시 대기 후 작업 취소
      await Task.Delay(1200);
      await CancelTaskAsync(taskName);

      sb.AppendLine("CancelTaskAsync 호출됨");

      // 4️⃣ 작업 종료 대기
      await longTask;
    }
    catch (OperationCanceledException)
    {
      sb.AppendLine("✔ 작업이 정상적으로 취소됨 (OperationCanceledException)");
    }
    catch (Exception ex)
    {
      sb.AppendLine($"❌ 예외 발생: {ex.GetType().Name} - {ex.Message}");
    }
    finally
    {
      BusyMessage = sb.ToString();
    }
  }

  private readonly StringBuilder _destroyLog = new();
  //protected override void OnViewModelDestroyed()
  //{
  //  _destroyLog.AppendLine("✔ OnViewModelDestroyed 호출됨");
  //}

  protected override Task OnViewModelDestroyedAsync()
  {
    _destroyLog.AppendLine("✔ OnViewModelDestroyedAsync 호출됨");
    return Task.CompletedTask;
  }

  private async Task TestDestroy()
  {
    const string taskName = "DestroyTestTask";

    var sb = new StringBuilder();
    sb.AppendLine("[Destroy 테스트 시작]");

    try
    {
      // 1️⃣ 장기 작업 시작
      var token = GetCancellationToken(taskName);

      var longTask = Task.Run(async () =>
      {
        sb.AppendLine("장기 작업 시작");

        for (int i = 1; i <= 10; i++)
        {
          token.ThrowIfCancellationRequested();

          sb.AppendLine($"작업 진행 중... {i}/10");
          await Task.Delay(500, token);
        }

        sb.AppendLine("❌ 작업이 끝까지 실행됨 (비정상)");
      }, token);

      // 2️⃣ 일부 진행 후 Destroy 호출
      await Task.Delay(1200);
      sb.AppendLine("Destroy() 호출");

      Destroy();

      // 3️⃣ 작업 종료 대기
      await longTask;
    }
    catch (OperationCanceledException)
    {
      sb.AppendLine("✔ Destroy 호출로 작업이 정상 취소됨");
    }
    catch (Exception ex)
    {
      sb.AppendLine($"❌ 예외 발생: {ex.GetType().Name} - {ex.Message}");
    }
    finally
    {
      sb.Append(_destroyLog.ToString());
      BusyMessage = sb.ToString();
    }
  }

  public async Task TestDestroyAsync()
  {
    var sb = new StringBuilder();
    _destroyLog.Clear();

    sb.AppendLine("[DestroyAsync 테스트 시작]");

    _ = Task.Run(async () =>
    {
      sb.AppendLine("장기 작업 시작");

      try
      {
        for (int i = 1; i <= 10; i++)
        {
          await Task.Delay(300, GetCancellationToken("LongTask"));
          sb.AppendLine($"작업 진행 중... {i}/10");
        }

        sb.AppendLine("❌ 작업이 정상 종료됨 (취소되지 않음)");
      }
      catch (OperationCanceledException)
      {
        sb.AppendLine("✔ DestroyAsync 호출로 작업이 정상 취소됨");
      }
    });

    // 🔥 핵심: 작업이 끝나기 전에 취소
    await Task.Delay(700); // 2~3회 출력 후

    sb.AppendLine("DestroyAsync() 호출");
    await DestroyAsync();

    await RunOnUiThreadAsync(async () =>
    {
      BusyMessage = sb.ToString() + Environment.NewLine + _destroyLog;
      await Task.Delay(3000);
      BusyMessage = string.Empty;
    });
  }

  public override void OnNavigatedTo(NavigationContext navigationContext)
  {
    base.OnNavigatedTo(navigationContext);
    _ = RunTests();
  }
  #endregion

  #region

  #region DialogTest
  private DelegateCommand? _openDialogCommand;
  public DelegateCommand? OpenDialogCommand =>
      _openDialogCommand ??= new DelegateCommand(OpenDialog);

  private void OpenDialog()
  {
    var parameters = new DialogParameters
        {
            { "Message", "Test Dialog Opened!" }
        };

    _dialogService.Value.ShowDialog("TestDialogView", parameters, r =>
    {
      if (r.Result == ButtonResult.OK)
      {
        // 다이얼로그가 OK로 닫힌 경우
      }
    });
  }
  #endregion
  #endregion

  #region LoadingOverlay
  private bool _isLoading;
  public bool IsLoading
  {
    get => _isLoading;
    set => SetProperty(ref _isLoading, value);
  }

  private DelegateCommand? _toggleBusyCommand;
  public DelegateCommand? ToggleBusyCommand =>
      _toggleBusyCommand ??= new DelegateCommand(async () =>
      {
        // IsBusy를 true로 변경 → LoadingOverlay 표시
        IsLoading = true;
        await UIHelper.ShowBusyMessageAsync(this, "데이터 로딩 중...", 2000);
        //BusyMessage = "데이터 로딩 중...";

        // 테스트용 딜레이 (예: API 호출)
        //await Task.Delay(3000);

        // 완료 후 IsBusy를 false로 변경 → LoadingOverlay 숨김
        IsLoading = false;
        BusyMessage = "완료되었습니다.";
      });
  #endregion

  #region HeimdallrProgressBar 테스트
  private double _value;
  private DispatcherTimer? _timer;
  private double _maximum = 100;
  private double _minimum = 0;

  // ProgressBar Value
  public double Value
  {
    get => _value;
    set
    {
      if (_value != value)
      {
        _value = value;
        RaisePropertyChanged(nameof(Value));
      }
    }
  }

  // ProgressBar Maximum
  public double Maximum
  {
    get => _maximum;
    set
    {
      if (_maximum != value)
      {
        _maximum = value;
        RaisePropertyChanged(nameof(Maximum));
      }
    }
  }

  // ProgressBar Minimum
  public double Minimum
  {
    get => _minimum;
    set
    {
      if (_minimum != value)
      {
        _minimum = value;
        RaisePropertyChanged(nameof(Minimum));
      }
    }
  }

  private void InitializeTimer()
  {
    _timer = new DispatcherTimer
    {
      Interval = TimeSpan.FromMilliseconds(10)
    };
    _timer.Tick += (s, e) =>
    {
      if (Value < Maximum)
      {
        Value += 1; // 진행률 증가
      }
      else
      {
        _timer.Stop(); // 최대값 도달 시 타이머 중지
      }
    };
  }

  private DelegateCommand? _startCommand;
  public DelegateCommand StartCommand => _startCommand ??= new DelegateCommand(() =>
  {
    Value = Minimum; // 초기값 설정
    _timer!.Start();
  });

  #endregion

  #region ContextMenuBackbround 테스트
  private ContextMenuVisualState _contextMenuState = ContextMenuVisualState.Normal;
  public ContextMenuVisualState ContextMenuState
  {
    get => _contextMenuState;
    set
    {
      if (SetProperty(ref _contextMenuState, value))
        UpdateContextMenuVisual();
    }
  }

  private void UpdateContextMenuVisual()
  {
    ContextMenuBackgroundColor = ContextMenuState switch
    {
      ContextMenuVisualState.Hover => Brushes.Goldenrod,
      ContextMenuVisualState.Pressed => Brushes.OrangeRed,
      ContextMenuVisualState.Disabled => Brushes.DimGray,
      _ => Brushes.DarkSlateGray
    };
  }

  private Brush _contextMenuBackgroundColor =
      new SolidColorBrush(Color.FromRgb(30, 30, 30));

  public Brush ContextMenuBackgroundColor
  {
    get => _contextMenuBackgroundColor;
    set => SetProperty(ref _contextMenuBackgroundColor, value);
  }

  private bool _toggle;

  private DelegateCommand? _onContextMenuThemeTestCommand;
  public DelegateCommand? OnContextMenuThemeTestCommand =>
      _onContextMenuThemeTestCommand ??= new DelegateCommand(() =>
      {
        _toggle = !_toggle;

        ContextMenuBackgroundColor = _toggle
          ? new SolidColorBrush(Colors.Red)   // 테스트 색상
          : new SolidColorBrush(Colors.Yellow);  // DarkEbony
      });

  #endregion

  #region Dimming 테스트

  private DelegateCommand<string>? _changeDimmingColorCommand;
  public DelegateCommand<string> ChangeDimmingColorCommand =>
      _changeDimmingColorCommand ??= new DelegateCommand<string>(colorCode =>
      {
        if (string.IsNullOrWhiteSpace(colorCode)) return;

        // 현재 활성 윈도우 가져오기
        if (Application.Current.Windows.OfType<BaseThemeWindow>().FirstOrDefault() is BaseThemeWindow window)
        {
          // 색상 적용
          window.DimmingColor = (Brush)new BrushConverter().ConvertFromString(colorCode)!;

          // 다이밍 활성화
          window.Dimming = true;

          // 흐림 정도 조절 (원하는 만큼 낮춤, 기본은 0.5 정도 추천)
          window.DimmingOpacity = 0.2;
        }
      });

  private DelegateCommand? _toggleDimmingCommand;
  public DelegateCommand ToggleDimmingCommand =>
      _toggleDimmingCommand ??= new DelegateCommand(() =>
      {
        if (Application.Current.Windows.OfType<BaseThemeWindow>().FirstOrDefault() is BaseThemeWindow window)
        {
          window.Dimming = !window.Dimming;
        }
      });

  #endregion

  #region ThemeManager 테스트
  public DelegateCommand<string> ChangeThemeCommand => new DelegateCommand<string>(theme =>
  {
    ThemeManager.ChangedTheme(theme);
  });
  #endregion

  #region HeimdallrSlider 테스트
  private double _sliderValue;

  public double SliderValue
  {
    get => _sliderValue;
    set
    {
      if (SetProperty(ref _sliderValue, value))
      {
        RaisePropertyChanged(nameof(DisplayText));
      }
    }
  }

  public string DisplayText => $"값 변경 테스트: {SliderValue:F0}";
  #endregion

  #region FocusVisualHelper 테스트
  private Brush _primaryBrush = Brushes.Red;
  private Thickness _primaryThickness = new Thickness(3);
  private Brush _secondaryBrush = Brushes.Yellow;
  private Thickness _secondaryThickness = new Thickness(1);
  private Thickness _focusMargin = new Thickness(2);

  public Brush ButtonPrimaryBrush
  {
    get => _primaryBrush;
    set { _primaryBrush = value; RaisePropertyChanged(); }
  }

  public Thickness ButtonPrimaryThickness
  {
    get => _primaryThickness;
    set { _primaryThickness = value; RaisePropertyChanged(); }
  }

  public Brush ButtonSecondaryBrush
  {
    get => _secondaryBrush;
    set { _secondaryBrush = value; RaisePropertyChanged(); }
  }

  public Thickness ButtonSecondaryThickness
  {
    get => _secondaryThickness;
    set { _secondaryThickness = value; RaisePropertyChanged(); }
  }

  public Thickness ButtonFocusMargin
  {
    get => _focusMargin;
    set { _focusMargin = value; RaisePropertyChanged(); }
  }

  public bool UseSystemFocusVisuals { get; set; } = false;
  public bool IsTemplateFocusTarget { get; set; } = true;

  // 버튼 클릭 Command
  public ICommand ButtonClickCommand => new DelegateCommand<string>(buttonName =>
  {
    MessageBox.Show($"{buttonName} 클릭됨!");
  });
  #endregion

  #region TreeView 테스트
  public ObservableCollection<TreeItemModel>? Items { get; set; }
  private void HeimdallrTreeViewInitialize()
  {
    Items = new ObservableCollection<TreeItemModel>
        {
            new TreeItemModel
            {
                Text = "Root Item 1",
                Children = new ObservableCollection<TreeItemModel>
                {
                    new TreeItemModel { Text = "Child 1" },
                    new TreeItemModel { Text = "Child 2" }
                }
            },
            new TreeItemModel
            {
                Text = "Root Item 2",
                Children = new ObservableCollection<TreeItemModel>
                {
                    new TreeItemModel { Text = "Child A" },
                    new TreeItemModel { Text = "Child B" }
                }
            }
        };
  }
  #endregion

  #region ListView 테스트
  public ObservableCollection<ListViewModel> ListViewTest { get; } =
        new ObservableCollection<ListViewModel>
        {
            new ListViewModel { Name = "홍길동", Age = 30, Department = "개발" },
            new ListViewModel { Name = "김철수", Age = 25, Department = "기획" },
            new ListViewModel { Name = "이영희", Age = 28, Department = "디자인" }
        };
  #endregion

  #region SliderContentPanel 테스트
  private bool _isOpen;
  public bool IsOpen { get => _isOpen; set => SetProperty(ref _isOpen, value); }

  public DelegateCommand SliderContentPanelCommand => new DelegateCommand(() => { IsOpen = true; });
  #endregion
}


#region AnimatedContentMenu
public class MenuItemViewModel : INotifyPropertyChanged
{
  private string? _title;
  private bool _isChecked;

  public string? Title
  {
    get => _title;
    set { _title = value; OnPropertyChanged(); }
  }

  public bool IsChecked
  {
    get => _isChecked;
    set { _isChecked = value; OnPropertyChanged(); }
  }

  public ObservableCollection<MenuItemViewModel>? SubItems { get; set; } = new();

  // 하위 메뉴 표시 여부
  private bool _isExpanded;
  public bool IsExpanded
  {
    get => _isExpanded;
    set { _isExpanded = value; OnPropertyChanged(); }
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  protected void OnPropertyChanged([CallerMemberName] string? name = null)
      => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  #endregion

}

// 예시 서비스 인터페이스
public interface IExampleService
{
  string GetMessage();
}

// 예시 서비스 구현
public class ExampleService : IExampleService
{
  public ExampleService()
  {
    Debug.WriteLine("ExampleService 생성자 호출");
  }

  public string GetMessage() => "서비스 실행됨";
}

#region TreeViewItem Model
public class TreeItemModel
{
  public string? Text { get; set; }
  public ObservableCollection<TreeItemModel>? Children { get; set; }
  public bool IsExpanded { get; set; }
  public bool IsSelected { get; set; }
}
#endregion

#region ListView Model
public class ListViewModel
{
  public string? Name { get; set; }
  public int Age { get; set; }
  public string? Department { get; set; }
}
#endregion