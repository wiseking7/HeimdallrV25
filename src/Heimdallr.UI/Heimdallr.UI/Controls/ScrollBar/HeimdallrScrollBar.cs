using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;
/// <summary>
/// HeimdallrScrollBar는 커스터마이징된 WPF ScrollBar 컨트롤입니다.
/// 아이콘 사이즈, 트랙 너비, 썸 너비, 썸 높이 자동계산 등을 지원합니다.
/// </summary>
public class HeimdallrScrollBar : ScrollBar
{
  static HeimdallrScrollBar()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrScrollBar),
      new FrameworkPropertyMetadata(typeof(HeimdallrScrollBar)));
  }

  #region TrackBackground Property
  public Brush TrackBackground
  {
    get => (Brush)GetValue(TrackBackgroundProperty);
    set => SetValue(TrackBackgroundProperty, value);
  }

  public static readonly DependencyProperty TrackBackgroundProperty =
      DependencyProperty.Register(nameof(TrackBackground), typeof(Brush), typeof(HeimdallrScrollBar),
          new PropertyMetadata(Brushes.Transparent));
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
         typeof(HeimdallrScrollBar),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
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
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrScrollBar),
          new PropertyMetadata(IconType.None));

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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrScrollBar),
          new PropertyMetadata(Brushes.Transparent));
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
          typeof(HeimdallrScrollBar), new PropertyMetadata(8.0));
  #endregion

  #region VerticalThumbWidth
  public double VerticalThumbWidth
  {
    get => (double)GetValue(VerticalThumbWidthProperty);
    set => SetValue(VerticalThumbWidthProperty, value);
  }

  public static readonly DependencyProperty VerticalThumbWidthProperty =
      DependencyProperty.Register(nameof(VerticalThumbWidth), typeof(double),
          typeof(HeimdallrScrollBar), new PropertyMetadata(25.0));
  #endregion

  #region HorizontalThumbHeight
  public double HorizontalThumbHeight
  {
    get => (double)GetValue(HorizontalThumbHeightProperty);
    set => SetValue(HorizontalThumbHeightProperty, value);
  }

  public static readonly DependencyProperty HorizontalThumbHeightProperty =
      DependencyProperty.Register(nameof(HorizontalThumbHeight), typeof(double),
          typeof(HeimdallrScrollBar), new PropertyMetadata(15.0));
  #endregion


  /// <summary>
  /// HeimdallrScrollbar 템플릿이 적용될 때 호출됩니다.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // Thumb 가 너무 작아지는 문제 방지용
    if (ViewportSize <= 0)
      ViewportSize = 1; // 기본 가시 영영 크기 설정
  }
}
