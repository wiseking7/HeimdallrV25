using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// HeimdallrIconToggleButton 스타일 정의 (팝업창이나 ContentMenu 외부 UI 열기)
/// 콤보박스 토글 버튼, 팝업창, ContentMenu 등에 사용
/// </summary>
public class HeimdallrIconToggleButton : ToggleButton
{
  #region 종속성 생성자
  static HeimdallrIconToggleButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrIconToggleButton),
      new FrameworkPropertyMetadata(typeof(HeimdallrIconToggleButton)));
  }
  #endregion

  #region 마우스 오버  배경색 지정
  /// <summary>
  /// 마우스 오버 시 배경색을 설정하는 종속성 속성입니다.
  /// </summary>
  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }

  /// <summary>
  /// 마우스 오버 시 배경색을 설정하는 종속성 속성입니다.
  /// </summary>
  public static readonly DependencyProperty IconMouseOverFillProperty =
        DependencyProperty.Register(
            nameof(IconMouseOverFill),
            typeof(Brush),
            typeof(HeimdallrIconToggleButton),
            new PropertyMetadata(Brushes.Transparent));
  #endregion

  #region CornerRadius
  /// <summary>
  /// 그리드 항목의 모서리 반경을 설정하는 종속성 속성입니다.
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }
  /// <summary>
  /// 그리드 항목의 모서리 반경을 설정하는 종속성 속성입니다.
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
    DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeimdallrIconToggleButton),
      new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region Icon, IconSize, IconFill, IconCheckedFill, IconDisabledFill
  /// <summary>
  /// 타이틀 아이콘으로 표시될 PathIcon 타입 지정 (예: Barcode, Setting 등)
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrIconToggleButton),
        new PropertyMetadata(IconType.None));

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
          typeof(HeimdallrIconToggleButton), new PropertyMetadata(24.0));
  #endregion

  /// <summary>
  /// PathIcon (주 아이콘) 의 색상 지정용 Brush
  /// 기본값은 회색(Gray)
  /// </summary>
  public Brush IconFill
  {
    get { return (Brush)GetValue(IconFillProperty); }
    set { SetValue(IconFillProperty, value); }
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrIconToggleButton),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"))));

  /// <summary>
  /// IsChecked=True일 때 배경에 적용할 색상
  /// </summary>
  public Brush IconCheckedFill
  {
    get => (Brush)GetValue(IconCheckedFillProperty);
    set => SetValue(IconCheckedFillProperty, value);
  }
  /// <summary>
  /// 종속성 주입
  /// </summary>
  public static readonly DependencyProperty IconCheckedFillProperty =
      DependencyProperty.Register(nameof(IconCheckedFill), typeof(Brush), typeof(HeimdallrIconToggleButton),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008B8B")))); // 기본값

  /// <summary>
  /// IsEditable="False" 편집불가시 색상변경
  /// </summary>
  public Brush IconDisabledFill
  {
    get => (Brush)GetValue(IconDisabledFillProperty);
    set => SetValue(IconDisabledFillProperty, value);
  }

  /// <summary>
  /// 기본값 그레이
  /// </summary>
  public static readonly DependencyProperty IconDisabledFillProperty =
      DependencyProperty.Register(nameof(IconDisabledFill), typeof(Brush), typeof(HeimdallrIconToggleButton),
          new PropertyMetadata(Brushes.Gray));
}
