using Heimdallr.UI.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrListView : ListView
{
  /// <summary>현재 정렬된 컬럼</summary>
  public GridViewColumn? SortedColumn { get; private set; }

  /// <summary>컬럼별 정렬 방향 관리</summary>
  private readonly Dictionary<GridViewColumn, ListSortDirection> _columnSortDirections = new();

  #region 종속성 생성자
  static HeimdallrListView()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrListView),
      new FrameworkPropertyMetadata(typeof(HeimdallrListView)));
  }
  #endregion

  #region 컬럼/숨김/복원
  public Dictionary<string, bool> ColumnVisibility
  {
    get => (Dictionary<string, bool>)GetValue(ColumnVisibilityProperty);
    set => SetValue(ColumnVisibilityProperty, value);
  }

  public static readonly DependencyProperty ColumnVisibilityProperty =
      DependencyProperty.Register(nameof(ColumnVisibility), typeof(Dictionary<string, bool>),
          typeof(HeimdallrListView), new PropertyMetadata(new Dictionary<string, bool>(), OnColumnVisibilityChanged));
  #endregion

  #region OnColumnVisibilityChanged 컬럼/숨김/복원 메서드
  private static void OnColumnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrListView lv && lv.View is GridView gv && e.NewValue is Dictionary<string, bool> dict)
    {
      foreach (var col in gv.Columns)
      {
        if (col.Header is string header && dict.TryGetValue(header, out var visible))
          col.Width = visible ? (col.Width == 0 ? 100 : col.Width) : 0;
      }
    }
  }
  #endregion

  #region 컬럼 너비 저장/복원 
  public Dictionary<string, double> ColumnWidths
  {
    get => (Dictionary<string, double>)GetValue(ColumnWidthsProperty);
    set => SetValue(ColumnWidthsProperty, value);
  }

  public static readonly DependencyProperty ColumnWidthsProperty =
      DependencyProperty.Register(nameof(ColumnWidths), typeof(Dictionary<string, double>),
          typeof(HeimdallrListView), new PropertyMetadata(new Dictionary<string, double>()));
  #endregion

  #region HeaderHegight
  public double ColumnHeaderHeight
  {
    get => (double)GetValue(ColumnHeaderHeightProperty);
    set => SetValue(ColumnHeaderHeightProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderHeightProperty =
    DependencyProperty.Register(nameof(ColumnHeaderHeight), typeof(double), typeof(HeimdallrListView), new PropertyMetadata(45d));
  #endregion

  #region HeaderBackground
  public Brush ColumnHeaderBackground
  {
    get => (Brush)GetValue(ColumnHeaderBackgroundProperty);
    set => SetValue(ColumnHeaderBackgroundProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderBackgroundProperty =
    DependencyProperty.Register(nameof(ColumnHeaderBackground), typeof(Brush), typeof(HeimdallrListView), new PropertyMetadata(
        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4B70F5"))));

  #endregion

  #region ColumnHeaderForeground
  public Brush ColumnHeaderForeground
  {
    get => (Brush)GetValue(ColumnHeaderForegroundProperty);
    set => SetValue(ColumnHeaderForegroundProperty, value);
  }

  public static readonly DependencyProperty ColumnHeaderForegroundProperty = DependencyProperty.Register(nameof(ColumnHeaderForeground), typeof(Brush),
      typeof(HeimdallrListView), new PropertyMetadata(Brushes.White));
  #endregion

  #region ColumnHeaderFontSize
  public double ColumnHeaderFontSize
  {
    get => (double)GetValue(ColumnHeaderFontSizeProperty);
    set => SetValue(ColumnHeaderFontSizeProperty, value);
  }

  public static readonly DependencyProperty ColumnHeaderFontSizeProperty = DependencyProperty.Register(nameof(ColumnHeaderFontSize), typeof(double),
      typeof(HeimdallrListView), new PropertyMetadata(18d));
  #endregion

  #region ColumnHeaderFontWeight
  public FontWeight ColumnHeaderFontWeight
  {
    get => (FontWeight)GetValue(ColumnHeaderFontWeightProperty);
    set => SetValue(ColumnHeaderFontWeightProperty, value);
  }

  public static readonly DependencyProperty ColumnHeaderFontWeightProperty = DependencyProperty.Register(nameof(ColumnHeaderFontWeight), typeof(FontWeight),
      typeof(HeimdallrListView), new PropertyMetadata(FontWeights.SemiBold));
  #endregion

  #region SaveColumnWidths 메서드
  public void SaveColumnWidths()
  {
    if (View is GridView gv)
    {
      var dict = new Dictionary<string, double>();
      foreach (var col in gv.Columns)
        if (col.Header is string header)
          dict[header] = col.Width;
      ColumnWidths = dict;
    }
  }
  #endregion

  #region RestoreColumnWidths
  public void RestoreColumnWidths()
  {
    if (View is GridView gv)
    {
      foreach (var col in gv.Columns)
        if (col.Header is string header && ColumnWidths.TryGetValue(header, out var width))
          col.Width = width;
    }
  }
  #endregion

  #region 마우스 선택시 색상
  public Brush SelectedBackground
  {
    get => (Brush)GetValue(SelectedBackgroundProperty);
    set => SetValue(SelectedBackgroundProperty, value);
  }

  public static readonly DependencyProperty SelectedBackgroundProperty =
      DependencyProperty.Register(nameof(SelectedBackground), typeof(Brush),
          typeof(HeimdallrListView), new PropertyMetadata(Brushes.Transparent));
  #endregion

  #region 마우스오버시 색상
  public Brush MouseOverBackground
  {
    get => (Brush)GetValue(MouseOverBackgroundProperty);
    set => SetValue(MouseOverBackgroundProperty, value);
  }

  public static readonly DependencyProperty MouseOverBackgroundProperty =
      DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush),
          typeof(HeimdallrListView), new PropertyMetadata(Brushes.Transparent));
  #endregion

  #region GetContainerForItemOverride
  protected override DependencyObject GetContainerForItemOverride() => new HeimdallrListViewItem();
  #endregion

  #region IsItemItsOwnContainerOverride
  protected override bool IsItemItsOwnContainerOverride(object item) => item is HeimdallrListViewItem;
  #endregion

  #region PrepareContainerForItemOverride
  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);

    if (element is HeimdallrListViewItem lvi)
    {
      // 공통 전달
      lvi.SelectedBackground = SelectedBackground;
      lvi.Background = Brushes.Transparent;
      lvi.Foreground = Foreground;

      // 인덱스 기준 색상 분기
      int index = ItemContainerGenerator.IndexFromContainer(element);

      if (index == 0)
      {
        lvi.MouseOverBackground =
          new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9198A3"));
      }
      else
      {
        lvi.MouseOverBackground =
          new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));
      }
    }
  }
  #endregion

  #region OnPreviewMouseLeftButtonUp 헤더 클릭 정렬
  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);

    if (e.OriginalSource is not DependencyObject source) return;

    // GridViewColumnHeader 탐색
    var header = VisualUpwardSearch<GridViewColumnHeader>(source);
    if (header?.Column == null || ItemsSource == null) return;

    var column = header.Column;

    // 정렬 속성 결정
    string? sortProperty = null;

    if (column.DisplayMemberBinding is Binding binding && !string.IsNullOrEmpty(binding.Path?.Path))
    {
      sortProperty = binding.Path.Path;
    }
    else
    {
      sortProperty = GridViewColumnExtensions.GetSortMemberPath(column);
    }

    if (string.IsNullOrEmpty(sortProperty)) return;

    var collectionView = CollectionViewSource.GetDefaultView(ItemsSource);
    if (collectionView == null) return;

    // 이전 정렬 방향 확인, 같은 컬럼이면 토글
    if (!_columnSortDirections.TryGetValue(column, out var direction))
      direction = ListSortDirection.Ascending;
    else if (SortedColumn == column)
      direction = direction == ListSortDirection.Ascending
          ? ListSortDirection.Descending
          : ListSortDirection.Ascending;
    else
      direction = ListSortDirection.Ascending;

    _columnSortDirections[column] = direction;
    SortedColumn = column;

    // 정렬 적용
    collectionView.SortDescriptions.Clear();
    collectionView.SortDescriptions.Add(new SortDescription(sortProperty, direction));
    collectionView.Refresh();
  }
  #endregion

  #region VisualUpwardSearch 정적 메서드
  /// <summary>VisualTree에서 특정 타입 부모 탐색</summary>
  private static T? VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
  {
    while (source != null && source is not T)
      source = VisualTreeHelper.GetParent(source);
    return source as T;
  }
  #endregion
}
