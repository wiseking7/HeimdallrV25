using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Heimdallr.UI.Controls;

/// <summary>
/// 진행률 표시용 커스텀 프로그레스바 컨트롤
/// 최소값/최대값 지원, 인디터미넌트 모드, 진행률 텍스트 표시, 색상 커스터마이징 포함
/// </summary>
public class HeimdallrProgressBar : ProgressBar
{
  #region 생성자
  static HeimdallrProgressBar()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrProgressBar),
        new FrameworkPropertyMetadata(typeof(HeimdallrProgressBar)));

    // ValueProperty 변경 감지
    ValueProperty.OverrideMetadata(typeof(HeimdallrProgressBar),
        new FrameworkPropertyMetadata(0.0, OnValueChanged));
  }

  // Value 이 변경될때 마다 UpdateVisuals()가 호출되어 시각적 요소(진행률 Width, 애니메이션 등)를 갱신할 수 있습니다.
  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrProgressBar bar)
    {
      bar.UpdateVisuals();
    }
  }

  public HeimdallrProgressBar()
  {
    ToolTipOpening += HeimdallrProgressBar_ToolTipOpening;
  }
  #endregion

  #region HeimdallrProgressBar_ToolTipOpening 이벤트
  private void HeimdallrProgressBar_ToolTipOpening(object sender, ToolTipEventArgs e)
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

  #region 사용자 선택 옵션 프로퍼티
  /// <summary> 프로그레스바 진행률 색상 </summary>
  public Brush Fill
  {
    get => (Brush)GetValue(FillProperty);
    set => SetValue(FillProperty, value);
  }
  /// <summary>
  /// 기본값 DeepSkyBlue로 설정된 진행률 채우기 색상 속성입니다.
  /// </summary>
  public static readonly DependencyProperty FillProperty =
      DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(HeimdallrProgressBar),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6EACDA"))));

  /// <summary> IsProgressTextVisible (진행률 텍스트 표시 여부) (기본 true) </summary>
  public bool IsProgressTextVisible
  {
    get => (bool)GetValue(IsProgressTextVisibleProperty);
    set => SetValue(IsProgressTextVisibleProperty, value);
  }
  /// <summary>
  /// 기본값 true로 설정된 진행률 텍스트 표시 여부 속성입니다.
  /// </summary>
  public static readonly DependencyProperty IsProgressTextVisibleProperty =
      DependencyProperty.Register(nameof(IsProgressTextVisible), typeof(bool), typeof(HeimdallrProgressBar),
          new PropertyMetadata(true));


  // ProgressTextForeground (진행 텍스트 색상 속성)
  public Brush ProgressTextForeground
  {
    get => (Brush)GetValue(ProgressTextForegroundProperty);
    set => SetValue(ProgressTextForegroundProperty, value);
  }
  public static readonly DependencyProperty ProgressTextForegroundProperty =
      DependencyProperty.Register(nameof(ProgressTextForeground), typeof(Brush), typeof(HeimdallrProgressBar),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))));
  #endregion

  #region Fileds
  private Border? _progressBar;                        // 진행률을 표시하는 Border
  private TranslateTransform? _translateTransform;     // Indeterminate 애니메이션용
  #endregion

  #region OnApplyTemplate 재정의 메서드
  /// <summary>
  /// 템플릿 적용 시 호출됨. 애니메이션 초기화 등을 여기서 처리.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    _progressBar = GetTemplateChild("PART_ProgressBar") as Border;
    _translateTransform = GetTemplateChild("ProgressTranslateTransform") as TranslateTransform;

    if (_progressBar == null)
    {
      Debug.WriteLine($"[{nameof(HeimdallrProgressBar)}.OnApplyTemplate] PART_ProgressBar 템플릿 요소를 찾을 수 없습니다.");
    }
    if (_translateTransform == null)
    {
      Debug.WriteLine($"[{nameof(HeimdallrProgressBar)}.OnApplyTemplate] ProgressTranslateTransform 템플릿 요소를 찾을 수 없습니다.");
    }

    UpdateVisuals();
  }
  #endregion

  #region UpdateVisuals 메서드
  /// <summary>
  /// 진행률 및 상태에 따라 시각 요소(너비, 애니메이션 등) 갱신
  /// Determinate 모드에서는 Width 애니메이션 적용
  /// Indeterminate 모드에서는 기존 좌우 TranslateTransform 애니메이션 유지
  /// </summary>
  private void UpdateVisuals()
  {
    if (_progressBar == null)
      return;

    if (IsIndeterminate)
    {
      // Indeterminate 상태
      VisualStateManager.GoToState(this, "Indeterminate", true);

      // Width는 컨트롤 전체로 유지
      _progressBar.Width = ActualWidth;

      // 색상 애니메이션 예시 (원하면)
      if (Fill is SolidColorBrush originalBrush)
      {
        var animatedBrush = originalBrush.Clone(); // Clone 해야 봉인 문제 방지
        _progressBar.Background = animatedBrush;

        var colorAnimation = new ColorAnimation
        {
          To = ((SolidColorBrush)FindResource("ProgressBarIndeterminateBrush")).Color,
          Duration = TimeSpan.FromSeconds(0.5),
          AutoReverse = true,
          RepeatBehavior = RepeatBehavior.Forever
        };
        animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
      }
    }
    else
    {
      // Determinate 상태
      VisualStateManager.GoToState(this, "Determinate", true);

      // 목표 Width 계산
      double targetWidth = Maximum > 0 ? (Value / Maximum) * ActualWidth : 0;

      // Width가 Auto(NaN)일 경우 0으로 초기화
      if (double.IsNaN(_progressBar.Width))
        _progressBar.Width = 0;

      // 부드러운 Width 애니메이션 적용
      var widthAnimation = new DoubleAnimation
      {
        To = targetWidth,
        Duration = TimeSpan.FromSeconds(0.3),
        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
        FillBehavior = FillBehavior.HoldEnd
      };
      _progressBar.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);

      // Determinate에서는 Fill 색상은 그대로 유지
      _progressBar.Background = Fill;
    }
  }
  #endregion
}

