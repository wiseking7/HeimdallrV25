namespace Heimdallr.Utility;

// 자원 관리 - 취소 토큰 관리 및 자원 해제 클래스
public class CancellationManager : IDisposable
{
  private CancellationTokenSource _cts = new CancellationTokenSource();

  // 현재 취소 토큰을 반환
  public CancellationToken CancellationToken => _cts.Token;

  // 현재 실행 중인 작업을 취소하고, 새로운 취소 토큰을 생성
  public void CancelCurrentTask()
  {
    if (!_cts.IsCancellationRequested)
    {
      _cts.Cancel(); // 취소
      _cts.Dispose(); // 자원 해제
      _cts = new CancellationTokenSource(); // 새로운 토큰 생성
    }
  }

  public void Dispose()
  {
    _cts?.Cancel();
  }
}

