using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrThumb : Thumb
{
  #region 종속성 생성자
  static HeimdallrThumb()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrThumb),
        new FrameworkPropertyMetadata(typeof(HeimdallrThumb)));
  }
  #endregion

  #region CornerRadius 
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }

  public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
          typeof(HeimdallrThumb), new FrameworkPropertyMetadata(new CornerRadius(0)));
  #endregion

  #region BackgroundBrush 
  public Brush BackgroundBrush
  {
    get => (Brush)GetValue(BackgroundBrushProperty);
    set => SetValue(BackgroundBrushProperty, value);
  }

  public static readonly DependencyProperty BackgroundBrushProperty =
      DependencyProperty.Register(nameof(BackgroundBrush), typeof(Brush),
          typeof(HeimdallrThumb), new PropertyMetadata(Brushes.Gray));
  #endregion

  #region MouseOverBrush 
  public Brush MouseOverBrush
  {
    get => (Brush)GetValue(MouseOverBrushProperty);
    set => SetValue(MouseOverBrushProperty, value);
  }

  public static readonly DependencyProperty MouseOverBrushProperty =
      DependencyProperty.Register(nameof(MouseOverBrush), typeof(Brush),
          typeof(HeimdallrThumb), new PropertyMetadata(Brushes.DarkGray));
  #endregion

  #region DraggingBrush
  public Brush IsDraggingBrush
  {
    get => (Brush)GetValue(IsDraggingBrushProperty);
    set => SetValue(IsDraggingBrushProperty, value);
  }

  public static readonly DependencyProperty IsDraggingBrushProperty =
      DependencyProperty.Register(nameof(IsDraggingBrush), typeof(Brush),
          typeof(HeimdallrThumb), new PropertyMetadata(Brushes.DimGray));
  #endregion
}

