using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// Heimdallr 스타일의 커스텀 ComboBox 컨트롤입니다.
/// </summary>
public class HeimdallrComboBox : ComboBox
{
  #region 종속성 생성자
  static HeimdallrComboBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrComboBox), new FrameworkPropertyMetadata(typeof(HeimdallrComboBox)));
  }
  #endregion

  #region 아이콘 마우스오버 배경색
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
            typeof(HeimdallrComboBox),
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
    DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeimdallrComboBox),
      new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region Icon, IconSize, IconFill, IconCheckedFill
  /// <summary>
  /// 아이콘 속성
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 종속성 속성
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrComboBox),
          new PropertyMetadata(IconType.None));

  /// <summary>
  /// 주 아이콘(PathIcon)의 너비 (픽셀 단위) 지정
  /// 기본값은 16픽셀
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
    DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HeimdallrComboBox),
      new PropertyMetadata(16.0)); // 기본값 16

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
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrComboBox), new PropertyMetadata(Brushes.Gray));

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
      DependencyProperty.Register(nameof(IconCheckedFill), typeof(Brush), typeof(HeimdallrComboBox),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008B8B")))); // 기본값
  #endregion

  #region PlaceHolderText
  /// <summary>
  /// 사용자가 아무것도 입력하지 않았을 때 표시할 Placeholder 텍스트
  /// </summary>
  public string PlaceholderText
  {
    get => (string)GetValue(PlaceholderTextProperty);
    set => SetValue(PlaceholderTextProperty, value);
  }

  /// <summary>
  /// 종속성 속성 주입
  /// </summary>
  public static readonly DependencyProperty PlaceholderTextProperty =
      DependencyProperty.Register(
          nameof(PlaceholderText),
          typeof(string),
          typeof(HeimdallrComboBox),
          new PropertyMetadata(string.Empty));
  #endregion

  #region 드롭다운배경색
  /// <summary>
  /// 드롭다운 영역의 배경색 (Popup 내부)
  /// </summary>
  public Brush DropDownBackground
  {
    get => (Brush)GetValue(DropDownBackgroundProperty);
    set => SetValue(DropDownBackgroundProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty DropDownBackgroundProperty =
      DependencyProperty.Register(nameof(DropDownBackground), typeof(Brush), typeof(HeimdallrComboBox),
          new PropertyMetadata(Brushes.White));
  #endregion

  #region 아이콘 활성 색상
  /// <summary>
  /// IsEnabled = false 일때 아이콘 색상 변경
  /// </summary>
  public Brush IconDisabledFill
  {
    get => (Brush)GetValue(IconDisabledFillProperty);
    set => SetValue(IconDisabledFillProperty, value);
  }
  /// <summary>
  /// 기본색상 그레이
  /// </summary>
  public static readonly DependencyProperty IconDisabledFillProperty =
      DependencyProperty.Register(nameof(IconDisabledFill), typeof(Brush), typeof(HeimdallrComboBox),
          new PropertyMetadata(Brushes.LightGray));
  #endregion

  #region 드롭다운 Item 선택시 글자색 
  /// <summary>
  /// 아이템을 선택시 글자색 지정
  /// </summary>
  public Brush SelectedItemForeground
  {
    get => (Brush)GetValue(SelectedItemForegroundProperty);
    set => SetValue(SelectedItemForegroundProperty, value);
  }
  /// <summary>
  /// 기본색상 그레이
  /// </summary>
  public static readonly DependencyProperty SelectedItemForegroundProperty =
      DependencyProperty.Register(nameof(SelectedItemForeground), typeof(Brush), typeof(HeimdallrComboBox),
          new PropertyMetadata(Brushes.LightGray));
  #endregion

  #region 텍스트 입력 커서 색상
  /// <summary>텍스트 입력 커서 색상</summary>
  public Brush CaretBrush
  {
    get => (Brush)GetValue(CaretBrushProperty);
    set => SetValue(CaretBrushProperty, value);
  }
  /// <summary>
  /// CareBrush
  /// </summary>
  public static readonly DependencyProperty CaretBrushProperty =
      DependencyProperty.Register(nameof(CaretBrush), typeof(Brush),
          typeof(HeimdallrComboBox),
          new PropertyMetadata(Brushes.Black));
  #endregion

  #region 팝업의 테두리 설정
  /// <summary>
  /// 팝업의 BorderThickness 설정
  /// </summary>
  public Thickness PopupBorderThickness
  {
    get => (Thickness)GetValue(PopupBorderThicknessProperty);
    set => SetValue(PopupBorderThicknessProperty, value);
  }

  /// <summary>
  /// 팝업 BorderThickness용 DependencyProperty
  /// </summary>
  public static readonly DependencyProperty PopupBorderThicknessProperty =
      DependencyProperty.Register(
          nameof(PopupBorderThickness),
          typeof(Thickness),
          typeof(HeimdallrComboBox),
          new PropertyMetadata(new Thickness(1)) // 기본값 1
      );
  #endregion

  #region Placeholder 글자색 지정
  /// <summary>
  /// Placeholder 텍스트 색상
  /// </summary>
  public Brush PlaceholderForeground
  {
    get => (Brush)GetValue(PlaceholderForegroundProperty);
    set => SetValue(PlaceholderForegroundProperty, value);
  }

  /// <summary>
  /// Placeholder 텍스트 색상
  /// </summary>
  public static readonly DependencyProperty PlaceholderForegroundProperty =
      DependencyProperty.Register(
          nameof(PlaceholderForeground),
          typeof(Brush),
          typeof(HeimdallrComboBox),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(160, 160, 160)))); // 기본 #A0A0A0

  #endregion
}
