using Heimdallr.UI.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tests.Controls;

public class HeimdallrThumbTests
{
  [Fact]
  public void Thumb_ApplyTemplate_NoBindingErrors()
  {
    var thumb = new HeimdallrThumb
    {
      Background = Brushes.Blue,
      MouseOverBrush = Brushes.Green,
      IsDraggingBrush = Brushes.Red
    };

    ControlTestHelper.ApplyTemplate(thumb);

    var border = thumb.Template.FindName("border", thumb) as Border;

    Assert.NotNull(border);
    Assert.NotNull(border.Background);
    Assert.NotSame(DependencyProperty.UnsetValue, border.Background);
  }
}

