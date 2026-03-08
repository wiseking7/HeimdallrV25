using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// Heimdallr 스타일의 Placeholder 텍스트 입력 컨트롤 (TextBox 기반 커스텀 컨트롤)
/// 좌측 아이콘 + Placeholder 지원 기능 포함
/// </summary>
public class HeimdallrPlaceholderTextBox : TextBox
{
  #region Constructor
  /// <summary>
  /// 기본 스타일 키 등록 (Generic.xaml에서 템플릿 정의 필요)
  /// </summary>
  static HeimdallrPlaceholderTextBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrPlaceholderTextBox),
        new FrameworkPropertyMetadata(typeof(HeimdallrPlaceholderTextBox)));
  }

  public HeimdallrPlaceholderTextBox()
  {
    ToolTipOpening += HeimdallrPlaceholderTextBox_ToolTipOpening;
  }
  #endregion

  #region HeimdallrPlaceholderTextBox_ToolTipOpening 이벤트
  private void HeimdallrPlaceholderTextBox_ToolTipOpening(object sender, ToolTipEventArgs e)
  {
    // ToolTip 자체가 없으면 아예 열리지 않게
    if (ToolTip == null)
    {
      e.Handled = true;
      return;
    }

    // 이미 HeimdallrToolTip이면 그대로 사용
    if (ToolTip is HeimdallrToolTip)
      return;

    // 문자열일 경우만 변환
    if (ToolTip is string tooltipText && !string.IsNullOrWhiteSpace(tooltipText))
    {
      ToolTip = new HeimdallrToolTip
      {
        Content = tooltipText
      };
    }
    else
    {
      // 빈 문자열 / 알 수 없는 타입 → 표시 안 함
      e.Handled = true;
    }
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
         typeof(HeimdallrPlaceholderTextBox),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion

  #region PlaceholderText
  /// <summary>
  /// 입력 전 표시할 안내 텍스트 (Placeholder)
  /// </summary>
  public string PlaceholderText
  {
    get => (string)GetValue(PlaceholderTextProperty);
    set => SetValue(PlaceholderTextProperty, value);
  }

  /// <summary>
  /// 기본값 없음
  /// </summary>
  public static readonly DependencyProperty PlaceholderTextProperty =
      DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(string.Empty));
  #endregion

  #region PlaceholderForeground 
  /// <summary>
  /// Placeholder 색상지정
  /// </summary>
  public Brush PlaceholderForeground
  {
    get => (Brush)GetValue(PlaceholderForegroundProperty);
    set => SetValue(PlaceholderForegroundProperty, value);
  }

  /// <summary>
  /// 기본값 #AAAAAA
  /// </summary>
  public static readonly DependencyProperty PlaceholderForegroundProperty =
      DependencyProperty.Register(nameof(PlaceholderForeground), typeof(Brush), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"))));
  #endregion

  #region HasText, HasTextProperty
  /// <summary>
  /// 현재 입력 텍스트가 있는지 여부
  /// </summary>
  public bool HasText
  {
    get => (bool)GetValue(HasTextProperty);
    private set => SetValue(HasTextPropertyKey, value);
  }

  /// <summary>
  /// 현재 입력된 텍스트가 있는지 여부를 나타내는 읽기 전용 속성 으로 수정하면 좋습니다.
  /// </summary>
  private static readonly DependencyPropertyKey HasTextPropertyKey =
      DependencyProperty.RegisterReadOnly(nameof(HasText), typeof(bool), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(false));

  /// <summary>
  /// HasTextPropertyKey.DependencyProperty
  /// </summary>
  public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;
  #endregion

  #region Icon 
  /// <summary>
  /// 아이콘 지정
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }

  /// <summary>
  /// 아이콘 속성
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(IconType.None));
  #endregion

  #region IconFill
  /// <summary>
  /// 아이콘 색상지정
  /// </summary>
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }

  /// <summary>
  /// 아이콘 색상지정 속성
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"))));
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
          typeof(HeimdallrPlaceholderTextBox), new PropertyMetadata(30.0));
  #endregion

  #region IconMouseOverFill
  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }
  public static readonly DependencyProperty IconMouseOverFillProperty =
      DependencyProperty.Register(nameof(IconMouseOverFill), typeof(Brush), typeof(HeimdallrPlaceholderTextBox), new PropertyMetadata(Brushes.DeepSkyBlue));
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
          typeof(HeimdallrPlaceholderTextBox),
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
          typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF58F84"))));
  #endregion

  #region IsFloatingLabel
  /// <summary>
  /// Floating Label 사용 여부
  /// - false : 일반 Placeholder (기본값)
  /// - true  : Floating Label 동작
  /// </summary>
  public bool IsFloatingLabel
  {
    get => (bool)GetValue(IsFloatingLabelProperty);
    set => SetValue(IsFloatingLabelProperty, value);
  }

  public static readonly DependencyProperty IsFloatingLabelProperty =
      DependencyProperty.Register(
          nameof(IsFloatingLabel),
          typeof(bool),
          typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(false));
  #endregion

  #region RightIcon
  public IconType RightIcon
  {
    get => (IconType)GetValue(RightIconProperty);
    set => SetValue(RightIconProperty, value);
  }

  public static readonly DependencyProperty RightIconProperty =
      DependencyProperty.Register(nameof(RightIcon), typeof(IconType),
          typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(IconType.None));
  #endregion

  #region RightIconFill
  /// <summary>
  /// 아이콘 색상지정
  /// </summary>
  public Brush RightIconFill
  {
    get => (Brush)GetValue(RightIconFillProperty);
    set => SetValue(RightIconFillProperty, value);
  }

  /// <summary>
  /// 아이콘 색상지정 속성
  /// </summary>
  public static readonly DependencyProperty RightIconFillProperty =
      DependencyProperty.Register(nameof(RightIconFill), typeof(Brush), typeof(HeimdallrPlaceholderTextBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCBE4DE"))));
  #endregion

  #region RightCommand
  public ICommand? RightCommand
  {
    get => (ICommand?)GetValue(RightCommandProperty);
    set => SetValue(RightCommandProperty, value);
  }

  public static readonly DependencyProperty RightCommandProperty =
      DependencyProperty.Register(nameof(RightCommand), typeof(ICommand),
          typeof(HeimdallrPlaceholderTextBox));
  #endregion

  #region RightIconSize
  /// <summary>
  /// 이이콘 사이즈 너비,높이
  /// </summary>
  public double RightIconSize
  {
    get => (double)GetValue(RightIconSizeProperty);
    set => SetValue(RightIconSizeProperty, value);
  }

  /// <summary>
  /// 아이콘사이즈 기본값
  /// </summary>
  public static readonly DependencyProperty RightIconSizeProperty =
      DependencyProperty.Register(nameof(RightIconSize), typeof(double),
          typeof(HeimdallrPlaceholderTextBox), new PropertyMetadata(24.0));
  #endregion

  #region RightIconToolTip
  public string RightIconToolTip
  {
    get => (string)GetValue(RightIconToolTipProperty);
    set => SetValue(RightIconToolTipProperty, value);
  }

  public static readonly DependencyProperty RightIconToolTipProperty = DependencyProperty.Register(nameof(RightIconToolTip), typeof(string), typeof(HeimdallrPlaceholderTextBox),
    new PropertyMetadata(string.Empty));
  #endregion
}

/* Converter 사용으로 변경
 public HeimdallrPlaceholderTextBox()
  {
    ToolTipOpening += HeimdallrPlaceholderTextBox_ToolTipOpening;

    DataObject.AddPastingHandler(this, OnPaste);
  } 

 #region TextConverter
  public IValueConverter? TextConverter
  {
    get => (IValueConverter?)GetValue(TextConverterProperty);
    set => SetValue(TextConverterProperty, value);
  }

  public static readonly DependencyProperty TextConverterProperty = DependencyProperty.Register(nameof(TextConverter), typeof(IValueConverter), typeof(HeimdallrPlaceholderTextBox),
    new PropertyMetadata(null));
  #endregion

  #region InputFormat Type
  public InputFormatType InputFormat
  {
    get => (InputFormatType)GetValue(InputFormatProperty);
    set => SetValue(InputFormatProperty, value);
  }

  public static readonly DependencyProperty InputFormatProperty = DependencyProperty.Register(nameof(InputFormat), typeof(InputFormatType), typeof(HeimdallrPlaceholderTextBox),
    new PropertyMetadata(InputFormatType.None, OnInputFormatChanged));
  #endregion

  #region OnInputFormatChanged 메서드
  private static void OnInputFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrPlaceholderTextBox textBox)
    {
      textBox.ApplyInputFormat();
      // InputFormat이 변경될 때마다 필요한 초기화 작업 수행
      // 예: 포맷터 재설정, 텍스트 갱신 등
      // 현재는 특별한 작업이 필요하지 않으므로 빈 메서드로 유지
    }
  }
  #endregion

  #region ApplyInputFormat 메서드
  private void ApplyInputFormat()
  {
    TextConverter = InputFormat switch
    {
      InputFormatType.Numeric => new NumericConverter(),
      InputFormatType.NumericWithThousands => new NumericWithThousandsConverter(),
      InputFormatType.NumericWithDecimal => new NumericWithDecimalConverter(),
      InputFormatType.MobilePhone => new MobilePhoneConverter(),
      InputFormatType.PhoneLandLine => new PhoneLandLineConverter(),
      _ => null,
    };

    if (!string.IsNullOrEmpty(Text))
    {
      // 기존 텍스트를 새 Converter에 맞게 즉시 변환
      Text = TextConverter?.ConvertBack(Text, typeof(string), null, CultureInfo.CurrentCulture)?.ToString() ?? Text;
    }
  }
  #endregion

  #region OnTextChanged - 실시간 Converter 적용
  protected override void OnTextChanged(TextChangedEventArgs e)
  {
    base.OnTextChanged(e);

    HasText = !string.IsNullOrEmpty(Text);

    // InputFormat이 None이면 포맷 로직을 건너뛰고 그대로 처리
    if (InputFormat == InputFormatType.None || TextConverter == null)
      return;

    string oldText = Text;
    int oldCaret = CaretIndex;

    // 포맷 적용 (화면용)
    string newText = TextConverter.Convert(Text, typeof(string), null, CultureInfo.CurrentCulture)?.ToString() ?? Text;

    if (newText != oldText)
    {
      int newCaret = oldCaret;

      // 하이픈, 콤마 등 포맷 문자 변화 시 Caret 위치 보정
      int formatCharDiff = newText.Take(newCaret).Count(c => !char.IsDigit(c))
                           - oldText.Take(oldCaret).Count(c => !char.IsDigit(c));

      newCaret += formatCharDiff;

      Dispatcher.BeginInvoke(new Action(() =>
      {
        Text = newText;
        CaretIndex = Math.Max(0, Math.Min(newCaret, Text.Length));
      }), System.Windows.Threading.DispatcherPriority.Normal);
    }
  }
  #endregion

  #region LostFocus - 최종 포맷 적용
  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);

    if (TextConverter == null) return;

    if (string.IsNullOrWhiteSpace(Text)) return;

    // NumericWithDecimal이면 최종 포맷 적용 (천단위 + 소수점 2자리)
    if (InputFormat == InputFormatType.NumericWithDecimal)
    {
      // ViewModel에는 항상 숫자만 저장됨
      if (decimal.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
      {
        // 천 단위 콤마 + 소수점 2자리
        Text = number.ToString("#,0.00", CultureInfo.CurrentCulture);
      }
    }
    else
    {
      // 다른 포맷이 있으면 Convert 사용
      Text = TextConverter.Convert(Text, typeof(string), null, CultureInfo.CurrentCulture)?.ToString() ?? Text;
    }
  }
  #endregion

  #region PreviewTextInput - MaxLength 제한
  protected override void OnPreviewTextInput(TextCompositionEventArgs e)
  {
    base.OnPreviewTextInput(e);
  }
  #endregion

  #region Paste 처리
  private void OnPaste(object sender, DataObjectPastingEventArgs e)
  {
    if (!e.DataObject.GetDataPresent(typeof(string)))
    {
      e.CancelCommand();
      return;
    }

    string pasted = (string)e.DataObject.GetData(typeof(string))!;

    if (TextConverter != null)
    {
      pasted = TextConverter.ConvertBack(pasted, typeof(string), null, CultureInfo.CurrentCulture)?.ToString()!;
    }

    // MaxLength 체크 제거, Converter에서 처리
    Text = pasted;
    CaretIndex = Text.Length;

    e.CancelCommand();
  }
  #endregion

 */