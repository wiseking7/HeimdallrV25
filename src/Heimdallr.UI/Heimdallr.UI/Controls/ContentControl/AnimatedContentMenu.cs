using Heimdallr.UI.Animations;
using Heimdallr.UI.Enums;
using Heimdallr.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Heimdallr.UI.Controls;

/// <summary>
/// AnimatedContentMenu는 좌/우/상/하 방향으로 슬라이딩 가능한 메뉴 컨트롤입니다.
/// Overlay, Push 모드를 지원하며, 내부 콘텐츠의 DesiredSize를 기반으로 애니메이션 진행.
/// Easing 기능과 Open/Close 이벤트를 제공합니다.
/// </summary>
public class AnimatedContentMenu : ContentControl
{
  #region Dependency Properties

  #region IsOpen
  /// <summary>
  /// 메뉴 열림 여부
  /// true면 메뉴가 열려있고, false면 닫혀있음
  /// 변경 시 ToggleMenu()를 호출
  /// </summary>
  public bool IsOpen
  {
    get => (bool)GetValue(IsOpenProperty);
    set => SetValue(IsOpenProperty, value);
  }
  public static readonly DependencyProperty IsOpenProperty =
      DependencyProperty.Register(
          nameof(IsOpen),
          typeof(bool),
          typeof(AnimatedContentMenu),
          new PropertyMetadata(false, OnIsOpenPropertyChanged));
  #endregion

