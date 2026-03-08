using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// HeimdallrDropdownContentMenu는 드롭다운 방식의 콘텐츠 컨트롤입니다.
/// 내부에 Popup과 Toggle(CheckBox)을 포함하며, 사용자가 토글 버튼을 클릭하면 Popup이 열립니다.
/// </summary>
[TemplatePart(Name = PART_POPUP_NAME, Type = typeof(Popup))]
[TemplatePart(Name = PART_TOGGLE_NAME, Type = typeof(CheckBox))]
public class HeimdallrDropdownContentMenu : ContentControl
{
  #region 필드 
  // 템플릿에서 사용할 PART 이름 정의
  private const string PART_POPUP_NAME = "PART_Popup";
  private const string PART_TOGGLE_NAME = "PART_Toggle";

  // 실제 템플릿 내부 요소 참조
  private Popup? _popup;
  private CheckBox? _toggle;
  #endregion

  #region 생성자
  /// <summary>
  /// 정적 생성자: 기본 스타일 키를 설정하여 Generic.xaml 스타일과 연결합니다.
  /// </summary>
  static HeimdallrDropdownContentMenu()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrDropdownContentMenu),
      new FrameworkPropertyMetadata(typeof(HeimdallrDropdownContentMenu)));
  }
  #endregion

  #region IsOpen
  /// <summary>
  /// 드롭다운 팝업이 열려 있는지 여부를 나타냅니다.
  /// 외부 바인딩 또는 내부 이벤트로 값을 제어합니다.
  /// </summary>
  public bool IsOpen
  {
    get { return (bool)GetValue(IsOpenProperty); }
    set { SetValue(IsOpenProperty, value); }
  }

  /// <summary>
  /// 의존 속성 등록: IsOpen
  /// </summary>
  public static readonly DependencyProperty IsOpenProperty =
      DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(HeimdallrDropdownContentMenu),
          new FrameworkPropertyMetadata(false,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  #endregion

  #region IconType
  /// <summary>
  /// HeimdallrIcon에 사용할 아이콘 PathGeometry 유형을 지정합니다.
  /// (예: 펼침 화살표, 사용자 아이콘 등)
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(PathIconProperty);
    set => SetValue(PathIconProperty, value);
  }

  /// <summary>
  /// 의존 속성 등록: Icon
  /// </summary>
  public static readonly DependencyProperty PathIconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(IconType.None));
  #endregion

  #region IconFill
  /// <summary>
  /// HeimdallrIcon의 색상을 지정합니다. 기본색상는 회색(Gray)입니다.
  /// </summary>
  public Brush IconFill
  {
    get { return (Brush)GetValue(IconFillProperty); }
    set { SetValue(IconFillProperty, value); }
  }

  /// <summary>
  /// 의존 속성 등록: Fill
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrDropdownContentMenu),
        new PropertyMetadata(Brushes.Gray));
  #endregion

  #region IconSize
  /// <summary>
  /// 이이콘 사이즈 너비,높이
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  /// <summary>
  /// 아이콘사이즈 기본값
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register(nameof(IconSize), typeof(double),
          typeof(HeimdallrDropdownContentMenu), new PropertyMetadata(24.0));
  #endregion

  #region IconMouseOverFill
  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }
  public static readonly DependencyProperty IconMouseOverFillProperty =
      DependencyProperty.Register(nameof(IconMouseOverFill), typeof(Brush), typeof(HeimdallrDropdownContentMenu), new PropertyMetadata(Brushes.DeepSkyBlue));
  #endregion

  #region Placement
  /// <summary>
  /// 팝업이 표시될 위치입니다. 기본값은 Bottom입니다.
  /// </summary>
  public PlacementMode Placement
  {
    get => (PlacementMode)GetValue(PlacementProperty);
    set => SetValue(PlacementProperty, value);
  }
  /// <summary>
  /// 의존 속성 등록: Placement
  /// 변경 시 Popup의 실제 Placement 속성도 업데이트됩니다.
  /// </summary>
  public static readonly DependencyProperty PlacementProperty =
      DependencyProperty.Register(nameof(Placement), typeof(PlacementMode), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(PlacementMode.Bottom, OnPlacementChanged));
  #endregion

  #region StaysOpenWhenMouseOver
  /// <summary>
  /// 마우스가 Toggle 위에 있을 때 Popup을 닫지 않도록 유지할지 여부를 지정합니다.
  /// </summary>
  public bool StaysOpenWhenMouseOver
  {
    get => (bool)GetValue(StaysOpenWhenMouseOverProperty);
    set => SetValue(StaysOpenWhenMouseOverProperty, value);
  }
  /// <summary>
  /// 의존 속성 등록: StaysOpenWhenMouseOver
  /// </summary>
  public static readonly DependencyProperty StaysOpenWhenMouseOverProperty =
      DependencyProperty.Register(nameof(StaysOpenWhenMouseOver), typeof(bool), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(false));
  #endregion

  #region ClosedCommand
  /// <summary>
  /// 팝업이 닫힐 때 실행할 ICommand입니다.
  /// </summary>
  public ICommand? ClosedCommand
  {
    get => (ICommand?)GetValue(ClosedCommandProperty);
    set => SetValue(ClosedCommandProperty, value);
  }
  /// <summary>
  /// 의존 속성 등록: ClosedCommand
  /// </summary>
  public static readonly DependencyProperty ClosedCommandProperty =
      DependencyProperty.Register(nameof(ClosedCommand), typeof(ICommand), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(null));
  #endregion

  #region 문자열표기
  /// <summary>
  /// 버튼 영역에 표시할 텍스트입니다 (아이콘 오른쪽)
  /// </summary>
  public string Label
  {
    get => (string)GetValue(LabelProperty);
    set => SetValue(LabelProperty, value);
  }
  /// <summary>
  /// 기본값은 빈 문자열입니다.
  /// </summary>
  public static readonly DependencyProperty LabelProperty =
      DependencyProperty.Register(nameof(Label), typeof(string), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(string.Empty));
  #endregion

  #region 체크박스 백그라운드
  /// <summary>
  /// 드롭다운 토글 버튼(체크박스)의 배경 브러시입니다.
  /// </summary>
  public Brush ToggleBackground
  {
    get => (Brush)GetValue(ToggleBackgroundProperty);
    set => SetValue(ToggleBackgroundProperty, value);
  }
  /// <summary>
  /// 기본값은 투명(Transparent)입니다.
  /// </summary>
  public static readonly DependencyProperty ToggleBackgroundProperty =
      DependencyProperty.Register(nameof(ToggleBackground), typeof(Brush), typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(Brushes.Transparent));
  #endregion

  #region AutoCloseOnClick 메뉴클릭시 팝업창 자동 닫힘
  public bool AutoCloseOnClick
  {
    get => (bool)GetValue(AutoCloseOnClickProperty);
    set => SetValue(AutoCloseOnClickProperty, value);
  }

  public static readonly DependencyProperty AutoCloseOnClickProperty =
      DependencyProperty.Register(nameof(AutoCloseOnClick),
          typeof(bool),
          typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(true));
  #endregion

  #region CloseOnOutsideClick
  /// <summary>
  /// Popup 외부를 클릭하면 자동으로 닫힐지 여부
  /// </summary>
  public bool CloseOnOutsideClick
  {
    get => (bool)GetValue(CloseOnOutsideClickProperty);
    set => SetValue(CloseOnOutsideClickProperty, value);
  }

  public static readonly DependencyProperty CloseOnOutsideClickProperty =
      DependencyProperty.Register(nameof(CloseOnOutsideClick),
          typeof(bool),
          typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(true));
  #endregion

  #region PopupAnimation
  public PopupAnimation PopupAnimation
  {
    get => (PopupAnimation)GetValue(PopupAnimationProperty);
    set => SetValue(PopupAnimationProperty, value);
  }

  public static readonly DependencyProperty PopupAnimationProperty =
      DependencyProperty.Register(nameof(PopupAnimation),
          typeof(PopupAnimation),
          typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(PopupAnimation.Fade));
  #endregion

  #region VerticalOffset 팝업창 위/아래 이동
  public double VerticalOffset
  {
    get => (double)GetValue(VerticalOffsetProperty);
    set => SetValue(VerticalOffsetProperty, value);
  }

  public static readonly DependencyProperty VerticalOffsetProperty =
      DependencyProperty.Register(nameof(VerticalOffset),
          typeof(double),
          typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(0.0));
  #endregion

  #region HorizontalOffset 팝업창 좌/우 이동
  /// <summary>
  /// Popup의 좌우 위치 보정
  /// </summary>
  public double HorizontalOffset
  {
    get => (double)GetValue(HorizontalOffsetProperty);
    set => SetValue(HorizontalOffsetProperty, value);
  }

  public static readonly DependencyProperty HorizontalOffsetProperty =
      DependencyProperty.Register(
          nameof(HorizontalOffset),
          typeof(double),
          typeof(HeimdallrDropdownContentMenu),
          new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

  private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrDropdownContentMenu menu && menu._popup != null)
    {
      menu._popup.HorizontalOffset = (double)e.NewValue;
    }
  }
  #endregion

  #region OnApplyTemplate 재정의 메서드
  /// <summary>
  /// 템플릿이 적용된 후 호출됩니다.
  /// Popup과 Toggle(CheckBox)을 템플릿에서 가져와 초기화하고, 이벤트를 설정합니다.
  /// </summary>

  private Window? _window;
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // Popup 참조 획득 및 이벤트 연결
    _popup = Template.FindName(PART_POPUP_NAME, this) as Popup;

    if (_popup != null)
    {
      // 중복 등록 방지
      _popup.Closed -= Popup_Closed;
      _popup.Closed += Popup_Closed;

      // 바인딩된 Placement 초기 적용
      _popup.Placement = Placement;

      // ⭐ Offset 적용
      _popup.HorizontalOffset = HorizontalOffset;
      _popup.VerticalOffset = VerticalOffset;
    }

    // 토글 버튼 (CheckBox) 참조
    _toggle = Template.FindName(PART_TOGGLE_NAME, this) as CheckBox;

    // ⭐ 메뉴 클릭 감지 추가
    AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnMenuItemClicked), true);

    // ⭐ Outside Click 감지
    _window = Window.GetWindow(this);

    if (_window != null)
    {
      _window.PreviewMouseDown -= Window_PreviewMouseDown;
      _window.PreviewMouseDown += Window_PreviewMouseDown;
    }
  }
  #endregion

  #region
  private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (!CloseOnOutsideClick)
      return;

    if (!IsOpen)
      return;

    if (e.OriginalSource is DependencyObject dep)
    {
      if (IsDescendantOf(dep, this))
        return;

      if (_popup != null && IsDescendantOf(dep, _popup))
        return;
    }

    IsOpen = false;
  }
  #endregion

  #region Menu Click Close
  private void OnMenuItemClicked(object sender, RoutedEventArgs e)
  {
    if (!AutoCloseOnClick)
      return;

    if (!IsOpen)
      return;

    // Toggle 클릭이면 무시
    if (_toggle != null && _toggle.IsMouseOver)
      return;

    // 실제 메뉴 버튼만 처리
    if (e.OriginalSource is DependencyObject dep)
    {
      var button = FindParent<ButtonBase>(dep);

      if (button == null)
        return;
    }

    IsOpen = false;
  }
  #endregion

  #region Helper Method
  private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
  {
    DependencyObject parent = VisualTreeHelper.GetParent(child);

    while (parent != null)
    {
      if (parent is T typed)
        return typed;

      parent = VisualTreeHelper.GetParent(parent);
    }

    return null;
  }

  private static bool IsDescendantOf(DependencyObject child, DependencyObject parent)
  {
    while (child != null)
    {
      if (child == parent)
        return true;

      child = VisualTreeHelper.GetParent(child);
    }

    return false;
  }
  #endregion

  #region Popup_Closed 메서드
  /// <summary>
  /// Popup이 닫힐 때 호출되는 이벤트 핸들러입니다.
  /// 필요 시 IsOpen을 false로 설정하고 ClosedCommand를 실행합니다.
  /// </summary>
  private void Popup_Closed(object? sender, EventArgs e)
  {
    if (StaysOpenWhenMouseOver && (_toggle?.IsMouseOver == true || _popup?.IsMouseOver == true))
    {
      // 마우스가 토글 위에 있으면 닫지 않음
      return;
    }

    IsOpen = false;

    if (ClosedCommand?.CanExecute(null) == true)
    {
      ClosedCommand.Execute(null);
    }
  }
  #endregion

  #region OnPlacementChanged 메서드
  private static void OnPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrDropdownContentMenu menu && menu._popup != null)
    {
      // Popup의 Placement 속성을 변경합니다.
      menu._popup.Placement = (PlacementMode)e.NewValue;
    }
  }
  #endregion

  #region OnPropertyChanged 재정의 메서드
  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);

    if (e.Property == IsOpenProperty && _toggle != null)
    {
      if (_toggle.IsChecked != IsOpen)
        _toggle.IsChecked = IsOpen;
    }
  }
  #endregion

  #region OnKeyDown 키보드 접근성 재정의 메서드
  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
    {
      IsOpen = false;
      e.Handled = true;
    }

    base.OnKeyDown(e);
  }
  #endregion

  #region OnPreviewKeyDown ESC 키로 Popup 닫기
  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);

    if (e.Key == Key.Escape && IsOpen)
    {
      IsOpen = false;
      e.Handled = true;
    }
  }
  #endregion
}

