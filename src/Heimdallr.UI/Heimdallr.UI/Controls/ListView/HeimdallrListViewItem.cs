using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrListViewItem : ListViewItem
{
  // 선택 시 배경
  public Brush? SelectedBackground { get; set; }

  // 마우스 오버 시 배경
  public Brush? MouseOverBackground { get; set; }

  #region OnMouseEnter 재정의
  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);

    if (!IsSelected && MouseOverBackground != null)
    {
      Background = MouseOverBackground;
    }
  }
  #endregion

  #region OnMouseEnter 재정의
  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);

    if (!IsSelected)
    {
      Background = Brushes.Transparent;
    }
  }
  #endregion

  #region OnMouseEnter 재정의
  protected override void OnSelected(RoutedEventArgs e)
  {
    base.OnSelected(e);

    if (SelectedBackground != null)
    {
      Background = SelectedBackground;
    }
  }
  #endregion

  #region OnUnselected 재정의
  protected override void OnUnselected(RoutedEventArgs e)
  {
    base.OnUnselected(e);

    Background = Brushes.Transparent;
  }
  #endregion

  #region 생성자
  static HeimdallrListViewItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrListViewItem),
     new FrameworkPropertyMetadata(typeof(HeimdallrListViewItem)));
  }
  #endregion
}
