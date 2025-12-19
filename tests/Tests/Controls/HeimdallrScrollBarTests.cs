using Heimdallr.UI.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tests.Controls;

public class HeimdallrScrollBarTests
{
  [Fact]
  public void ScrollBar_ApplyTemplate_NoIconFillBindingErrors()
  {
    var scrollBar = new HeimdallrScrollBar
    {
      Orientation = Orientation.Vertical,
      IconFill = Brushes.Orange
    };

    ControlTestHelper.ApplyTemplate(scrollBar);

    var up = scrollBar.Template.FindName("PART_LineUpButton", scrollBar) as HeimdallrRepeatButton;
    var down = scrollBar.Template.FindName("PART_LineDownButton", scrollBar) as HeimdallrRepeatButton;

    Assert.NotNull(up);
    Assert.NotNull(down);

    // 내부 아이콘 접근
    var upIcon = up.Template.FindName("icon", up) as HeimdallrIcon;
    var downIcon = down.Template.FindName("icon", down) as HeimdallrIcon;

    Assert.NotNull(upIcon);
    Assert.NotNull(downIcon);

    Assert.NotSame(DependencyProperty.UnsetValue, upIcon.Fill);
    Assert.NotSame(DependencyProperty.UnsetValue, downIcon.Fill);
  }
}

