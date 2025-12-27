using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

  #region Fill 프로퍼티 (진행률 채우기 색상)
  /// <summary>
  /// 진행률 영역을 채울 브러시 색상 (기본: DeepSkyBlue)
  /// </summary>
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
  #endregion

  #region IsProgressTextVisibleProperty 프로퍼티 (진행률 텍스트 표시 여부)
  /// <summary>
  /// 진행률 값을 %로 텍스트로 표시할지 여부 (기본 true)
  /// </summary>
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
  #endregion

  #region 진행 텍스트 색상 속성
  public Brush ProgressTextForeground
  {
    get => (Brush)GetValue(ProgressTextForegroundProperty);
    set => SetValue(ProgressTextForegroundProperty, value);
  }
  public static readonly DependencyProperty ProgressTextForegroundProperty =
      DependencyProperty.Register(nameof(ProgressTextForeground), typeof(Brush), typeof(HeimdallrProgressBar),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))));
  #endregion

  #region OnApplyTemplate 재정의 메서드
  /// <summary>
  /// 템플릿 적용 시 호출됨. 애니메이션 초기화 등을 여기서 처리.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    UpdateVisuals();
  }
  #endregion

  #region UpdateVisuals 메서드
  /// <summary>
  /// 진행률 및 상태에 따라 시각 요소(너비, 애니메이션 등) 갱신
  /// </summary>
  private void UpdateVisuals()
  {
    if (IsIndeterminate)
    {
      VisualStateManager.GoToState(this, "Indeterminate", true);
    }
    else
    {
      VisualStateManager.GoToState(this, "Determinate", true);
    }
  }
  #endregion
}
