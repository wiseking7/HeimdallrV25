using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrMaximizeButton : Button
{
  #region 생성자
  static HeimdallrMaximizeButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMaximizeButton), new FrameworkPropertyMetadata(typeof(HeimdallrMaximizeButton)));
  }

  /// <summary>
  /// 생성자: 특별한 로직 없음 (기본 생성자)
  /// </summary>
  public HeimdallrMaximizeButton()
  {
  }
  #endregion

  #region HeimdallrIcon
  /// <summary>
  /// 템플릿 내부에서 사용하는 아이콘 컨트롤입니다.
  /// 템플릿의 PART_IMG 파트로 연결되며, IsMaximize 상태에 따라 아이콘이 변경됩니다.
  /// </summary>
  private HeimdallrIcon? img;
  #endregion

  #region DependencyProperty - IsMaximize

  /// <summary>
  /// 현재 최대화 상태 여부를 나타냅니다.
  /// true면 Restore 아이콘, false면 Maximize 아이콘이 표시됩니다.
  /// </summary>
  public bool IsMaximize
  {
    get => (bool)GetValue(IsMaximizeProperty);
    set => SetValue(IsMaximizeProperty, value);
  }

  /// <summary>
  /// IsMaximize 속성 등록 - 변경 시 아이콘을 자동 변경하도록 콜백 지정
  /// </summary>
  public static readonly DependencyProperty IsMaximizeProperty =
      DependencyProperty.Register(
        nameof(IsMaximize),              // 속성명
        typeof(bool),                    // 속성 타입
        typeof(HeimdallrMaximizeButton),          // 소유자 타입
        new PropertyMetadata(
          false,                         // 기본값: false (즉, Maximize 상태)
          MaximizePropertyChanged        // 변경 콜백
        ));

  /// <summary>
  /// IsMaximize 값이 변경되었을 때 호출되는 콜백 메서드
  /// 아이콘을 Maximize 또는 Restore로 전환합니다.
  /// </summary>
  private static void MaximizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var btn = (HeimdallrMaximizeButton)d;

    // 템플릿 내부의 아이콘 컨트롤이 존재할 경우 아이콘을 갱신
    if (btn.img != null)
    {
      btn.img.Icon = btn.IsMaximize ? IconType.Restore : IconType.Maximize;
    }
  }
  #endregion

  #region 템플릿 적용 및 아이콘 처리

  /// <summary>
  /// 컨트롤 템플릿이 적용된 후 호출되는 메서드
  /// 템플릿에서 HeimdallrIcon을 찾아서 내부 필드에 보관합니다.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // 컨트롤 템플릿 내의 PART_IMG 요소(HeimdallrIcon 타입)를 찾아 img 필드에 저장
    if (GetTemplateChild("PART_IMG") is HeimdallrIcon maxbtn)
    {
      img = maxbtn;

      // 초기 상태에서도 아이콘을 반영
      img.Icon = IsMaximize ? IconType.Restore : IconType.Maximize;
    }
  }

  #endregion

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
        typeof(HeimdallrMaximizeButton), new PropertyMetadata(Brushes.Gray));
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
          typeof(HeimdallrMaximizeButton), new PropertyMetadata(25.0));
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
    typeof(Stretch), typeof(HeimdallrMaximizeButton), new PropertyMetadata(Stretch.Uniform));
  #endregion
}
