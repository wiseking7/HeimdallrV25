using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

/// <summary>
/// HeimdallrToolTip은 사용자 정의 ToolTip 클래스로, Heimdallr UI에서 사용되는 툴팁을 나타냅니다.
/// </summary>
public class HeimdallrToolTip : ToolTip
{
  static HeimdallrToolTip()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrToolTip), new FrameworkPropertyMetadata(typeof(HeimdallrToolTip)));
  }

  protected override void OnOpened(RoutedEventArgs e)
  {
    base.OnOpened(e);

    if (PlacementTarget != null)
    {
      // 컨트롤 폭과 툴팁 폭
      var targetWidth = PlacementTarget.RenderSize.Width;
      var tooltipWidth = ActualWidth;

      // 툴팁의 오른쪽 끝이 컨트롤의 오른쪽 끝과 맞도록
      HorizontalOffset = targetWidth - tooltipWidth;

      // 수직 위치는 기본 Bottom 그대로
      // 필요하면 VerticalOffset으로 조금 올리거나 내릴 수 있음
      VerticalOffset = 3;
    }
  }
}
