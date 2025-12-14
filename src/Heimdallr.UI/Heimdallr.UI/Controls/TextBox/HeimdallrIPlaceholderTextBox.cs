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

  #region UseFloatingLabel
  /// <summary>
  /// Floating Label 사용 여부
  /// - false : 일반 Placeholder (기본값)
  /// - true  : Floating Label 동작
  /// </summary>
  public bool UseFloatingLabel
  {
    get => (bool)GetValue(UseFloatingLabelProperty);
    set => SetValue(UseFloatingLabelProperty, value);
  }

  public static readonly DependencyProperty UseFloatingLabelProperty =
      DependencyProperty.Register(
          nameof(UseFloatingLabel),
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

  public string RightIconToolTipContent
  {
    get => (string)GetValue(RightIconToolTipContentProperty);
    set => SetValue(RightIconToolTipContentProperty, value);
  }

  public static readonly DependencyProperty RightIconToolTipContentProperty =
      DependencyProperty.Register(nameof(RightIconToolTipContent), typeof(string), typeof(HeimdallrPlaceholderTextBox), new PropertyMetadata(string.Empty));
}
