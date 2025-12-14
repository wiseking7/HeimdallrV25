using Heimdallr.Domain.Enums;
using Heimdallr.UI.Enums;
using Heimdallr.UI.Helpers;
using Heimdallr.Utility.Helpers;
using Heimdallr.Utility.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Heimdallr.App.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
  private string? _busyMessage;
  public string? BusyMessage
  {
    get => _busyMessage;
    set => SetProperty(ref _busyMessage, value);
  }

  private DelegateCommand? _messageCommand;
  public DelegateCommand? MessageCommand => _messageCommand ??= new DelegateCommand(async () =>
  {
    // 1. HeimdallrMessageBox 호출 (UI 스레드 안전)
    var result = UIHelper.ShowMessageBox(
        "삭제하시겠습니까",
        "CHECK",
        HeimdallrMessageBoxButtonEnum.YesNo,
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

  #region 생성자
  public MainWindowViewModel(IContainerProvider container) : base(container)
  {
    SetupEnumComboBox();
    LoadMenuItems();

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
  private string? _name = string.Empty;
  public string? Name
  {
    get => _name;
    set => SetPropertyAndValidate(ref _name, value);
  }

  public ObservableCollection<string> NameErrors { get; set; } = new();

  #endregion

  /// <summary>속성 검증 실행</summary>
  protected override async Task ValidatePropertyAsync(string propertyName)
  {
    await base.ValidatePropertyAsync(propertyName);  // 비동기적으로 부모의 검증을 처리

    // 특정 속성 검증
    if (propertyName == nameof(Name))
    {
      // UI 스레드에서 ObservableCollection 갱신 (동기적으로 처리)
      ExecuteOnUiThread(() =>
      {
        NameErrors.Clear();
        // 오류가 있을 경우 ObservableCollection에 추가
        foreach (var err in GetErrors(nameof(Name)).Cast<string>())
        {
          NameErrors.Add(err);
        }
      });
    }
  }
}