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

  #region OnApplyTemplate 메서드
  /// <summary>
  /// 템플릿이 적용된 후에 호출되는 메서드입니다.
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // 템플릿이 적용된 후에 스타일을 설정
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
      // 수직 스크롤 처리
      base.OnMouseWheel(e);
    }
    // 만약 스크롤 가능한 영역이 없으면 마우스 휠 이벤트를 무시해서 불필요한 스크롤 동작 방지
  }
  #endregion
}
