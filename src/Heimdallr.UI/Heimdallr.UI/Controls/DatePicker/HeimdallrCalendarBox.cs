using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class HeimdallrCalendarBox : ListBox
{
  static HeimdallrCalendarBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrCalendarBox), new FrameworkPropertyMetadata(typeof(HeimdallrCalendarBox)));
  }
}
