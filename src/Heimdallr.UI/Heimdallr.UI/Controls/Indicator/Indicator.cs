using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// Indicator 컨트롤은 로딩 상태, 진행 상태 등의 단순한 시각적 지표를 제공하는 사용자 정의 컨트롤입니다.
/// VisualStateManager를 사용하여 IsActive 상태에 따라 "Active" / "Inactive" 상태로 전환됩니다.
/// IndicatorType 및 Foreground 등 다양한 속성을 통해 스타일과 동작을 제어할 수 있습니다.
/// </summary>
public class Indicator : Control
{
  static Indicator()
  {
    // Indicator 컨트롤이 로드될 때 이 컨트롤의 기본 스타일을 Generic.xaml에서 가져오도록 메타데이터를 오버라이드.
    DefaultStyleKeyProperty.OverrideMetadata(typeof(Indicator),
        new FrameworkPropertyMetadata(typeof(Indicator)));
  }

  #region IsActive 활성 상태
  /// <summary>
  /// Indicator의 활성 상태 (True: Active, False: Inactive)
  /// </summary>
  public bool IsActive
  {
    get => (bool)GetValue(IsActiveProperty);
    set => SetValue(IsActiveProperty, value);
  }
  /// <summary>
  /// IsActive 속성은 Indicator가 활성 상태인지 비활성 상태인지를 나타냅니다.
  /// 활성화 여부에 따라 VisualStateManager를 통해 상태 전환이 일어납니다.
  /// </summary>
  public static readonly DependencyProperty IsActiveProperty =
      DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(Indicator),
          new PropertyMetadata(false, OnIsActiveChanged));

  /// <summary>
  /// IsActive 속성 값이 변경될 때 호출됩니다.
  /// VisualStateManager.GoToState를 통해 "Active" 또는 "Inactive" 상태로 전환합니다.
  /// </summary>
  private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var control = (Indicator)d;
    VisualStateManager.GoToState(control, control.IsActive ? "Active" : "Inactive", true);
  }
  #endregion

  #region
  /// <summary>
  /// Indicator 종류를 가져오거나 설정합니다.
  /// </summary>
  public IndicatorType IndicatorType
  {
    get => (IndicatorType)GetValue(IndicatorTypeProperty);
    set => SetValue(IndicatorTypeProperty, value);
  }
  /// <summary>
  /// IndicatorType 속성은 Indicator의 종류(예: Spinner, Bar 등)를 정의합니다.
  /// Template에서 IndicatorType에 따라 다른 시각적 요소를 제공하도록 바인딩됩니다.
  /// </summary>
  public static readonly DependencyProperty IndicatorTypeProperty =
      DependencyProperty.Register(nameof(IndicatorType), typeof(IndicatorType), typeof(Indicator),
          new PropertyMetadata(IndicatorType.None));
  #endregion

  #region Foreground 로 색상가져오기
  /// <summary>
  /// Indicator 내부 시각 요소(Ellipse, Rectangle 등)에 색상을 제공하는 Foreground 속성.
  /// Foreground는 XAML에서 Brush를 지정할 수 있으며, 부모 컨트롤의 Foreground 상속도 지원됩니다.
  /// </summary>
  public new Brush Foreground
  {
    get => (Brush)GetValue(ForegroundProperty);
    set => SetValue(ForegroundProperty, value);
  }
  /// 아래 참고																																	
  public new static readonly DependencyProperty ForegroundProperty =
    TextElement.ForegroundProperty.AddOwner(typeof(Indicator),
      new FrameworkPropertyMetadata(Brushes.Gray,
        FrameworkPropertyMetadataOptions.AffectsRender));
  #endregion

  /// <summary>
  /// 템플릿이 적용될 때 VisualStateManager 상태를 초기화합니다.
  /// 컨트롤이 로드될 때 IsActive 값에 따라 "Active" 또는 "Inactive" 상태로 전환됩니다.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // OnApplyTemplate 시 초기 상태로 VSM(Visual State Manager) 상태를 설정
    VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);
  }

  #region Indicator 크기 설정
  #endregion
}
