using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrMinimizeButton : Button
{
  #region 생성자
  static HeimdallrMinimizeButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMinimizeButton), new FrameworkPropertyMetadata(typeof(HeimdallrMinimizeButton)));
  }

  public HeimdallrMinimizeButton()
  {
    ToolTipOpening += HeimdallrMinimizeButton_ToolTipOpening;

  }
  #endregion

  #region HeimdallrMinimizeButton_ToolTipOpening 이벤트
  private void HeimdallrMinimizeButton_ToolTipOpening(object sender, ToolTipEventArgs e)
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
        typeof(HeimdallrMinimizeButton), new PropertyMetadata(Brushes.Gray));
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
          typeof(HeimdallrMinimizeButton), new PropertyMetadata(25.0));
  #endregion

  #region MouseOverBackground
  /// <summary>
  /// 마우스오버시 백그라운드 색상 지정 
  /// </summary>
  public Brush MouseOverBackground
  {
    get => (Brush)GetValue(MouseOverBackgroundProperty);
    set => SetValue(MouseOverBackgroundProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty MouseOverBackgroundProperty =
      DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush),
          typeof(HeimdallrMinimizeButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x39, 0x3E, 0x46))));
  #endregion

  #region PressedBackground
  /// <summary>
  /// 버튼클릭시 백그라운드 색상지정
  /// </summary>
  public Brush PressedBackground
  {
    get => (Brush)GetValue(PressedBackgroundProperty);
    set => SetValue(PressedBackgroundProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty PressedBackgroundProperty =
      DependencyProperty.Register(nameof(PressedBackground), typeof(Brush),
          typeof(HeimdallrMinimizeButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xAD, 0x49, 0xE1))));
  #endregion
}