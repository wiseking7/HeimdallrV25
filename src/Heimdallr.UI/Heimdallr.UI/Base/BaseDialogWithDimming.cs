using Heimdallr.UI.Helpers;

namespace Heimdallr.UI.Base;

public abstract class BaseDialogWithDimming : ViewModelBase, IDialogAware
{
  private readonly Lazy<DimmingManager> _dimmingManager;

  protected BaseDialogWithDimming(IContainerProvider containerProvider) : base(containerProvider)
  {
    _dimmingManager = ResolveLazy<DimmingManager>();

    RequestClose = new DialogCloseListener();  // RequestClose 초기화
  }

  public string? Title { get; set; }

  /// <summary>
  /// 다이얼로그 닫기 요청 시 호출되는 이벤트
  /// </summary>
  public DialogCloseListener RequestClose { get; }

  /// <summary>
  /// 다이얼로그를 닫을 수 있는지 여부를 반환 (기본값은 true)
  /// </summary>
  public virtual bool CanCloseDialog() => true;

  /// <summary>
  /// 다이얼로그가 닫힐 때 호출됩니다. 디밍 해제
  /// </summary>
  public virtual void OnDialogClosed()
  {
    _dimmingManager.Value.Dimming(isDimming: false);
  }

  /// <summary>
  /// 다이얼로그가 열릴 때 호출됩니다. 디밍 적용
  /// </summary>
  public virtual void OnDialogOpened(IDialogParameters parameters)
  {
    _dimmingManager.Value.Dimming(isDimming: true);
  }
}
