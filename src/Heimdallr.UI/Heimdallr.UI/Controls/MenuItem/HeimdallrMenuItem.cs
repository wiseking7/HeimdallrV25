using Heimdallr.UI.Enums;
using Heimdallr.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// MenuItem 커스터마이징
/// </summary>
public class HeimdallrMenuItem : MenuItem
{
  #region 생성자
  // 종속성 생성자
  static HeimdallrMenuItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMenuItem),
        new FrameworkPropertyMetadata(typeof(HeimdallrMenuItem)));
  }

  // 생성자
  public HeimdallrMenuItem()
  {
    IsEnabledChanged += OnIsEnabledChanged;
  }
  #endregion

  #region Icon
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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrMenuItem),
        new PropertyMetadata(Brushes.Gray));
  #endregion

  #region IconSize
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
  #endregion

  #region SubHeaderText
  /// ShortcutKeyText (단축키 텍스트)
  public string SubHeaderText
  {
    get => (string)GetValue(SubHeaderTextProperty);
    set => SetValue(SubHeaderTextProperty, value);
  }

  /// <summary>
  /// 
  /// </summary>
  public static readonly DependencyProperty SubHeaderTextProperty =
      DependencyProperty.Register(nameof(SubHeaderText), typeof(string), typeof(HeimdallrMenuItem), new PropertyMetadata(string.Empty));
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
         typeof(HeimdallrMenuItem),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion

  #region ContextMenu 상태 전파 로직 
  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    SetContextMenuState(ContextMenuVisualState.Hover);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    SetContextMenuState(ContextMenuVisualState.Normal);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonDown(e);
    SetContextMenuState(ContextMenuVisualState.Pressed);
  }

  protected override void OnSubmenuOpened(RoutedEventArgs e)
  {
    base.OnSubmenuOpened(e);
    SetContextMenuState(ContextMenuVisualState.Hover);
  }

  protected override void OnSubmenuClosed(RoutedEventArgs e)
  {
    base.OnSubmenuClosed(e);
    SetContextMenuState(ContextMenuVisualState.Normal);
  }

  private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (!(bool)e.NewValue)
      SetContextMenuState(ContextMenuVisualState.Disabled);
  }

  private void SetContextMenuState(ContextMenuVisualState state)
  {
    if (ItemsControl.ItemsControlFromItemContainer(this) is HeimdallrContextMenu contextMenu)
    {
      contextMenu.VisualState = state;

      // 배경 Brush 결정
      Brush targetBackground = state switch
      {
        ContextMenuVisualState.Hover => MenuItemIsMouseOver,
        ContextMenuVisualState.Pressed => MenuItemPressedBrush,
        ContextMenuVisualState.Disabled => MenuItemDisabledForegroundBrush,
        _ => Background
      };

      // 전경 Brush 결정
      Brush targetForeground = state switch
      {
        ContextMenuVisualState.Hover => MenuItemIsMouseOverBrush,
        ContextMenuVisualState.Pressed => Foreground,
        ContextMenuVisualState.Disabled => MenuItemDisabledForegroundBrush,
        _ => Foreground
      };

      AnimationExtensions.SetAnimatedBackground(contextMenu, targetBackground);
      AnimationExtensions.SetAnimatedForeground(contextMenu, targetForeground);
    }
  }
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

  public Brush MenuItemDisabledForegroundBrush
  {
    get => (Brush)GetValue(MenuItemDisabledForegroundBrushProperty);
    set => SetValue(MenuItemDisabledForegroundBrushProperty, value);
  }
  public static readonly DependencyProperty MenuItemDisabledForegroundBrushProperty =
      DependencyProperty.Register(nameof(MenuItemDisabledForegroundBrush), typeof(Brush), typeof(HeimdallrMenuItem), new PropertyMetadata(Brushes.Gray));
  #endregion
}
