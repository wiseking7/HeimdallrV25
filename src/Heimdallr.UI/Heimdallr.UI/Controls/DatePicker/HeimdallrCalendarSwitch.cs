using System.Windows;
using System.Windows.Controls.Primitives;

namespace Heimdallr.UI.Controls;

public class HeimdallrCalendarSwitch : ToggleButton
{
  static HeimdallrCalendarSwitch()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrCalendarSwitch), new FrameworkPropertyMetadata(typeof(HeimdallrCalendarSwitch)));
  }
}

