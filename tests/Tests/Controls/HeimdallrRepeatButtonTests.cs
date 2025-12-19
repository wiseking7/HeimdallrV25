using Heimdallr.UI.Controls;
using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Media;

namespace Tests.Controls;


public class HeimdallrRepeatButtonTests
{
  [Fact]
  public void RepeatButton_ApplyTemplate_NoBindingErrors()
  {
    // Arrange
    var button = new HeimdallrRepeatButton
    {
      Icon = IconType.ArrowUpBold,
      IconFill = Brushes.Red
    };

    // Act
    ControlTestHelper.ApplyTemplate(button);

    var icon = button.Template.FindName("icon", button) as HeimdallrIcon;

    // Assert
    Assert.NotNull(icon);
    Assert.NotNull(icon.Fill);
    Assert.NotSame(DependencyProperty.UnsetValue, icon.Fill);
  }
}

