using Heimdallr.UI.Controls; // HeimdallrMessageBox namespace
using Heimdallr.UI.Enums;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Heimdallr.UI.Helpers;

/// <summary>
/// ViewModel이 구현해야 하는 BusyMessage 계약
/// ViewModelUIService 데체 삭제 바인딩 오류 없으면 
/// </summary>
public interface IBusyMessageHost
{
  string? BusyMessage { get; set; }
}

/// <summary>
/// WPF UI Thread(Dispatcher) 접근을 중앙에서 관리하기 위한 Helper 클래스
/// 
/// 목적:
/// 1. UI Thread 안전성 보장
/// 2. ViewModel / Service 계층에서 UI 접근 단순화
/// 3. Dispatcher 중복 코드 제거
/// 4. BusyMessage, MessageBox, Command 생성 표준화
/// </summary>
public static class UIHelper
{
  #region RunOnUIThread 기본 메서드
  /// <summary>
  /// UI Thread에서 Action을 실행한다.
  /// 이미 UI Thread라면 바로 실행하고,
  /// 아니라면 Dispatcher.Invoke를 사용한다.
  /// </summary>
  /// <param name="action">UI에서 실행할 코드</param>
  /// <param name="priority">Dispatcher 우선순위</param>
  public static void RunOnUIThread(Action action,
                                   DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null)
    {
      // Application이 없는 경우 fallback
      action();
      return;
    }

    if (dispatcher.CheckAccess())
      action();
    else
      dispatcher.Invoke(action, priority);
  }

  /// <summary>
  /// UI Thread에서 비동기 작업(Func<Task>)을 실행한다.
  /// async/await 패턴을 안전하게 사용하기 위함
  /// </summary>
  public static Task RunOnUIThreadAsync(Func<Task> asyncAction, DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;

    if (dispatcher == null)
    {
      return asyncAction(); // dispatcher 없으면 그냥 awaitable Task 반환
    }

    if (dispatcher.CheckAccess())
    {
      return asyncAction();
    }
    else
    {
      return dispatcher.InvokeAsync(asyncAction, priority).Task;
    }
  }

  /// <summary>
  /// UI Thread에서 값을 반환하는 함수 실행
  /// </summary>
  public static T? RunOnUIThread<T>(Func<T> func,
                                    DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null) return default;

    return dispatcher.CheckAccess() ? func() : dispatcher.Invoke(func, priority);
  }

  #endregion

  #region BusyMessage 통합
  // race condition 방지 토큰
  private static int _busyToken;

  /// <summary>
  /// ViewModel의 BusyMessage를 UI Thread에서 안전하게 설정
  /// </summary>
  public static Task SetBusyMessageAsync(IBusyMessageHost vm, string? busyMessage)
  {
    return RunOnUIThreadAsync(() =>
    {
      vm.BusyMessage = busyMessage;
      return Task.CompletedTask;
    });
  }

  /// <summary>
  /// BusyMessage를 일정 시간 표시 후 제거 (race condition 방지)
  /// </summary>
  public static async Task ShowBusyMessageAsync(IBusyMessageHost vm, string busyMessage, int milliseconds = 2000)
  {
    int token = Interlocked.Increment(ref _busyToken);

    await SetBusyMessageAsync(vm, busyMessage);

    await Task.Delay(milliseconds);

    // 마지막 호출만 메시지 제거
    if (token == _busyToken)
    {
      await SetBusyMessageAsync(vm, null);
    }
  }

  #endregion

  #region HeimdallrMessageBox 통합

  /// <summary>
  /// HeimdallrMessageBox를 UI 스레드에서 안전하게 호출
  /// </summary>
  /// <param name="message">메시지</param>
  /// <param name="caption">제목</param>
  /// <param name="buttons">버튼 유형</param>
  /// <param name="icon">아이콘 유형</param>
  /// <param name="iconFill">아이콘 색상 (옵션)</param>
  /// <returns>사용자 선택 결과</returns>
  public static MessageBoxResult ShowMessageBox(string message,
                                                string caption = "Message",
                                                HeimdallrMessageBoxButtonEnum buttons = HeimdallrMessageBoxButtonEnum.OK,
                                                IconType icon = IconType.None,
                                                Brush? iconFill = null)
  {
    MessageBoxResult result = MessageBoxResult.None;

    RunOnUIThread(() =>
    {
      var dlg = new HeimdallrMessageBox(message, caption, buttons, icon);
      if (iconFill != null)
      {
        dlg.IconFill = iconFill;
      }

      dlg.Owner = Application.Current?.MainWindow;
      dlg.ShowDialog();
      result = dlg.Result;
    });

    return result;
  }
  #endregion

  public static Task<bool> ShowConfirmAsync(string message, string caption = "확인", IconType icon = IconType.Question, Brush? iconFill = null)
  {
    bool result = RunOnUIThread(() =>
    {
      var r = ShowMessageBox(message, caption, HeimdallrMessageBoxButtonEnum.YesNo, icon, iconFill);

      return r == MessageBoxResult.Yes;
    });

    return Task.FromResult(result);
  }
}



