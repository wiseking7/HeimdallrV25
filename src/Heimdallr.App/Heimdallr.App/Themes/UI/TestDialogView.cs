using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.App.Themes.UI;

public class TestDialogView : ContentControl
{
  static TestDialogView()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(TestDialogView), new FrameworkPropertyMetadata(typeof(TestDialogView)));
  }
}
