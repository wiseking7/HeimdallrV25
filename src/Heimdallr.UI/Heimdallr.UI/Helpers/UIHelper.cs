using Heimdallr.UI.Controls; // HeimdallrMessageBox namespace
using Heimdallr.UI.Enums;
using Prism.Commands;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Heimdallr.UI.Helpers;

/// <summary>
/// Prism + WPF UIHelper 확장
/// 백그라운드 작업, 비동기 작업, ViewModel에서 UI 조작 등 UI 스레드 접근이 필요한 모든 곳에서 사용합니다.
/// </summary>
public static class UIHelper
{
  #region RunOnUIThread 기본 메서드

  public static void RunOnUIThread(Action action,
                                   DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null) return;

    if (dispatcher.CheckAccess())
      action();
    else
      dispatcher.Invoke(action, priority);
  }

  public static async Task RunOnUIThreadAsync(Func<Task> asyncAction,
                                              DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null) return;

    if (dispatcher.CheckAccess())
      await asyncAction();
    else
      await dispatcher.InvokeAsync(asyncAction, priority);
  }

  public static T? RunOnUIThread<T>(Func<T> func,
                                    DispatcherPriority priority = DispatcherPriority.Normal)
  {
    var dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null) return default;

    if (dispatcher.CheckAccess())
      return func();
    else
      return dispatcher.Invoke(func, priority);
  }

  #endregion

  #region BusyMessage 통합

  /// <summary>
  /// BusyMessage를 안전하게 UI 스레드에서 변경합니다.
  /// ViewModel의 BusyMessage 바인딩 속성에 바로 적용 가능
  /// </summary>
  /// <param name="viewModel">ViewModel</param>
  /// <param name="busyMessage">표시할 메시지</param>
  public static void SetBusyMessage(dynamic viewModel, string busyMessage)
  {
    RunOnUIThread(() => viewModel.BusyMessage = busyMessage);
  }

  /// <summary>
  /// 일정 시간 동안 BusyMessage 표시
  /// </summary>
  /// <param name="viewModel">ViewModel</param>
  /// <param name="busyMessage">메시지</param>
  /// <param name="milliseconds">표시 시간</param>
  public static async Task ShowBusyMessageAsync(dynamic viewModel, string busyMessage, int milliseconds = 2000)
  {
    SetBusyMessage(viewModel, busyMessage);
    await Task.Delay(milliseconds);
    SetBusyMessage(viewModel, string.Empty);
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

      dlg.Owner = Application.Current.MainWindow;
      dlg.ShowDialog();
      result = dlg.Result;
    });

    return result;
  }

  #endregion

  #region Prism DelegateCommand 통합

  /// <summary>
  /// DelegateCommand 생성, UI 스레드 + BusyMessage + MessageBox 통합
  /// </summary>
  /// <param name="execute">실행 액션</param>
  /// <param name="canExecute">실행 가능 여부</param>
  /// <returns>DelegateCommand</returns>
  public static DelegateCommand CreateDelegateCommand(Action execute, Func<bool>? canExecute = null)
  {
    return new DelegateCommand(() => RunOnUIThread(execute), canExecute);
  }

  /// <summary>
  /// DelegateCommand<T> 생성, UI 스레드 + BusyMessage + MessageBox 통합
  /// </summary>
  /// <typeparam name="T">파라미터 타입</typeparam>
  /// <param name="execute">실행 액션</param>
  /// <param name="canExecute">실행 가능 여부</param>
  /// <returns>DelegateCommand&lt;T&gt;</returns>
  public static DelegateCommand<T> CreateDelegateCommand<T>(Action<T> execute, Func<T, bool>? canExecute = null)
  {
    return new DelegateCommand<T>((param) => RunOnUIThread(() => execute(param)), canExecute);
  }

  #endregion
}



