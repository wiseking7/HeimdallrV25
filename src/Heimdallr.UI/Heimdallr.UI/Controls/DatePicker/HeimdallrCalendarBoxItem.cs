using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Controls;

public class HeimdallrCalendarBoxItem : ListBoxItem
{
  public string? DateFormat { get; set; }

  public DateTime Date
  {
    get => (DateTime)GetValue(DateProperty);
    set => SetValue(DateProperty, value);
  }

  public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(HeimdallrCalendarBoxItem));

  public bool IsCurrentMonth    // 이번 달 날짜인지 여부
  {
    get { return (bool)GetValue(IsCurrentMonthProperty); }
    set { SetValue(IsCurrentMonthProperty, value); }
  }

  public static readonly DependencyProperty IsCurrentMonthProperty =
      DependencyProperty.Register("IsCurrentMonth", typeof(bool), typeof(HeimdallrCalendarBoxItem), new PropertyMetadata(false));


  static HeimdallrCalendarBoxItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrCalendarBoxItem), new FrameworkPropertyMetadata(typeof(HeimdallrCalendarBoxItem)));
  }
}
