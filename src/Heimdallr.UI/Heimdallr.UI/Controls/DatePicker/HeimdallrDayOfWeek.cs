using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class HeimdallrDayOfWeek : Label
{
  static HeimdallrDayOfWeek()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrDayOfWeek), new FrameworkPropertyMetadata(typeof(HeimdallrDayOfWeek)));
  }
}

