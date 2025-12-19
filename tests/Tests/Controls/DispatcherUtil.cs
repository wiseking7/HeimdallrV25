using System.Windows.Threading;

namespace Tests.Controls;

public static class DispatcherUtil
{
  public static void DoEvents()
  {
    DispatcherFrame frame = new();
    Dispatcher.CurrentDispatcher.BeginInvoke(
      DispatcherPriority.Background,
      new DispatcherOperationCallback(_ =>
      {
        frame.Continue = false;
        return null;
      }),
      null);

    Dispatcher.PushFrame(frame);
  }
}

