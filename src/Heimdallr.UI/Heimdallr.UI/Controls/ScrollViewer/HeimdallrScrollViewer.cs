using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Heimdallr.UI.Controls;

/// <summary>
/// ScrollViewer 커스터마이징
/// </summary>
public class HeimdallrScrollViewer : ScrollViewer
{
  #region 종속성 생성자
  static HeimdallrScrollViewer()
  {
    // 기본 스타일 키를 HeimdallrScrollViewer로 설정하여 테마에서 해당 스타일을 찾도록 함
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrScrollViewer),
      new FrameworkPropertyMetadata(typeof(HeimdallrScrollViewer)));
  }
  #endregion

  #region 마우스 휠
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    // 마우스 휠 이벤트가 이미 처리된 경우, 추가 처리를 하지 않습니다.
    if (e.Handled) { return; }

    if (ScrollableHeight > 0)
    {
      // 마우스 휠 이벤트가 발생했을 때, ScrollViewer의 VerticalOffset을 조정합니다.
      base.OnMouseWheel(e);
    }
    // 만약 스크롤 가능한 영역이 없으면 마우스 휠 이벤트를 무시해서 불필요한 스크롤 동작 방지
  }
  #endregion
}
