using System.Windows;
using System.Windows.Controls;

namespace Heimdallr.UI.Extensions;

public static class GridViewColumnExtensions
{
  public static readonly DependencyProperty SortMemberPathProperty =
      DependencyProperty.RegisterAttached(
          "SortMemberPath",
          typeof(string),
          typeof(GridViewColumnExtensions),
          new PropertyMetadata(null));

  public static void SetSortMemberPath(GridViewColumn column, string value)
      => column.SetValue(SortMemberPathProperty, value);

  public static string? GetSortMemberPath(GridViewColumn column)
      => (string?)column.GetValue(SortMemberPathProperty);
}
