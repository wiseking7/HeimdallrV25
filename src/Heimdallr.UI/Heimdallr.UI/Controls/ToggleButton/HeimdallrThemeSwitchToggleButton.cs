using System.Windows;
using System.Windows.Controls.Primitives;

namespace Heimdallr.UI.Controls;

public class HeimdallrThemeSwitchToggleButton : ToggleButton
{
  static HeimdallrThemeSwitchToggleButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrThemeSwitchToggleButton),
      new FrameworkPropertyMetadata(typeof(HeimdallrThemeSwitchToggleButton)));
  }
}