  #region OpenCloseDuration
  /// <summary>
  /// 메뉴 열기/닫기 애니메이션 지속 시간 기본값 : 0.8 초
  /// Duration 구조체로 지정 가능
  /// 기본값: 800ms
  /// </summary>
  public Duration OpenCloseDuration
  {
    get => (Duration)GetValue(OpenCloseDurationProperty);
    set => SetValue(OpenCloseDurationProperty, value);
  }
  public static readonly DependencyProperty OpenCloseDurationProperty =
      DependencyProperty.Register(
          nameof(OpenCloseDuration),
          typeof(Duration),
          typeof(AnimatedContentMenu),
          new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(800))));
  #endregion

  #region SlideDirection
  /// <summary>
  /// 메뉴 슬라이드 방향 (Left, Right, Top, Bottom)
  /// </summary>
  public SlideDirection SlideDirection
  {
    get => (SlideDirection)GetValue(SlideDirectionProperty);
    set => SetValue(SlideDirectionProperty, value);
  }
  public static readonly DependencyProperty SlideDirectionProperty =
      DependencyProperty.Register(
          nameof(SlideDirection),
          typeof(SlideDirection),
          typeof(AnimatedContentMenu),
          new PropertyMetadata(SlideDirection.Left));
  #endregion

  #region OverlayMode
  /// <summary>
  /// Overlay 동작 모드
  /// Overlay: 메뉴 위에 덮어씀
  /// Push: 메뉴 열릴 때 다른 컨텐츠를 밀어냄
  /// </summary>
  public OverlayMode OverlayMode
  {
    get => (OverlayMode)GetValue(OverlayModeProperty);
    set => SetValue(OverlayModeProperty, value);
  }
  public static readonly DependencyProperty OverlayModeProperty =
      DependencyProperty.Register(
          nameof(OverlayMode),
          typeof(OverlayMode),
          typeof(AnimatedContentMenu),
          new PropertyMetadata(OverlayMode.Overlay));
  #endregion

  #region EasingMode -> 애니메이션을 열릴때 모드 지정
  /// <summary>
  /// 애니메이션 Easing 모드
  /// Open/Close 애니메이션 곡선 정의
  /// 기본값: CubicEaseOut
  /// </summary>
  public EasingFunctionBaseMode EasingMode
  {
    get => (EasingFunctionBaseMode)GetValue(EasingModeProperty);
    set => SetValue(EasingModeProperty, value);
  }
  public static readonly DependencyProperty EasingModeProperty =
      DependencyProperty.Register(
          nameof(EasingMode),
          typeof(EasingFunctionBaseMode),
          typeof(AnimatedContentMenu),
          new PropertyMetadata(EasingFunctionBaseMode.CubicEaseOut));
  #endregion
  #endregion

  #region Template Fields
  private Border? _rootBorder;           // PART_Root: 메뉴 전체 컨테이너, TranslateTransform 적용 대상
  private ContentPresenter? _presenter;  // PART_ContentPresenter: 내부 콘텐츠 실제 표시
  #endregion

  #region Constructor / Template
  static AnimatedContentMenu()
  {
    // Generic.xaml에서 기본 스타일 참조
    DefaultStyleKeyProperty.OverrideMetadata(
        typeof(AnimatedContentMenu),
        new FrameworkPropertyMetadata(typeof(AnimatedContentMenu)));
  }
  #endregion

  #region 재정의 OnApplyTemplate
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // PART_Root와 PART_ContentPresenter 가져오기
    _rootBorder = GetTemplateChild("PART_Root") as Border;
    _presenter = GetTemplateChild("PART_ContentPresenter") as ContentPresenter;

    // OverlayMode 및 초기 메뉴 상태 적용
    UpdateLayoutForOverlayMode();
    ApplyInitialState();
  }
  #endregion

  #region Animation Logic
  /// <summary>
  /// IsOpen 속성 변경 시 호출
  /// </summary>
  private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is AnimatedContentMenu menu)
      menu.ToggleMenu(); // 메뉴 열기/닫기 처리
  }

  /// <summary>
  /// 메뉴 열기/닫기 토글
  /// </summary>
  private void ToggleMenu()
  {
    if (_rootBorder == null || _presenter == null) return;

    // 1. 내부 콘텐츠 DesiredSize 측정
    _presenter.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    double menuWidth = _presenter.DesiredSize.Width;
    double menuHeight = _presenter.DesiredSize.Height;

    // 2. TranslateTransform 준비 (없으면 새로 생성)
    if (!(_rootBorder.RenderTransform is TranslateTransform))
      _rootBorder.RenderTransform = new TranslateTransform();
    var transform = (TranslateTransform)_rootBorder.RenderTransform;

    // 3. 방향별 애니메이션 From/To 값과 Transform Property 가져오기
    var (from, to, property) = GetAnimationValues(menuWidth, menuHeight);

    // 4. 애니메이션 시작
    transform.BeginAnimation(property, CreateAnimation(from, to));

    // 5. 이벤트 호출
    if (IsOpen) OnOpened();
    else OnClosed();
  }

  /// <summary>
  /// 방향별 애니메이션 From/To 값과 Transform Property 반환
  /// </summary>
  private (double from, double to, DependencyProperty property) GetAnimationValues(double menuWidth, double menuHeight)
  {
    double from = 0, to = 0;
    DependencyProperty property;
    var transform = (TranslateTransform)_rootBorder!.RenderTransform!;

    switch (SlideDirection)
    {
      case SlideDirection.Left:
        _rootBorder.HorizontalAlignment = HorizontalAlignment.Left;
        _rootBorder.Width = menuWidth;
        property = TranslateTransform.XProperty;
        if (IsOpen) { transform.X = -menuWidth; from = -menuWidth; to = 0; }
        else { from = 0; to = -menuWidth; }
        break;

      case SlideDirection.Right:
        _rootBorder.HorizontalAlignment = HorizontalAlignment.Right;
        _rootBorder.Width = menuWidth;
        property = TranslateTransform.XProperty;
        if (IsOpen) { transform.X = menuWidth; from = menuWidth; to = 0; }
        else { from = 0; to = menuWidth; }
        break;

      case SlideDirection.Top:
        _rootBorder.VerticalAlignment = VerticalAlignment.Top;
        _rootBorder.Height = menuHeight;
        property = TranslateTransform.YProperty;
        if (IsOpen) { transform.Y = -menuHeight; from = -menuHeight; to = 0; }
        else { from = 0; to = -menuHeight; }
        break;

      case SlideDirection.Bottom:
        _rootBorder.VerticalAlignment = VerticalAlignment.Bottom;
        _rootBorder.Height = menuHeight;
        property = TranslateTransform.YProperty;
        if (IsOpen) { transform.Y = menuHeight; from = menuHeight; to = 0; }
        else { from = 0; to = menuHeight; }
        break;

      default:
        throw new NotSupportedException($"SlideDirection {SlideDirection}은 지원되지 않습니다.");
    }

    return (from, to, property);
  }

  /// <summary>
  /// TranslateTransform용 DoubleAnimation 생성
  /// </summary>
  private DoubleAnimation CreateAnimation(double from, double to)
  {
    return new DoubleAnimation
    {
      From = from,
      To = to,
      Duration = OpenCloseDuration,
      EasingFunction = EasingMode.GetEasingFunction()
    };
  }

  #endregion

  #region Overlay / Layout Logic

  /// <summary>
  /// OverlayMode에 따른 초기 ZIndex 및 정렬 적용
  /// Overlay 모드면 ZIndex를 높게 설정하여 다른 콘텐츠 위에 표시
  /// Push 모드면 HorizontalAlignment를 SlideDirection 기준으로 설정
  /// </summary>
  private void UpdateLayoutForOverlayMode()
  {
    if (_rootBorder == null) return;

    Panel.SetZIndex(this, OverlayMode == OverlayMode.Overlay ? 99 : 1);

    if (OverlayMode == OverlayMode.Push)
    {
      HorizontalAlignment = SlideDirection switch
      {
        SlideDirection.Left => HorizontalAlignment.Left,
        SlideDirection.Right => HorizontalAlignment.Right,
        _ => HorizontalAlignment.Stretch
      };
    }
  }

  /// <summary>
  /// 초기 상태 적용 (닫힌 상태)
  /// </summary>
  private void ApplyInitialState()
  {
    if (!IsOpen)
      CloseInstant();
  }

  /// <summary>
  /// 즉시 닫기 (Width/Height = 0)
  /// 애니메이션 없이 초기 숨김 상태 적용
  /// </summary>
  private void CloseInstant()
  {
    if (_rootBorder == null) return;

    if (SlideDirection is SlideDirection.Left or SlideDirection.Right)
      _rootBorder.Width = 0;
    else
      _rootBorder.Height = 0;
  }

  #endregion

  #region Events

  /// <summary>메뉴가 열렸을 때 발생</summary>
  public event EventHandler? Opened;

  /// <summary>메뉴가 닫혔을 때 발생</summary>
  public event EventHandler? Closed;

  protected virtual void OnOpened() => Opened?.Invoke(this, EventArgs.Empty);
  protected virtual void OnClosed() => Closed?.Invoke(this, EventArgs.Empty);

  #endregion
}








