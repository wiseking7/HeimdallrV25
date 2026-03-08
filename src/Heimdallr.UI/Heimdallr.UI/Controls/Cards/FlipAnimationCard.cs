using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// CardView: 카드 형태의 사용자 정의 컨트롤
/// 이 컨트롤은 제목, 설명, 이미지 속성을 갖고 있으며,
/// 클릭 이벤트를 처리할 수 있습니다.
/// </summary>

// ContentProperty를 BackContent로 지정
[ContentProperty(nameof(BackContent))]
public class FlipAnimationCard : ContentControl
{
  #region 생성자
  // 정적 생성자
  static FlipAnimationCard()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipAnimationCard), new FrameworkPropertyMetadata(typeof(FlipAnimationCard)));
  }

  /// <summary>
  /// 생성자
  /// </summary>
  public FlipAnimationCard()
  {
    ToolTipOpening += FlipAnimationCard_ToolTipOpening;
  }
  #endregion

  #region FlipAnimationCard_ToolTipOpening 이벤트
  private void FlipAnimationCard_ToolTipOpening(object sender, ToolTipEventArgs e)
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

  #region Title, Description
  /// <summary> 카드제목 프로퍼티 </summary>
  public string CardTitle
  {
    get => (string)GetValue(CardTitleProperty);
    set => SetValue(CardTitleProperty, value);
  }

  public static readonly DependencyProperty CardTitleProperty =
    DependencyProperty.Register("CardTitle", typeof(string), typeof(FlipAnimationCard), new PropertyMetadata(string.Empty));

  /// <summary> 카드설명 프로퍼티  </summary>
  public string CardDescription
  {
    get => (string)GetValue(CardDescriptionProperty);
    set => SetValue(CardDescriptionProperty, value);
  }

  public static readonly DependencyProperty CardDescriptionProperty =
    DependencyProperty.Register("CardDescription", typeof(string), typeof(FlipAnimationCard), new PropertyMetadata(string.Empty));

  #endregion

  #region Card width, height
  /// <summary> 카드 너비 설정 </summary>
  public double CardWidth
  {
    get => (double)GetValue(CardWidthProperty);
    set => SetValue(CardWidthProperty, value);
  }

  public static readonly DependencyProperty CardWidthProperty = DependencyProperty.Register(nameof(CardWidth), typeof(double), typeof(FlipAnimationCard),
    new PropertyMetadata(100.0));

  /// <summary> 카드의 높이 설정 </summary>
  public double CardHeight
  {
    get => (double)GetValue(CardHeightProperty);
    set => SetValue(CardHeightProperty, value);
  }
  public static readonly DependencyProperty CardHeightProperty = DependencyProperty.Register(nameof(CardHeight), typeof(double), typeof(FlipAnimationCard),
    new PropertyMetadata(100.0));
  #endregion

  #region Card BorderThickness, BorderBrush
  /// <summary> BorderThickness 설정 </summary>
  public Thickness CardBorderThickness
  {
    get => (Thickness)GetValue(CardBorderThicknessProperty);
    set => SetValue(CardBorderThicknessProperty, value);
  }
  public static readonly DependencyProperty CardBorderThicknessProperty = DependencyProperty.Register(nameof(CardBorderThickness), typeof(Thickness), typeof(FlipAnimationCard),
    new PropertyMetadata(new Thickness(1)));

  /// <summary> BorderBrush 설정 </summary>
  public Brush CardBorderBrush
  {
    get => (Brush)GetValue(CardBorderBrushProperty);
    set => SetValue(CardBorderBrushProperty, value);
  }
  public static readonly DependencyProperty CardBorderBrushProperty = DependencyProperty.Register(nameof(CardBorderBrush), typeof(Brush), typeof(FlipAnimationCard),
    new PropertyMetadata(Brushes.Gray));
  #endregion

  #region Card : front/back 개별 설정
  /// <summary> 앞면 모서리 둥글기 설정 </summary>
  public CornerRadius FrontCornerRadius
  {
    get => (CornerRadius)GetValue(FrontCornerRadiusProperty);
    set => SetValue(FrontCornerRadiusProperty, value);
  }
  public static readonly DependencyProperty FrontCornerRadiusProperty = DependencyProperty.Register(nameof(FrontCornerRadius), typeof(CornerRadius), typeof(FlipAnimationCard),
    new PropertyMetadata(new CornerRadius(7)));

  /// <summary> 뒷면 모서리 둥글기 설정 </summary>
  public CornerRadius BackCornerRadius
  {
    get => (CornerRadius)GetValue(BackCornerRadiusProperty);
    set => SetValue(BackCornerRadiusProperty, value);
  }

  public static readonly DependencyProperty BackCornerRadiusProperty = DependencyProperty.Register(nameof(BackCornerRadius), typeof(CornerRadius), typeof(FlipAnimationCard),
  new PropertyMetadata(new CornerRadius(7)));
  #endregion

  #region Card : Front/Back 배경색 설정
  /// <summary> 앞면 배경색 설정 </summary>
  public Brush FrontBackground
  {
    get => (Brush)GetValue(FrontBackgroundProperty);
    set => SetValue(FrontBackgroundProperty, value);
  }
  public static readonly DependencyProperty FrontBackgroundProperty = DependencyProperty.Register(nameof(FrontBackground), typeof(Brush), typeof(FlipAnimationCard),
    new PropertyMetadata(Brushes.White));

  /// <summary> 뒷면 배경색 설정 </summary>
  public Brush BackBackground
  {
    get => (Brush)GetValue(BackBackgroundProperty);
    set => SetValue(BackBackgroundProperty, value);
  }
  public static readonly DependencyProperty BackBackgroundProperty = DependencyProperty.Register(nameof(BackBackground), typeof(Brush), typeof(FlipAnimationCard),
    new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#21201E"))));
  #endregion

  #region BackContent
  /// <summary> 뒷면 컨텐츠/ </summary>
  public object BackContent
  {
    get => GetValue(BackContentProperty);
    set => SetValue(BackContentProperty, value);
  }

  /// <summary> 뒷면 컨텐츠 DependencyProperty </summary>
  public static readonly DependencyProperty BackContentProperty =
    DependencyProperty.Register(nameof(BackContent), typeof(object), typeof(FlipAnimationCard), new PropertyMetadata(null));
  #endregion

  #region Icon 사용자 지정 속성
  /// <summary> IconType 속성 추가 </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(FlipAnimationCard),
    new PropertyMetadata(IconType.None));

  /// <summary> 이이콘 사이즈 너비,높이 </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }
  public static readonly DependencyProperty IconSizeProperty =
    DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(FlipAnimationCard), new PropertyMetadata(70.0));

  /// <summary> 아이콘 색상지정 </summary>
  public Brush Fill
  {
    get => (Brush)GetValue(FillProperty);
    set => SetValue(FillProperty, value);
  }
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(FlipAnimationCard),
    new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"))));

  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }
  public static readonly DependencyProperty IconMouseOverFillProperty =
      DependencyProperty.Register(nameof(IconMouseOverFill), typeof(Brush), typeof(FlipAnimationCard), new PropertyMetadata(Brushes.DeepSkyBlue));
  #endregion

  #region Image
  /// <summary> ImageType Enum 값으로 지정된 아이콘 이미지를 표시 (Base64 인코딩된 이미지 사용) </summary>
  public ImageType Image
  {
    get => (ImageType)GetValue(ImageProperty);
    set => SetValue(ImageProperty, value);
  }
  public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageType), typeof(FlipAnimationCard),
    new PropertyMetadata(ImageType.None));
  #endregion
}
