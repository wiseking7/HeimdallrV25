using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrGroupBox : GroupBox
{
  #region 생성자
  static HeimdallrGroupBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrGroupBox),
      new FrameworkPropertyMetadata(typeof(HeimdallrGroupBox)));
  }
  #endregion

  #region CornerRadius
  /// <summary> 그룹박스 테두리 둥근 정도를 설정합니다. </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }

  /// <summary> CornerRadius에 대한 종속성 속성 정의. 기본값은 (0)입니다. </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeimdallrGroupBox),
          new PropertyMetadata(new CornerRadius(0)));
  #endregion

  #region HeaderForeground
  public Brush HeaderForeground
  {
    get => (Brush)GetValue(HeaderForegroundProperty);
    set => SetValue(HeaderForegroundProperty, value);
  }

  public static readonly DependencyProperty HeaderForegroundProperty =
      DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(HeimdallrGroupBox),
          new PropertyMetadata(Brushes.Black));
  #endregion

  #region HeaderFontSize / HeaderFontWeight
  public double HeaderFontSize
  {
    get => (double)GetValue(HeaderFontSizeProperty);
    set => SetValue(HeaderFontSizeProperty, value);
  }

  public static readonly DependencyProperty HeaderFontSizeProperty =
      DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(HeimdallrGroupBox),
          new PropertyMetadata(16.0));

  public FontWeight HeaderFontWeight
  {
    get => (FontWeight)GetValue(HeaderFontWeightProperty);
    set => SetValue(HeaderFontWeightProperty, value);
  }

  public static readonly DependencyProperty HeaderFontWeightProperty =
      DependencyProperty.Register(nameof(HeaderFontWeight), typeof(FontWeight), typeof(HeimdallrGroupBox),
          new PropertyMetadata(FontWeights.Bold));
  #endregion

  #region IconType
  /// <summary> HeimdallrIcon에 사용할 아이콘 PathGeometry 유형을 지정합니다. (예: 펼침 화살표, 사용자 아이콘 등) </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }

  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrGroupBox),
          new PropertyMetadata(IconType.None));
  #endregion

  #region IconFill
  /// <summary> HeimdallrIcon의 색상을 지정합니다. 기본색상는 회색(Gray)입니다. </summary>
  public Brush IconFill
  {
    get { return (Brush)GetValue(IconFillProperty); }
    set { SetValue(IconFillProperty, value); }
  }

  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrGroupBox),
        new PropertyMetadata(Brushes.Gray));

  /// <summary> 이이콘 사이즈 너비,높이, 아이콘의 기본크기는 24 입니다. </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register(nameof(IconSize), typeof(double),
          typeof(HeimdallrGroupBox), new PropertyMetadata(24.0));
  #endregion

  #region IsCollapsible
  public bool IsCollapsible
  {
    get => (bool)GetValue(IsCollapsibleProperty);
    set => SetValue(IsCollapsibleProperty, value);
  }

  public static readonly DependencyProperty IsCollapsibleProperty =
      DependencyProperty.Register(nameof(IsCollapsible), typeof(bool), typeof(HeimdallrGroupBox),
          new PropertyMetadata(false));
  #endregion

  #region IsExpanded
  public bool IsExpanded
  {
    get => (bool)GetValue(IsExpandedProperty);
    set => SetValue(IsExpandedProperty, value);
  }

  public static readonly DependencyProperty IsExpandedProperty =
      DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(HeimdallrGroupBox),
          new PropertyMetadata(true)); // 기본 열림 상태
  #endregion

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    var icon = GetTemplateChild("PART_HeaderIcon") as HeimdallrIcon;
    if (icon != null)
    {
      // 이벤트 중복 연결 방지
      icon.MouseLeftButtonUp -= Icon_MouseLeftButtonUp;

      // IsCollapsible가 true일 때만 접힘 기능 활성화
      if (IsCollapsible)
      {
        icon.MouseLeftButtonUp += Icon_MouseLeftButtonUp;
      }
    }
  }

  private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    IsExpanded = !IsExpanded;
  }
}
