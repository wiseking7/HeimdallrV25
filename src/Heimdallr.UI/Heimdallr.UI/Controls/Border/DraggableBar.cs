using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class DraggableBar : Border
{
  static DraggableBar()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableBar), new FrameworkPropertyMetadata(typeof(DraggableBar)));
  }
}
