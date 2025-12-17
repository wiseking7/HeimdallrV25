using System.Windows;

namespace Heimdallr.Utility;

// UI 스레드 작업 관리 - UI 스레드에서 비동기/동기 작업을 실행하는 클래스
public class UIThreadExecutor
{
  // 비동기 작업을 UI 스레드에서 실행
  public Task RunOnUiThreadAsync(Func<Task> action)
  {
    // UI 스레드에서 작업을 실행하도록 처리
    if (Application.Current?.Dispatcher == null)
    {
      return Task.Run(action); // Dispatcher가 없으면 별도의 스레드에서 실행
    }
    return Application.Current.Dispatcher.CheckAccess() ? action() : Application.Current.Dispatcher.InvokeAsync(action).Task;
  }

  // 동기 작업을 UI 스레드에서 실행
  public void RunOnUiThread(Action action)
  {
    // UI 스레드에서 작업을 실행
    if (Application.Current?.Dispatcher == null)
    {
      action(); // Dispatcher가 없으면 즉시 실행
    }
    else if (Application.Current.Dispatcher.CheckAccess())
    {
      action(); // UI 스레드에서 실행
    }
    else
    {
      Application.Current.Dispatcher.Invoke(action); // 동기적으로 UI 스레드에서 실행
    }
  }
}

