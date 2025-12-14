using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrPulseEllipseButton : Button
{
  #region 첫 번째 원 색상지정
  /// <summary>
  /// 첫 번째 원의 색상을 설정하는 속성
  /// </summary>
  public Brush Ellipse1Brush
  {
    get => (Brush)GetValue(Ellipse1BrushProperty);
    set => SetValue(Ellipse1BrushProperty, value);
  }
  /// <summary>
  /// 첫 번째 원의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty Ellipse1BrushProperty = DependencyProperty.Register(nameof(Ellipse1Brush),
    typeof(Brush), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(Brushes.LightBlue));
  #endregion

  #region 두 번째 원 색상지정
  /// <summary>
  /// 두 번째 원의 색상을 설정하는 속성
  /// </summary>
  public Brush Ellipse2Brush
  {
    get => (Brush)GetValue(Ellipse2BrushProperty);
    set => SetValue(Ellipse2BrushProperty, value);
  }
  /// <summary>
  /// 두 번째 원의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty Ellipse2BrushProperty =
      DependencyProperty.Register(nameof(Ellipse2Brush), typeof(Brush), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(Brushes.LightGreen));
  #endregion

  #region 세 번째 원 색상지정
  /// <summary>
  /// 세 번째 원의 색상을 설정하는 속성
  /// </summary>
  public Brush Ellipse3Brush
  {
    get => (Brush)GetValue(Ellipse3BrushProperty);
    set => SetValue(Ellipse3BrushProperty, value);
  }
  /// <summary>
  /// 세 번째 원의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty Ellipse3BrushProperty =
      DependencyProperty.Register(nameof(Ellipse3Brush), typeof(Brush), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(Brushes.LightCoral));
  #endregion

  #region HeimdallrIcon 색상지정
  /// <summary>
  /// HeimdallrIcon 의 색상을 설정하는 속성
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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(Brushes.DarkGray));
  #endregion

  #region Icon 지정
  /// <summary>
  /// Heimdallr 아이콘의 경로를 지정하는 속성
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// Heimdallr 아이콘의 경로를 지정하는 속성
  /// </summary>
  public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(IconType.None));

  #endregion

  #region Icon Size 지정
  /// <summary>
  /// Heimdallr 아이콘의 사이즈 지정
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }
  /// <summary>
  /// 기본값 사이즈 24
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(24.0));

  #endregion

  #region Shape Mode (Free / Circle)
  public AspectMode ShapeMode
  {
    get => (AspectMode)GetValue(ShapeModeProperty);
    set => SetValue(ShapeModeProperty, value);
  }
  public static readonly DependencyProperty ShapeModeProperty = DependencyProperty.Register(nameof(ShapeMode),
          typeof(AspectMode), typeof(HeimdallrPulseEllipseButton), new PropertyMetadata(AspectMode.Free));
  #endregion

  static HeimdallrPulseEllipseButton()
  {
    // DefaultStyleKey를 재정의하여 XAML에서 스타일을 사용할 수 있게 설정
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrPulseEllipseButton),
        new FrameworkPropertyMetadata(typeof(HeimdallrPulseEllipseButton)));
  }
}
