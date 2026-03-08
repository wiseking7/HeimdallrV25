using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class HeimdallrListViewItem : ListViewItem
{
  #region 생성자
  static HeimdallrListViewItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrListViewItem),
     new FrameworkPropertyMetadata(typeof(HeimdallrListViewItem)));
  }
  #endregion
}
