using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// MenuItem 커스터마이징
/// </summary>
public class HeimdallrMenuItem : MenuItem
{
  static HeimdallrMenuItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMenuItem),
        new FrameworkPropertyMetadata(typeof(HeimdallrMenuItem)));
  }

  /// <summary>
  /// HeimdallrIcon에 표시될 PathGeometry 아이콘 타입
  /// </summary>
  public new IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public new static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrMenuItem),
          new PropertyMetadata(IconType.None));

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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrMenuItem),
        new PropertyMetadata(Brushes.Gray));

  /// <summary>
  /// 아이콘 사이즈크기조정
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
    DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HeimdallrMenuItem), new PropertyMetadata(24.0));

  /// ShortcutKeyText (단축키 텍스트)
  public string ShortcutKeyText
  {
    get => (string)GetValue(ShortcutKeyTextProperty);
    set => SetValue(ShortcutKeyTextProperty, value);
  }

  /// <summary>
  /// 
  /// </summary>
  public static readonly DependencyProperty ShortcutKeyTextProperty =
      DependencyProperty.Register(nameof(ShortcutKeyText), typeof(string), typeof(HeimdallrMenuItem), new PropertyMetadata(string.Empty));

  // CommandParameter 기본값 자동 설정 (PlacementTarget.DataContext 등에서)
  // 이 기능은 ContextMenu 쪽에서 자동 처리하는 경우가 많지만, 필요시 여기에 구현 가능

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
         typeof(HeimdallrMenuItem),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion

  #region 사용자 정의 상태 속성들
  public Brush MenuItemIsMouseOver
  {
    get => (Brush)GetValue(MenuItemIsMouseOverProperty);
    set => SetValue(MenuItemIsMouseOverProperty, value);
  }
  public static readonly DependencyProperty MenuItemIsMouseOverProperty =
      DependencyProperty.Register(nameof(MenuItemIsMouseOver), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.DarkBlue));

  public Brush MenuItemIsMouseOverBrush
  {
    get => (Brush)GetValue(MenuItemIsMouseOverBrushProperty);
    set => SetValue(MenuItemIsMouseOverBrushProperty, value);
  }
  public static readonly DependencyProperty MenuItemIsMouseOverBrushProperty =
      DependencyProperty.Register(nameof(MenuItemIsMouseOverBrush), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.DodgerBlue));

  public Brush MenuItemPressedBrush
  {
    get => (Brush)GetValue(MenuItemPressedBrushProperty);
    set => SetValue(MenuItemPressedBrushProperty, value);
  }
  public static readonly DependencyProperty MenuItemPressedBrushProperty =
      DependencyProperty.Register(nameof(MenuItemPressedBrush), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.DarkBlue));

  public Brush MenuItemCheckedBrush
  {
    get => (Brush)GetValue(MenuItemCheckedBrushProperty);
    set => SetValue(MenuItemCheckedBrushProperty, value);
  }
  public static readonly DependencyProperty MenuItemCheckedBrushProperty =
      DependencyProperty.Register(nameof(MenuItemCheckedBrush), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.LightBlue));

  public Brush MenuItemDisabledForegroundBrush
  {
    get => (Brush)GetValue(MenuItemDisabledForegroundBrushProperty);
    set => SetValue(MenuItemDisabledForegroundBrushProperty, value);
  }
  public static readonly DependencyProperty MenuItemDisabledForegroundBrushProperty =
      DependencyProperty.Register(nameof(MenuItemDisabledForegroundBrush), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.Gray));
  #endregion
}
