using Heimdallr.UI.Enums;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Heimdallr.UI.Controls;

/// <summary>
/// Heimdallr 스타일의 Hold 버튼입니다.
/// 마우스를 일정 시간 누르면 Hold 이벤트가 발생하며, 진행 상태를 시각적으로 표시합니다.
/// </summary>
public class HeimdallrHoldButton : Button
{
  // Hold 동작 취소를 위한 CancellationTokenSource
  private CancellationTokenSource? _cts;

  // 템플릿 내부에서 참조되는 요소
  private Border? _bgRect;   // 배경 확대용
  private Border? _holdRect; // Hold 진행 상태 표시용

  #region HoldDuratin
  /// <summary>
  /// Hold 지속시간
  /// </summary>
  public Duration HoldDuration
  {
    get => (Duration)GetValue(HoldDurationProperty);
    set => SetValue(HoldDurationProperty, value);
  }
  /// <summary>
  /// 기본값 0.5
  /// </summary>
  public static readonly DependencyProperty HoldDurationProperty =
      DependencyProperty.Register(nameof(HoldDuration), typeof(Duration), typeof(HeimdallrHoldButton),
          new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.5))));
  #endregion

  #region CornerRadius
  /// <summary>
  /// 버튼 모서리 둥글기
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
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeimdallrHoldButton),
          new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region Icon
  /// <summary>
  /// 아이콘 타입 (사용자 정의 Icon)
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 기본값 없음
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrHoldButton),
          new PropertyMetadata(IconType.None));
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
          typeof(HeimdallrHoldButton), new PropertyMetadata(24.0));
  #endregion

  #region IconFill
  /// <summary>
  /// 아이콘 색상
  /// </summary>
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }
  /// <summary>
  /// 기본값 그레이
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.Gray));
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
  public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(nameof(ButtonText), typeof(string),
        typeof(HeimdallrHoldButton), new PropertyMetadata(string.Empty));
  #endregion

  #region BackgroundRectangleFill
  /// <summary>
  /// 배경 확대용 색상
  /// </summary>
  public Brush BackgroundRectangleFill
  {
    get => (Brush)GetValue(BackgroundRectangleFillProperty);
    set => SetValue(BackgroundRectangleFillProperty, value);
  }
  /// <summary>
  /// 기본값 검정
  /// </summary>
  public static readonly DependencyProperty BackgroundRectangleFillProperty =
      DependencyProperty.Register(nameof(BackgroundRectangleFill), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF456882")))); // 기본값
  #endregion

  #region OneHodlFill
  /// <summary>
  /// Hold 진행 상태 첫 번째 색상
  /// </summary>
  public Brush OneHoldFill
  {
    get => (Brush)GetValue(OneHoldFillProperty);
    set => SetValue(OneHoldFillProperty, value);
  }
  /// <summary>
  /// 기본값 오렌지
  /// </summary>
  public static readonly DependencyProperty OneHoldFillProperty =
      DependencyProperty.Register(nameof(OneHoldFill), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.Orange));
  #endregion

  #region TwoHoldFill
  /// <summary>
  /// Hold 진행 상태 두 번째 색상
  /// </summary>
  public Brush TwoHoldFill
  {
    get => (Brush)GetValue(TwoHoldFillProperty);
    set => SetValue(TwoHoldFillProperty, value);
  }
  /// <summary>
  /// 기본값 노란
  /// </summary>
  public static readonly DependencyProperty TwoHoldFillProperty =
      DependencyProperty.Register(nameof(TwoHoldFill), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.Yellow));
  #endregion

  #region ThreeHoldFill
  /// <summary>
  /// Hold 진행 상태 세 번째 색상
  /// </summary>
  public Brush ThreeHoldFill
  {
    get => (Brush)GetValue(ThreeHoldFillProperty);
    set => SetValue(ThreeHoldFillProperty, value);
  }
  /// <summary>
  /// 기본값 그린
  /// </summary>
  public static readonly DependencyProperty ThreeHoldFillProperty =
      DependencyProperty.Register(nameof(ThreeHoldFill), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.Green));
  #endregion

  #region MouseOver 배경색
  /// <summary>
  /// 마우스 오버 시 배경
  /// </summary>
  public Brush MouseOverBackground
  {
    get => (Brush)GetValue(MouseOverBackgroundProperty);
    set => SetValue(MouseOverBackgroundProperty, value);
  }
  /// <summary>
  /// 기본값 그레이
  /// </summary>
  public static readonly DependencyProperty MouseOverBackgroundProperty =
      DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.LightGray));
  #endregion

  #region Press(클릭상태) 배경 색상
  /// <summary>
  /// 클릭 상태 배경
  /// </summary>
  public Brush PressedBackground
  {
    get => (Brush)GetValue(PressedBackgroundProperty);
    set => SetValue(PressedBackgroundProperty, value);
  }
  /// <summary>
  /// 기본값 다크그레이
  /// </summary>
  public static readonly DependencyProperty PressedBackgroundProperty =
      DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(Brushes.DarkGray));
  #endregion

  #region 키보드 포커스 시 테두리 색상
  /// <summary>
  /// 키보드 포커스 시 테두리 색상
  /// </summary>
  public Brush KeyboardFocusBorderBrush
  {
    get => (Brush)GetValue(KeyboardFocusBorderBrushProperty);
    set => SetValue(KeyboardFocusBorderBrushProperty, value);
  }
  /// <summary>
  /// 기본값 블루
  /// </summary>
  public static readonly DependencyProperty KeyboardFocusBorderBrushProperty =
      DependencyProperty.Register(nameof(KeyboardFocusBorderBrush), typeof(Brush), typeof(HeimdallrHoldButton),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0C134F")))); // 기본값
  #endregion

  #region 키보드 포터스시 테두리 두께
  /// <summary>
  /// 키보드 포커스 시 테두리 두께
  /// </summary>
  public Thickness KeyboardFocusBorderThickness
  {
    get => (Thickness)GetValue(KeyboardFocusBorderThicknessProperty);
    set => SetValue(KeyboardFocusBorderThicknessProperty, value);
  }
  /// <summary>
  /// 기본값 2
  /// </summary>
  public static readonly DependencyProperty KeyboardFocusBorderThicknessProperty =
      DependencyProperty.Register(nameof(KeyboardFocusBorderThickness), typeof(Thickness), typeof(HeimdallrHoldButton),
          new PropertyMetadata(new Thickness(2)));
  #endregion

  #region RoutedEvents Hold 완료
  /// <summary>
  /// Hold 완료 이벤트
  /// </summary>
  public static readonly RoutedEvent HoldCompletedEvent =
      EventManager.RegisterRoutedEvent(nameof(HoldCompleted), RoutingStrategy.Bubble,
          typeof(RoutedEventHandler), typeof(HeimdallrHoldButton));

  /// <summary>
  /// Hold 진행 이벤트
  /// </summary>
  public event RoutedEventHandler HoldCompleted
  {
    add => AddHandler(HoldCompletedEvent, value);
    remove => RemoveHandler(HoldCompletedEvent, value);
  }
  #endregion

  #region RoutedEvents Hold 취소
  /// <summary>
  /// Hold 취소 이벤트
  /// </summary>
  public static readonly RoutedEvent HoldCancelledEvent =
      EventManager.RegisterRoutedEvent(nameof(HoldCancelled), RoutingStrategy.Bubble,
          typeof(RoutedEventHandler), typeof(HeimdallrHoldButton));

  /// <summary>
  /// 홀드 취소
  /// </summary>
  public event RoutedEventHandler HoldCancelled
  {
    add => AddHandler(HoldCancelledEvent, value);
    remove => RemoveHandler(HoldCancelledEvent, value);
  }
  #endregion

  #region 재정의 OnMouseLeftButtonDown Handling
  /// <summary>
  /// 마우스 왼쪽 버튼 눌림 처리
  /// Hold 애니메이션 시작, HoldDuration 후 Click 이벤트 발생
  /// </summary>
  protected override async void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);

    _cts?.Cancel();
    _cts?.Dispose();
    _cts = new CancellationTokenSource();

    StartHoldAnimation();

    try
    {
      await Task.Delay(HoldDuration.TimeSpan, _cts.Token);

      base.OnClick(); // Click 이벤트 트리거
      RaiseEvent(new RoutedEventArgs(HoldCompletedEvent));

      ResetAnimation();
    }
    catch (TaskCanceledException)
    {
      RaiseEvent(new RoutedEventArgs(HoldCancelledEvent));
      ResetAnimation();
    }
  }
  #endregion

  #region 재정의 OnMouseLeftButtonUp 마우스 왼쪽 버튼 때면 Hold 취소
  /// <summary>
  /// 마우스 왼쪽 버튼 떼기 시 Hold 취소
  /// </summary>
  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    CancelHold();
    base.OnMouseLeftButtonUp(e);
  }
  #endregion

  #region 마우스가 벗어나면 Hold 취소 메서드
  /// <summary>
  /// 마우스가 버튼을 벗어나면 Hold 취소
  /// </summary>
  protected override void OnMouseLeave(MouseEventArgs e)
  {
    CancelHold();
    base.OnMouseLeave(e);
  }
  #endregion

  #region Hold 취소 메서드
  private void CancelHold()
  {
    if (_cts != null && !_cts.IsCancellationRequested)
      _cts.Cancel();
  }
  #endregion

  #region Hold 애니메이션 시작
  /// <summary>
  /// Hold 애니메이션 시작
  /// _holdRect 색상 애니메이션, _bgRect 확대 애니메이션 수행
  /// Frozen 브러시 문제 해결 위해 Clone 사용
  /// </summary>
  private void StartHoldAnimation()
  {
    // 디자인타임에서는 애니메이션 건너뜀
    if (DesignerProperties.GetIsInDesignMode(this))
      return;

    if (_holdRect == null || _bgRect == null) return;

    double width = ActualWidth;
    double height = ActualHeight;

    // 크기가 아직 측정되지 않았다면 일단 나감 (Loaded 이벤트에서 다시 실행됨)
    if (width == 0 || height == 0)
    {
      Dispatcher.BeginInvoke(new Action(StartHoldAnimation), System.Windows.Threading.DispatcherPriority.Loaded);
      return;
    }

    // HoldRect 초기화
    _holdRect.Width = 0;
    _holdRect.Height = 0;

    // Frozen 문제 해결: Background Brush Clone
    if (_holdRect.Background is SolidColorBrush originalHoldBrush)
    {
      var brush = originalHoldBrush.Clone();
      brush.Color = Colors.Transparent; // 초기 투명
      _holdRect.Background = brush;
    }

    if (_bgRect.Background is SolidColorBrush originalBgBrush)
      _bgRect.Background = originalBgBrush.Clone();

    var holdBrush = _holdRect.Background as SolidColorBrush;

    // 색상 단계별 애니메이션 (빨강 20%, 노랑 25%, 초록 나머지)
    var colorAnimation = new ColorAnimationUsingKeyFrames
    {
      Duration = HoldDuration.TimeSpan,
      KeyFrames =
      {
        new LinearColorKeyFrame((OneHoldFill as SolidColorBrush)?.Color ?? Colors.Red, KeyTime.FromPercent(0.0)),
        new LinearColorKeyFrame((OneHoldFill as SolidColorBrush)?.Color ?? Colors.Red, KeyTime.FromPercent(0.2)), // 빨강 20%
        new LinearColorKeyFrame((TwoHoldFill as SolidColorBrush)?.Color ?? Colors.Yellow, KeyTime.FromPercent(0.45)), // 노랑 25%
        new LinearColorKeyFrame((ThreeHoldFill as SolidColorBrush)?.Color ?? Colors.Green, KeyTime.FromPercent(1.0)) // 나머지 초록
      }
    };
    holdBrush?.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

    // 배경 확대 애니메이션
    _bgRect.BeginAnimation(WidthProperty, new DoubleAnimation(0, width, HoldDuration.TimeSpan));
    _bgRect.BeginAnimation(HeightProperty, new DoubleAnimation(0, height, HoldDuration.TimeSpan));

    // HoldRect 위치 흔들림 애니메이션
    if (_holdRect.RenderTransform is TranslateTransform transform)
    {
      var moveDuration = new Duration(HoldDuration.TimeSpan);

      var animX = new DoubleAnimationUsingKeyFrames
      {
        Duration = moveDuration,
        KeyFrames =
        {
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)),
          new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.25)),
          new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.5)),
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.75)),
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1))
        }
      };

      var animY = new DoubleAnimationUsingKeyFrames
      {
        Duration = moveDuration,
        KeyFrames =
        {
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)),
          new LinearDoubleKeyFrame(-5, KeyTime.FromPercent(0.25)),
          new LinearDoubleKeyFrame(5, KeyTime.FromPercent(0.5)),
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.75)),
          new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1))
        }
      };

      transform.BeginAnimation(TranslateTransform.XProperty, animX);
      transform.BeginAnimation(TranslateTransform.YProperty, animY);
    }

    // HoldRect 크기 애니메이션
    _holdRect.BeginAnimation(WidthProperty, new DoubleAnimation(0, width, HoldDuration.TimeSpan));
    _holdRect.BeginAnimation(HeightProperty, new DoubleAnimation(0, height, HoldDuration.TimeSpan));
  }
  #endregion

  #region 애니메이션 초기화 메서드
  /// <summary>
  /// 애니메이션 초기화
  /// </summary>
  private void ResetAnimation()
  {
    if (_holdRect == null || _bgRect == null) return;

    _bgRect.BeginAnimation(WidthProperty, null);
    _bgRect.BeginAnimation(HeightProperty, null);
    _bgRect.Width = 0;
    _bgRect.Height = 0;

    _holdRect.BeginAnimation(WidthProperty, null);
    _holdRect.BeginAnimation(HeightProperty, null);

    if (_holdRect.Background is SolidColorBrush brush)
      brush.BeginAnimation(SolidColorBrush.ColorProperty, null);

    _holdRect.Width = 0;
    _holdRect.Height = 0;

    if (_holdRect.RenderTransform is TranslateTransform transform)
    {
      transform.X = 0;
      transform.Y = 0;
    }
  }
  #endregion

  #region Override OnApplyTemplate(템플시 요소 가져오기)
  /// <summary>
  /// 템플릿 요소 참조 가져오기
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    _bgRect = GetTemplateChild("BgRect") as Border;
    _holdRect = GetTemplateChild("HoldRect") as Border;

    if (_holdRect != null && _holdRect.RenderTransform == null)
      _holdRect.RenderTransform = new TranslateTransform();
  }
  #endregion

  #region AnimatedFillDuration
  public TimeSpan AnimatedFillDuration
  {
    get => (TimeSpan)GetValue(AnimatedFillDurationProperty);
    set => SetValue(AnimatedFillDurationProperty, value);
  }

  public static readonly DependencyProperty AnimatedFillDurationProperty =
    DependencyProperty.Register(
        nameof(AnimatedFillDuration),
        typeof(TimeSpan),
        typeof(HeimdallrHoldButton),
        new PropertyMetadata(TimeSpan.FromSeconds(0.3))); // 기본 0.3초
  #endregion

  #region 정적생성자, 생성자
  // 기본 스타일 키 설정
  static HeimdallrHoldButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrHoldButton),
        new FrameworkPropertyMetadata(typeof(HeimdallrHoldButton)));
  }
  /// 생성자
  public HeimdallrHoldButton()
  {
    // 컨트롤 로드 완료시 크기가 0인 경우를 대비해 한번만 등록 (실행시 바로 홀드표기)
    // Loaded += HeimdallrHoldIconButton_Loaded;

    // 마우스 클릭시만 StartHoldAnimation 호출

    ToolTipOpening += HeimdallrHoldButton_ToolTipOpening;
  }
  #endregion

  #region HeimdallrHoldButton_ToolTipOpening 이벤트
  private void HeimdallrHoldButton_ToolTipOpening(object sender, ToolTipEventArgs e)
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
}
