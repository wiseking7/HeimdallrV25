using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrPlaceholderPasswordBox : Control
{
  /// 비밀번호 표시/숨기기를 위한 커맨드 정의 (예: 눈 모양 아이콘 클릭 시)
  public static readonly RoutedCommand ToggleShowPasswordCommand = new RoutedCommand();

  #region Template Parts
  // 템플릿에서 찾을 컨트롤들
  private PasswordBox? _passwordBox;   // 실제 비밀번호 입력용 박스 (● 문자로 표시)
  private TextBox? _textBox;           // 비밀번호 보기 상태일 때 사용되는 일반 TextBox
  #endregion

  #region 생성자 (정적 생성자, CommandManage 바인딩 등록)
  // 기본 스타일 템플릿을 연결하기 위해 오버라이드 (Generic.xaml 등)
  static HeimdallrPlaceholderPasswordBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrPlaceholderPasswordBox),
      new FrameworkPropertyMetadata(typeof(HeimdallrPlaceholderPasswordBox)));
  }

  /// <summary>
  /// 인스턴스 생성자: 커맨드 바인딩 등록 및 이벤트 처리
  /// </summary>
  public HeimdallrPlaceholderPasswordBox()
  {
    // 비밀번호 보이기 아이콘을 클릭하면 비밀번호가 보이기 ToggleShowPasswordCommand 실행 시 OnToggleShowPassword 메서드 호출
    CommandBindings.Add(new CommandBinding(ToggleShowPasswordCommand, OnToggleShowPassword));

    // ToolTip이 열릴 때 커스텀 HeimdallrToolTip으로 변환
    ToolTipOpening += HeimdallrPlaceholderPasswordBox_ToolTipOpening;

    // Unloaded 이벤트 시 이벤트 해제 (메모리 누수 방지)
    Unloaded += (s, e) =>
    {
      ToolTipOpening -= HeimdallrPlaceholderPasswordBox_ToolTipOpening;
    };
  }
  #endregion

  #region HeimdallrPlaceholderPasswordBox_ToolTipOpening 이벤트
  private void HeimdallrPlaceholderPasswordBox_ToolTipOpening(object sender, ToolTipEventArgs e)
  {
    // ToolTip이 없으면 열리지 않게 처리
    if (ToolTip == null)
    {
      e.Handled = true;
      return;
    }

    // 이미 HeimdallrToolTip이면 그대로 사용
    if (ToolTip is HeimdallrToolTip)
      return;

    // 문자열이면 HeimdallrToolTip로 변환
    if (ToolTip is string tooltipText && !string.IsNullOrWhiteSpace(tooltipText))
    {
      ToolTip = new HeimdallrToolTip
      {
        Content = tooltipText
      };
    }
    else
    {
      // 빈 문자열이거나 알 수 없는 타입이면 표시 안 함
      e.Handled = true;
    }
  }
  #endregion

  #region 커맨드 처리기 ToggleShowPasswordCommand 실행 시 ShowPassword 속성 토글
  private void OnToggleShowPassword(object sender, ExecutedRoutedEventArgs e)
  {
    // ShowPassword 값을 인스턴스 별로 토글
    ShowPassword = !ShowPassword;

    TogglePasswordVisibility();
  }
  #endregion

  #region TogglePasswordVisibility
  /// <summary>
  /// _passwordBox와 _textBox의 Visibility를 설정하는 코드를 중복을 피하도록 리팩토링합니다. 
  /// 상태 변경에 대한 로직을 메서드로 추출
  /// </summary>
  private void TogglePasswordVisibility()
  {
    if (_passwordBox == null || _textBox == null) return;

    if (ShowPassword)
    {
      _textBox.Text = _passwordBox.Password;
      _textBox.Visibility = Visibility.Visible;
      _passwordBox.Visibility = Visibility.Collapsed;
    }
    else
    {
      _passwordBox.Password = _textBox.Text;
      _passwordBox.Visibility = Visibility.Visible;
      _textBox.Visibility = Visibility.Collapsed;
    }

    UpdateHasPassword();
  }
  #endregion

  #region UpdateHasPassword 메서드
  /// <summary>
  /// HasPassword 속성의 값은 Password 속성의 값과 동기화
  /// </summary>
  private void UpdateHasPassword()
  {
    HasPassword = !string.IsNullOrEmpty(Password);
  }
  #endregion

  #region ShowPassword
  /// <summary>
  /// 눈 아이콘을 누르면 비밀번호 보기 상태 여부 (true일 경우 일반 텍스트로 표시)
  /// </summary>
  public bool ShowPassword
  {
    get => (bool)GetValue(ShowPasswordProperty);
    set => SetValue(ShowPasswordProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty ShowPasswordProperty =
      DependencyProperty.Register(nameof(ShowPassword), typeof(bool), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(false, OnShowPasswordChanged));
  #endregion

  #region OnShowPasswordChanged 메서드
  private static void OnShowPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var control = (HeimdallrPlaceholderPasswordBox)d;
    control.TogglePasswordVisibility();
  }
  #endregion

  #region OnPasswordChanged 콜백 메서드 ViewModel에서 Password 속성 변경 시 내부 PasswordBox/TextBox 동기화
  // ViewModel → Password 속성 변경 시 내부 PasswordBox/TextBox 값도 업데이트
  private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var control = (HeimdallrPlaceholderPasswordBox)d;
    var newPassword = e.NewValue as string ?? string.Empty;

    if (control._passwordBox != null && control._passwordBox.Password != newPassword)
    {
      control._passwordBox.Password = newPassword;
    }

    if (control._textBox != null && control._textBox.Text != newPassword)
    {
      control._textBox.Text = newPassword;
    }

    control.UpdateHasPassword();
  }
  #endregion

  #region OnApplyTemplate 재정의
  /// <summary>
  /// 템플릿 적용 시 내부 PasswordBox, TextBox 참조 및 이벤트 연결
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // 이전 연결된 이벤트 해제
    if (_passwordBox != null)
      _passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

    if (_textBox != null)
      _textBox.TextChanged -= TextBox_TextChanged;

    // 템플릿에서 PasswordBox 와 TextBox 찾아서 컨트롤 가져오기
    _passwordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;
    _textBox = GetTemplateChild("PART_TextBox") as TextBox;

    // 초기 값 설정 및 이벤트 연결
    if (_passwordBox != null)
    {
      _passwordBox.Password = Password;
      _passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
    }

    if (_textBox != null)
    {
      _textBox.Text = Password;
      _textBox.TextChanged += TextBox_TextChanged;
    }

    TogglePasswordVisibility();
  }
  #endregion

  #region PasswordBox_PasswordChanged 이벤트
  private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
  {
    if (_passwordBox == null) return;

    if (Password != _passwordBox.Password)
      Password = _passwordBox.Password; // ViewModel 속성과 동기화
  }
  #endregion

  #region TextBox_TextChanged 이벤트
  private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (_textBox == null) return;

    if (Password != _textBox.Text)
      Password = _textBox.Text; // ViewModel 속성과 동기화
  }
  #endregion

  #region CornerRadius 
  /// <summary>
  /// 코너라디우스
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }

  /// <summary>
  /// 기본값 0
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
     DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
         typeof(HeimdallrPlaceholderPasswordBox),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion

  #region PlaceholderText 입력란에 표시될 워터마크 문자열
  public string PlaceholderText
  {
    get => (string)GetValue(PlaceholderTextProperty);
    set => SetValue(PlaceholderTextProperty, value);
  }
  public static readonly DependencyProperty PlaceholderTextProperty =
      DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(string.Empty));
  #endregion

  #region PlaceholderForeground 워터마크 텍스트 색상
  public Brush PlaceholderForeground
  {
    get => (Brush)GetValue(PlaceholderForegroundProperty);
    set => SetValue(PlaceholderForegroundProperty, value);
  }
  public static readonly DependencyProperty PlaceholderForegroundProperty =
      DependencyProperty.Register(nameof(PlaceholderForeground), typeof(Brush), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(Brushes.Gray));
  #endregion

  #region HasPassword
  /// <summary>
  /// 비밀번호 입력 여부 확인
  /// true → 입력됨, false → 비어 있음
  /// 워터마크 표시 Trigger 용
  /// </summary>
  public bool HasPassword
  {
    get => (bool)GetValue(HasPasswordProperty);
    private set => SetValue(HasPasswordPropertyKey, value);  // 읽기 전용
  }

  // 읽기 전용 의존 속성 정의
  private static readonly DependencyPropertyKey HasPasswordPropertyKey =
      DependencyProperty.RegisterReadOnly(nameof(HasPassword), typeof(bool), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(false));
  /// <summary>
  /// 현재 입력된 비밀번호가 있는지 여부 (워터마크 표시 Trigger 등에 사용)
  /// 읽기 전용 DependencyProperty 이므로 외부에서 직접 Set할 수 없음.
  /// 내부에서는 HasPasswordPropertyKey를 통해만 변경 가능.
  /// </summary>
  public static readonly DependencyProperty HasPasswordProperty = HasPasswordPropertyKey.DependencyProperty;
  #endregion

  #region Icon
  /// <summary>
  /// HeimdallrIcon에 표시될 PathGeometry 아이콘 타입
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(IconType.None));
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
          typeof(HeimdallrPlaceholderPasswordBox), new PropertyMetadata(25.0));
  #endregion

  #region IconFill
  /// <summary>
  /// 아이콘 색상 (HeimdallrIcon의 Fill과 바인딩됨)
  /// </summary>
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrPlaceholderPasswordBox),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA5D7E8"))));
  #endregion

  #region IconMouseOverFill
  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }
  public static readonly DependencyProperty IconMouseOverFillProperty =
      DependencyProperty.Register(nameof(IconMouseOverFill), typeof(Brush), typeof(HeimdallrPlaceholderPasswordBox), new PropertyMetadata(Brushes.DeepSkyBlue));
  #endregion

  #region Password
  /// <summary>
  /// MVVM 바인딩용 Password 속성
  /// 실제 PasswordBox.Password와 동기화됨
  /// </summary>
  public string Password
  {
    get => (string)GetValue(PasswordProperty);
    set => SetValue(PasswordProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty PasswordProperty =
      DependencyProperty.Register(nameof(Password), typeof(string), typeof(HeimdallrPlaceholderPasswordBox),
          new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));  // 값 변경 시 이벤트 호출
  #endregion

  #region CaretBrush
  /// <summary>
  /// 커서 색상
  /// </summary>
  public Brush CaretBrush
  {
    get => (Brush)GetValue(CaretBrushProperty);
    set => SetValue(CaretBrushProperty, value);
  }

  /// <summary>
  /// 기본값: 검은색
  /// </summary>
  public static readonly DependencyProperty CaretBrushProperty =
      DependencyProperty.Register(nameof(CaretBrush), typeof(Brush), typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"))));
  #endregion

  #region MouseOverBorderBrush
  /// <summary>
  /// 마우스오버시 색상지정
  /// </summary>
  public Brush MouseOverBorderBrush
  {
    get => (Brush)GetValue(MouseOverBorderBrushProperty);
    set => SetValue(MouseOverBorderBrushProperty, value);
  }

  /// <summary>
  /// 마우스오버시 색상지정 속성
  /// </summary>
  public static readonly DependencyProperty MouseOverBorderBrushProperty =
      DependencyProperty.Register(nameof(MouseOverBorderBrush), typeof(Brush),
          typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF58F84"))));
  #endregion

  #region FocusedBorderBrush
  /// <summary>
  /// 포커스시 보더브러시 지정
  /// </summary>
  public Brush FocusedBorderBrush
  {
    get => (Brush)GetValue(FocusedBorderBrushProperty);
    set => SetValue(FocusedBorderBrushProperty, value);
  }

  /// <summary>
  /// 포커스시 보더브러시 지정 속성
  /// </summary>
  public static readonly DependencyProperty FocusedBorderBrushProperty =
      DependencyProperty.Register(nameof(FocusedBorderBrush), typeof(Brush),
          typeof(HeimdallrPlaceholderPasswordBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF58F84"))));
  #endregion

  #region EyeIcon
  public IconType EyeIcon
  {
    get => (IconType)GetValue(EyeIconProperty);
    set => SetValue(EyeIconProperty, value);
  }

  public static readonly DependencyProperty EyeIconProperty = DependencyProperty.Register(nameof(EyeIcon), typeof(IconType),
          typeof(HeimdallrPlaceholderPasswordBox), new PropertyMetadata(IconType.None));
  #endregion

  #region EyeIconFill
  public Brush EyeIconFill
  {
    get => (Brush)GetValue(EyeIconFillProperty);
    set => SetValue(EyeIconFillProperty, value);
  }

  public static readonly DependencyProperty EyeIconFillProperty = DependencyProperty.Register(nameof(EyeIconFill), typeof(Brush),
          typeof(HeimdallrPlaceholderPasswordBox), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE6ED"))));
  #endregion

  #region RightIconToolTip
  public string RightIconToolTip
  {
    get => (string)GetValue(RightIconToolTipProperty);
    set => SetValue(RightIconToolTipProperty, value);
  }

  public static readonly DependencyProperty RightIconToolTipProperty =
      DependencyProperty.Register(nameof(RightIconToolTip), typeof(string), typeof(HeimdallrPlaceholderPasswordBox), new PropertyMetadata(string.Empty));
  #endregion
}


