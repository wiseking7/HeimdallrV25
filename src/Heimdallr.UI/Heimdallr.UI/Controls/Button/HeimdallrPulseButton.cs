using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// 클릭 시 사각형 애니메이션이 적용되는 버튼
/// </summary>
public class HeimdallrPulseButton : Button
{
  #region CornerRadius
  /// <summary>
  /// 버튼의 모서리 둥글기를 설정하는 속성
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }
  /// <summary>
  /// 버튼의 모서리 둥글기를 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
        typeof(HeimdallrPulseButton), new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region Icon
  /// <summary>
  /// PathGeometry 리소스를 사용하여 아이콘을 표시하는 속성
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// XAML에서 직접 등록된 PathGeometry 리소스를 사용하는 방식
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType),
        typeof(HeimdallrPulseButton), new PropertyMetadata(IconType.None));
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
          typeof(HeimdallrPulseButton), new PropertyMetadata(25.0));
  #endregion

  #region IconFill
  /// <summary>
  /// 아이콘의 색상을 설정하는 속성
  /// </summary>
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }
  /// <summary>
  /// 아이콘의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrPulseButton),
        new PropertyMetadata(Brushes.DarkGray));
  #endregion

  #region Text (버튼 우측에 표시할 문자열)
  /// <summary>
  /// 버튼 우측에 표시할 문자열을 설정하는 속성
  /// </summary>
  public string ButtonText
  {
    get => (string)GetValue(ButtonTextProperty);
    set => SetValue(ButtonTextProperty, value);
  }
  /// <summary>
  /// 버튼 우측에 표시할 문자열을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty ButtonTextProperty =
      DependencyProperty.Register(nameof(ButtonText), typeof(string),
        typeof(HeimdallrPulseButton), new PropertyMetadata(string.Empty));
  #endregion

  #region MouseOverBackground
  /// <summary>
  /// 마우스오버시 백그라운드 색상 지정 
  /// </summary>
  public Brush MouseOverBackground
  {
    get => (Brush)GetValue(MouseOverBackgroundProperty);
    set => SetValue(MouseOverBackgroundProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty MouseOverBackgroundProperty =
      DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x39, 0x3E, 0x46))));
  #endregion

  #region MouseOverBorderBrush
  /// <summary>
  /// 마우스 오버시 보더브러쉬 색상지정
  /// </summary>
  public Brush MouseOverBorderBrush
  {
    get => (Brush)GetValue(MouseOverBorderBrushProperty);
    set => SetValue(MouseOverBorderBrushProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty MouseOverBorderBrushProperty =
      DependencyProperty.Register(nameof(MouseOverBorderBrush), typeof(Brush),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88))));
  #endregion

  #region PressedBackground
  /// <summary>
  /// 버튼클릭시 백그라운드 색상지정
  /// </summary>
  public Brush PressedBackground
  {
    get => (Brush)GetValue(PressedBackgroundProperty);
    set => SetValue(PressedBackgroundProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty PressedBackgroundProperty =
      DependencyProperty.Register(nameof(PressedBackground), typeof(Brush),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xAD, 0x49, 0xE1))));
  #endregion

  #region PressedBorderBrush
  /// <summary>
  /// 버튼클릭시 보더브러쉬 색상 지정
  /// </summary>
  public Brush PressedBorderBrush
  {
    get => (Brush)GetValue(PressedBorderBrushProperty);
    set => SetValue(PressedBorderBrushProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty PressedBorderBrushProperty =
      DependencyProperty.Register(nameof(PressedBorderBrush), typeof(Brush),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66))));
  #endregion

  #region FocusedBorderBrush
  /// <summary>
  /// 포커스시 보더브러쉬 색상 지정
  /// </summary>
  public Brush FocusedBorderBrush
  {
    get => (Brush)GetValue(FocusedBorderBrushProperty);
    set => SetValue(FocusedBorderBrushProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty FocusedBorderBrushProperty =
      DependencyProperty.Register(nameof(FocusedBorderBrush), typeof(Brush),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x6E, 0xAC, 0xDA))));
  #endregion


  /// <summary>
  /// 포커스 보더 띠크니스 
  /// </summary>
  #region FocusedBorderThickness
  public Thickness FocusedBorderThickness
  {
    get => (Thickness)GetValue(FocusedBorderThicknessProperty);
    set => SetValue(FocusedBorderThicknessProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty FocusedBorderThicknessProperty =
      DependencyProperty.Register(nameof(FocusedBorderThickness), typeof(Thickness),
          typeof(HeimdallrPulseButton), new PropertyMetadata(new Thickness(2)));
  #endregion

  #region 생성자
  static HeimdallrPulseButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrPulseButton),
        new FrameworkPropertyMetadata(typeof(HeimdallrPulseButton)));
  }
  #endregion
}
