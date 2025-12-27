using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrRadioButton : RadioButton
{
  #region 생성자
  static HeimdallrRadioButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrRadioButton),
        new FrameworkPropertyMetadata(typeof(HeimdallrRadioButton)));
  }

  public HeimdallrRadioButton()
  {
    ToolTipOpening += HeimdallrRadioButton_ToolTipOpening;
  }
  #endregion

  #region HeimdallrRadioButton_ToolTipOpening 이벤트
  private void HeimdallrRadioButton_ToolTipOpening(object sender, ToolTipEventArgs e)
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

  #region CornerRadius
  /// <summary>
  /// 버튼의 모서리 둥글기를 설정하는 속성
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }
  /// <summary>
  /// 버튼의 모서리 둥글기를 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
        typeof(HeimdallrRadioButton), new PropertyMetadata(null));
  #endregion

  #region IconFill
  /// <summary>
  /// 아이콘의 색상을 설정하는 속성
  /// </summary>
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }
  /// <summary>
  /// 아이콘의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush),
        typeof(HeimdallrRadioButton), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"))));
  #endregion

  #region Icon
  /// <summary>
  /// PathGeometry 리소스를 사용하여 아이콘을 표시하는 속성
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// XAML에서 직접 등록된 PathGeometry 리소스를 사용하는 방식
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType),
        typeof(HeimdallrRadioButton), new PropertyMetadata(IconType.None));
  #endregion

  #region RadioButton 선택시 Icon 색상 변경
  /// <summary>
  /// 
  /// </summary>
  public Brush CheckedForeground
  {
    get => (Brush)GetValue(CheckedForegroundProperty);
    set => SetValue(CheckedForegroundProperty, value);
  }
  /// <summary>
  /// 
  /// </summary>
  public static readonly DependencyProperty CheckedForegroundProperty =
      DependencyProperty.Register(nameof(CheckedForeground), typeof(Brush), typeof(HeimdallrRadioButton),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BE3144"))));
  #endregion

  #region RadioButton Mouse Overred
  /// <summary>
  /// 
  /// </summary>
  public Brush MouseOverForeground
  {
    get => (Brush)GetValue(MouseOverForegroundProperty);
    set => SetValue(MouseOverForegroundProperty, value);
  }
  /// <summary>
  /// 
  /// </summary>
  public static readonly DependencyProperty MouseOverForegroundProperty =
      DependencyProperty.Register(nameof(MouseOverForeground), typeof(Brush), typeof(HeimdallrRadioButton),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EB5B00"))));
  #endregion

  #region IconSize
  /// <summary>
  /// 이이콘 사이즈
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
          typeof(HeimdallrRadioButton), new PropertyMetadata(24.0));
  #endregion
}
