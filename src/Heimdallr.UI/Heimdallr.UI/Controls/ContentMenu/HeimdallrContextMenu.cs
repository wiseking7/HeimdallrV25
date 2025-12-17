using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// 사용자 정의 ContextMenu로, MVVM 패턴과 테마 커스터마이징을 지원합니다.
/// </summary>
public class HeimdallrContextMenu : ContextMenu
{
  #region 생성자
  static HeimdallrContextMenu()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrContextMenu),
        new FrameworkPropertyMetadata(typeof(HeimdallrContextMenu)));
  }
  #endregion

  #region ContextMenu 열거형 (Normal, Hover, Pressed, Disabled)
  public ContextMenuVisualState VisualState
  {
    get => (ContextMenuVisualState)GetValue(VisualStateProperty);
    set => SetValue(VisualStateProperty, value);
  }

  public static readonly DependencyProperty VisualStateProperty =
    DependencyProperty.Register(
      nameof(VisualState),
      typeof(ContextMenuVisualState),
      typeof(HeimdallrContextMenu),
      new PropertyMetadata(ContextMenuVisualState.Normal));
  #endregion

  #region Constructor
  /// <summary>
  /// 생성자
  /// </summary>
  public HeimdallrContextMenu()
  {
    // ContextMenu가 열릴 때 PlacementTarget의 DataContext를 상속
    Opened += OnOpened;

    // 닫힐 때 DataContext 정리
    Closed += OnClosed;

    // ESC 키로 닫기 지원
    PreviewKeyDown += OnPreviewKeyDown;

    // 기본 위치: 마우스 포인트
    Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
  }
  #endregion

  #region Event Handlers
  private void OnOpened(object sender, RoutedEventArgs e)
  {
    if (PlacementTarget is FrameworkElement target)
    {
      // DataContext 자동 상속
      DataContext = target.DataContext;
    }
  }
  private void OnClosed(object sender, RoutedEventArgs e)
  {
    // ⭐ ContextMenu 닫히면 상태 초기화
    //if (DataContext is ContextMenuViewModel vm)
    //  vm.ContextMenuState = ContextMenuVisualState.Normal;

    // Popup 메모리 유지 방지
    ClearValue(DataContextProperty);
  }
  private void OnPreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
    {
      IsOpen = false;
      e.Handled = true;
    }
  }
  #endregion

  #region BackgroundColor 
  /// <summary>
  /// 배경색 사용자 지정 속성 (테마 연동)
  /// </summary>
  public Brush BackgroundColor
  {
    get => (Brush)GetValue(BackgroundColorProperty);
    set => SetValue(BackgroundColorProperty, value);
  }

  /// <summary>
  /// 종속성 속성 등록
  /// </summary>
  public static readonly DependencyProperty BackgroundColorProperty =
      DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(HeimdallrContextMenu),
         new PropertyMetadata(null));
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
         typeof(HeimdallrContextMenu),
         new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion
}