using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrRepeatButton : RepeatButton
{
  #region IconFill 색상지정
  /// <summary>
  /// HeimdallrIcon 의 색상을 설정하는 속성
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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrRepeatButton), new PropertyMetadata(Brushes.Transparent, OnIconFillChanged));
  #endregion

  #region OnIconFillChanged 메서드
  private static void OnIconFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == DependencyProperty.UnsetValue || e.NewValue == null)
    {
      ((HeimdallrRepeatButton)d).SetCurrentValue(
          IconFillProperty,
          Brushes.Transparent);
    }
  }
  #endregion

  #region MouserOverIconFill
  /// <summary>
  /// HeimdallrIcon 의 색상을 설정하는 속성
  /// </summary>
  public Brush MouserOverIconFill
  {
    get => (Brush)GetValue(MouserOverIconFillProperty);
    set => SetValue(MouserOverIconFillProperty, value);
  }
  /// <summary>
  /// 아이콘의 색상을 설정하는 속성
  /// </summary>
  public static readonly DependencyProperty MouserOverIconFillProperty =
      DependencyProperty.Register(nameof(MouserOverIconFill), typeof(Brush), typeof(HeimdallrRepeatButton), new PropertyMetadata(Brushes.Transparent, OnMouseOverIconFillChanged));
  #endregion

  #region OnMouseOverIconFillChanged 메서드
  private static void OnMouseOverIconFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == DependencyProperty.UnsetValue || e.NewValue == null)
    {
      ((HeimdallrRepeatButton)d).SetCurrentValue(
          MouserOverIconFillProperty,
          Brushes.Transparent);
    }
  }
  #endregion

  #region Icon
  /// <summary>
  /// Heimdallr 아이콘의 경로를 지정하는 속성
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// Heimdallr 아이콘의 경로를 지정하는 속성
  /// </summary>
  public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrRepeatButton), new PropertyMetadata(IconType.None));
  #endregion

  #region IconSize
  /// <summary>
  /// Heimdallr 아이콘의 사이즈 지정
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }
  /// <summary>
  /// 기본값 사이즈 24
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HeimdallrRepeatButton), new PropertyMetadata(24.0));

  #endregion

  #region CornerRadius
  /// <summary>
  /// 버튼의 테두리 둥근 정도를 설정합니다.
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }
  /// <summary>
  /// CornerRadius에 대한 종속성 속성 정의.
  /// 기본값은 (0)입니다.
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeimdallrRepeatButton),
          new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region 종속성 생성자
  static HeimdallrRepeatButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrRepeatButton),
      new FrameworkPropertyMetadata(typeof(HeimdallrRepeatButton)));
  }

  public HeimdallrRepeatButton()
  {
    ToolTipOpening += HeimdallrRepeatButton_ToolTipOpening;
  }
  #endregion

  #region HeimdallrRepeatButton_ToolTipOpening 이벤트
  private void HeimdallrRepeatButton_ToolTipOpening(object sender, ToolTipEventArgs e)
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
