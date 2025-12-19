using System.Windows;

namespace Tests.Controls;

public static class ControlTestHelper
{
  public static void ApplyTemplate(FrameworkElement control)
  {
    var window = new Window
    {
      Content = control,
      Width = 200,
      Height = 200,
      ShowInTaskbar = false,
      WindowStyle = WindowStyle.None
    };

    window.Show();
    control.ApplyTemplate();

    DispatcherUtil.DoEvents();

    window.Close();
  }
}

