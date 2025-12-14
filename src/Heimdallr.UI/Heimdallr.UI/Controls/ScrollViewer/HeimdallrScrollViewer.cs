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

  #region 초기화
  /// <summary>
  /// OnInitialized는 컨트롤이 초기화될 때 호출되는 메서드입니다.
  /// </summary>
  /// <param name="e"></param>
  protected override void OnInitialized(EventArgs e)
  {
    // 기본 ScrollViewer의 초기화 로직을 호출합니다.
    base.OnInitialized(e);

    // 현재 이 컨트롤에 Style이 설정되어 있지 않고,
    // StyleProperty에 로컬 값이 설정되어 있지 않은 경우 (즉, 스타일이 없으면)
    if (Style == null && ReadLocalValue(StyleProperty) == DependencyProperty.UnsetValue)
    {
      // ScrollViewer 타입에 등록된 기본 스타일을 이 컨트롤에 자동으로 적용하도록 합니다.
      // 이렇게 하면 별도로 스타일을 지정하지 않아도 ScrollViewer 기본 스타일을 사용하게 됨.
      SetResourceReference(StyleProperty, typeof(ScrollViewer));
    }
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
