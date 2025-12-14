using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrMinimizeButton : Button
{

  static HeimdallrMinimizeButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMinimizeButton), new FrameworkPropertyMetadata(typeof(HeimdallrMinimizeButton)));
  }

  #region Fill
  /// <summary>
  /// Path 아이콘에 색상을 적용할 때 사용되는 속성 (브러시)
  /// </summary>
  public Brush Fill
  {
    get => (Brush)GetValue(FillProperty);
    set => SetValue(FillProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush),
        typeof(HeimdallrMinimizeButton), new PropertyMetadata(Brushes.Gray));
  #endregion

  #region IconSize
  /// <summary>
  /// Icon, Image 사이즈 너비,높이
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  /// <summary>
  /// 아이콘사이즈 기본값
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double),
          typeof(HeimdallrMinimizeButton), new PropertyMetadata(25.0));
  #endregion

  #region Stretch
  // <summary>
  /// Viewbox 또는 Path 렌더링에 사용할 Stretch 모드
  /// </summary>
  public Stretch Stretch
  {
    get => (Stretch)GetValue(StretchProperty);
    set => SetValue(StretchProperty, value);
  }

  /// <summary>
  /// 기본값 Uniform으로 설정된 Stretch 속성
  /// </summary>
  public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch),
    typeof(Stretch), typeof(HeimdallrMinimizeButton), new PropertyMetadata(Stretch.Uniform));
  #endregion
}