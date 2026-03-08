using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class HeimdallrChevronButton : Button
{
  static HeimdallrChevronButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrChevronButton), new FrameworkPropertyMetadata(typeof(HeimdallrChevronButton)));
  }
}
