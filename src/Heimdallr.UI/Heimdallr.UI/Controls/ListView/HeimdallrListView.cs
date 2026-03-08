using Heimdallr.UI.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary> 커스텀 ListView: HeimdallrListView - GridView 기반으로 컬럼 클릭 정렬 지원 - 컬럼 숨김/보이기, 컬럼 너비 저장/복원 - 아이템/헤더 스타일 설정 가능 </summary>
public class HeimdallrListView : ListView
{
  /// <summary> 컬럼의 정렬 방향 관리  </summary>
  private readonly Dictionary<GridViewColumn, ListSortDirection> _columnSortDirections = new Dictionary<GridViewColumn, ListSortDirection>();

  /// <summary> 컬럼의 마지막 정렬 컬럼 </summary>
  public GridViewColumn? SortedColumn { get; private set; }

  #region DependencyProperty 선언
  public static readonly DependencyProperty ColumnVisibilityProperty;

  public static readonly DependencyProperty ColumnWidthsProperty;

  public static readonly DependencyProperty ItemRowHeightProperty;

  public static readonly DependencyProperty ItemFontSizeProperty;

  public static readonly DependencyProperty ColumnHeaderHeightProperty;

  public static readonly DependencyProperty ColumnHeaderBackgroundProperty;

  public static readonly DependencyProperty ColumnHeaderForegroundProperty;

  public static readonly DependencyProperty ColumnHeaderFontSizeProperty;

  public static readonly DependencyProperty ColumnHeaderFontWeightProperty;
  #endregion

  #region 컬럼/아이템/헤더 속성
  public Dictionary<string, bool> ColumnVisibility
  {
    get => (Dictionary<string, bool>)GetValue(ColumnVisibilityProperty);
    set => SetValue(ColumnVisibilityProperty, value);
  }

  public Dictionary<string, double> ColumnWidths
  {
    get => (Dictionary<string, double>)GetValue(ColumnWidthsProperty);
    set => SetValue(ColumnWidthsProperty, value);
  }
  public double ItemRowHeight
  {
    get => (double)GetValue(ItemRowHeightProperty);
    set => SetValue(ItemRowHeightProperty, value);
  }

  public double ItemFontSize
  {
    get => (double)GetValue(ItemFontSizeProperty);
    set => SetValue(ItemFontSizeProperty, value);
  }

  public double ColumnHeaderHeight
  {
    get => (double)GetValue(ColumnHeaderHeightProperty);
    set => SetValue(ColumnHeaderHeightProperty, value);
  }

  public Brush ColumnHeaderBackground
  {
    get => (Brush)GetValue(ColumnHeaderBackgroundProperty);
    set => SetValue(ColumnHeaderBackgroundProperty, value);
  }

  public Brush ColumnHeaderForeground
  {
    get => (Brush)GetValue(ColumnHeaderForegroundProperty);
    set => SetValue(ColumnHeaderForegroundProperty, value);
  }
  public double ColumnHeaderFontSize
  {
    get => (double)GetValue(ColumnHeaderFontSizeProperty);
    set => SetValue(ColumnHeaderFontSizeProperty, value);
  }

  public FontWeight ColumnHeaderFontWeight
  {
    get => (FontWeight)GetValue(ColumnHeaderFontWeightProperty);
    set => SetValue(ColumnHeaderFontWeightProperty, value);
  }
  #endregion

  #region ColumnHeader 마우스 오버 배경 / 마우스 오버 테두리 / 클릭 / Pressed 배경 / 클릭 / Pressed 테두리
  public Brush ColumnHeaderHoverBackground
  {
    get => (Brush)GetValue(ColumnHeaderHoverBackgroundProperty);
    set => SetValue(ColumnHeaderHoverBackgroundProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderHoverBackgroundProperty =
      DependencyProperty.Register(
          nameof(ColumnHeaderHoverBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(Brushes.LightBlue));

  // 마우스 오버 테두리
  public Brush ColumnHeaderHoverBorderBrush
  {
    get => (Brush)GetValue(ColumnHeaderHoverBorderBrushProperty);
    set => SetValue(ColumnHeaderHoverBorderBrushProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderHoverBorderBrushProperty =
      DependencyProperty.Register(
          nameof(ColumnHeaderHoverBorderBrush),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(Brushes.Blue));

  // 클릭 / Pressed 배경
  public Brush ColumnHeaderPressedBackground
  {
    get => (Brush)GetValue(ColumnHeaderPressedBackgroundProperty);
    set => SetValue(ColumnHeaderPressedBackgroundProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderPressedBackgroundProperty =
      DependencyProperty.Register(
          nameof(ColumnHeaderPressedBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(Brushes.DarkBlue));

  // 클릭 / Pressed 테두리
  public Brush ColumnHeaderPressedBorderBrush
  {
    get => (Brush)GetValue(ColumnHeaderPressedBorderBrushProperty);
    set => SetValue(ColumnHeaderPressedBorderBrushProperty, value);
  }
  public static readonly DependencyProperty ColumnHeaderPressedBorderBrushProperty =
      DependencyProperty.Register(
          nameof(ColumnHeaderPressedBorderBrush),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(Brushes.Navy));
  #endregion

  #region Item 아이템 배경색상 / 아이템 마우스 오버색상 / 아이템 선택시 배경색상 / 아이템 선택 + 마우스오버 배경색상 / 아이템 텍스트 색상 / // 선택시 글자색 변경 / // 마우스 오버시 글자색 변경
  // 아이템 기본 배경
  public Brush ItemBackground
  {
    get => (Brush)GetValue(ItemBackgroundProperty);
    set => SetValue(ItemBackgroundProperty, value);
  }
  public static readonly DependencyProperty ItemBackgroundProperty =
      DependencyProperty.Register(
          nameof(ItemBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x2E, 0x32, 0x3A))));

  // 아이템 마우스 오버 배경
  public Brush ItemHoverBackground
  {
    get => (Brush)GetValue(ItemHoverBackgroundProperty);
    set => SetValue(ItemHoverBackgroundProperty, value);
  }
  public static readonly DependencyProperty ItemHoverBackgroundProperty =
      DependencyProperty.Register(
          nameof(ItemHoverBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x3B, 0x3F, 0x4A))));

  // 아이템 선택 배경
  public Brush ItemSelectedBackground
  {
    get => (Brush)GetValue(ItemSelectedBackgroundProperty);
    set => SetValue(ItemSelectedBackgroundProperty, value);
  }
  public static readonly DependencyProperty ItemSelectedBackgroundProperty =
      DependencyProperty.Register(
          nameof(ItemSelectedBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x50, 0x55, 0x61))));

  // 아이템 선택 + 마우스오버 배경
  public Brush ItemSelectedHoverBackground
  {
    get => (Brush)GetValue(ItemSelectedHoverBackgroundProperty);
    set => SetValue(ItemSelectedHoverBackgroundProperty, value);
  }
  public static readonly DependencyProperty ItemSelectedHoverBackgroundProperty =
      DependencyProperty.Register(
          nameof(ItemSelectedHoverBackground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x60, 0x68, 0x80))));

  // 아이템 텍스트 색상
  public Brush ItemForeground
  {
    get => (Brush)GetValue(ItemForegroundProperty);
    set => SetValue(ItemForegroundProperty, value);
  }
  public static readonly DependencyProperty ItemForegroundProperty =
      DependencyProperty.Register(
          nameof(ItemForeground),
          typeof(Brush),
          typeof(HeimdallrListView),
          new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xEA, 0xEA, 0xEA))));
  #endregion

  #region 정적 생성자: DependencyProperty 초기화 및 스타일 설정
  static HeimdallrListView()
  {
    // 컬럼 숨김/보이기용 
    ColumnVisibilityProperty = DependencyProperty.Register("ColumnVisibility", typeof(Dictionary<string, bool>), typeof(HeimdallrListView), new PropertyMetadata(new Dictionary<string, bool>(), OnColumnVisibilityChanged));

    // 컬럼너비 저장용
    ColumnWidthsProperty = DependencyProperty.Register("ColumnWidths", typeof(Dictionary<string, double>), typeof(HeimdallrListView), new PropertyMetadata(new Dictionary<string, double>()));

    // 아이템 높이
    ItemRowHeightProperty = DependencyProperty.Register("ItemRowHeight", typeof(double), typeof(HeimdallrListView), new PropertyMetadata(35.0));

    // 아이템 폰트 크기 (변경 시 ItemRowHeight 자동 조정)
    ItemFontSizeProperty = DependencyProperty.Register("ItemFontSize", typeof(double), typeof(HeimdallrListView), new PropertyMetadata(14.0, OnItemFontSizeChanged));

    // 컬럼 헤더 높이
    ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(HeimdallrListView), new PropertyMetadata(45.0));

    // 컬럼 헤더 배경
    ColumnHeaderBackgroundProperty = DependencyProperty.Register("ColumnHeaderBackground", typeof(Brush), typeof(HeimdallrListView), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4B70F5"))));

    // 컬럼 헤더 글자색 
    ColumnHeaderForegroundProperty = DependencyProperty.Register("ColumnHeaderForeground", typeof(Brush), typeof(HeimdallrListView), new PropertyMetadata(Brushes.White));

    // 컬럼 헤더 글자 크기
    ColumnHeaderFontSizeProperty = DependencyProperty.Register("ColumnHeaderFontSize", typeof(double), typeof(HeimdallrListView), new PropertyMetadata(18.0));

    // 컬럼 헤더 글자 굵기
    ColumnHeaderFontWeightProperty = DependencyProperty.Register("ColumnHeaderFontWeight", typeof(FontWeight), typeof(HeimdallrListView), new PropertyMetadata(FontWeights.SemiBold));

    // 기본 스타일 지정
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrListView), new FrameworkPropertyMetadata(typeof(HeimdallrListView)));
  }
  #endregion

  #region 컬럼 숨김/보이기 콜백
  private static void OnColumnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is HeimdallrListView { View: GridView view }) || !(e.NewValue is Dictionary<string, bool> dictionary))
    {
      return;
    }

    foreach (GridViewColumn column in view.Columns)
    {
      if (column.Header is string key && dictionary.TryGetValue(key, out var value))
      {
        // 숨김: Width 0, 보이기: 기존 너비 유지 혹은 기본 100
        column.Width = ((!value) ? 0.0 : ((column.Width == 0.0) ? 100.0 : column.Width));
      }
    }
  }
  #endregion

  #region 아이템 폰트 크기 변경 콜백
  private static void OnItemFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrListView heimdallrListView && e.NewValue is double num)
    {
      // 아이템 높이를 폰트 크기에 맞춰 자동 조정
      heimdallrListView.ItemRowHeight = num + 18.0;
    }
  }
  #endregion

  #region 컬럼 너비 저장/복원
  public void SaveColumnWidths()
  {
    if (!(base.View is GridView gridView))
    {
      return;
    }

    Dictionary<string, double> dictionary = new Dictionary<string, double>();
    foreach (GridViewColumn column in gridView.Columns)
    {
      if (column.Header is string key)
      {
        dictionary[key] = column.Width;
      }
    }

    ColumnWidths = dictionary;
  }

  public void RestoreColumnWidths()
  {
    if (!(base.View is GridView gridView))
    {
      return;
    }

    foreach (GridViewColumn column in gridView.Columns)
    {
      if (column.Header is string key && ColumnWidths.TryGetValue(key, out var value))
      {
        column.Width = value;
      }
    }
  }
  #endregion

  #region ListViewItem 오버라이드
  protected override DependencyObject GetContainerForItemOverride()
  {
    return new HeimdallrListViewItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is HeimdallrListViewItem;
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);
    if (element is HeimdallrListViewItem heimdallrListViewItem)
    {
      heimdallrListViewItem.Background = Brushes.Transparent;
      heimdallrListViewItem.Foreground = base.Foreground;
    }
  }
  #endregion

  #region 컬럼 클릭 정렬
  /// <summary> 컬럼 헤더 클릭 시 정렬 처리 - DisplayMemberBinding 있는 컬럼은 Binding.Path.Path 기준 - Template-only 컬럼은 GridViewColumnExtensions.GetSortMemberPath 필요 </summary>
  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);

    if (!(e.OriginalSource is DependencyObject source))
      return;

    // 클릭한 VisualTree 상위에서 GridViewColumnHeader 찾기
    GridViewColumnHeader? header = VisualUpwardSearch<GridViewColumnHeader>(source);
    if (header?.Column == null || base.ItemsSource == null)
      return;

    GridViewColumn column = header.Column;

    // 정렬할 속성 결정
    string? sortProperty = null;

    // DisplayMemberBinding 있는 컬럼이면 Binding.Path.Path 사용
    if (column.DisplayMemberBinding is Binding binding && !string.IsNullOrEmpty(binding.Path?.Path))
      sortProperty = binding.Path.Path;
    else
      // Template-only 컬럼용: SortMemberPath 없으면 정렬하지 않음
      sortProperty = GridViewColumnExtensions.GetSortMemberPath(column);

    if (string.IsNullOrEmpty(sortProperty))
      return; // SortMemberPath 없으면 정렬 무시 (번호 컬럼 등)

    // 컬렉션 뷰 가져오기
    ICollectionView? collectionView = CollectionViewSource.GetDefaultView(base.ItemsSource);
    if (collectionView == null)
      return;

    // 정렬 방향 결정
    ListSortDirection direction;
    if (!_columnSortDirections.TryGetValue(column, out direction))
      direction = ListSortDirection.Ascending;
    else if (SortedColumn == column && direction == ListSortDirection.Ascending)
      direction = ListSortDirection.Descending;
    else
      direction = ListSortDirection.Ascending;

    _columnSortDirections[column] = direction;
    SortedColumn = column;

    // 기존 정렬 제거 후 새로운 정렬 적용
    collectionView.SortDescriptions.Clear();
    collectionView.SortDescriptions.Add(new SortDescription(sortProperty, direction));
    collectionView.Refresh();
  }
  #endregion

  #region VisualTree 검색 유틸리티
  /// <summary> 상위 VisualTree에서 T 타입 검색 </summary>
  private static T? VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
  {
    while (source != null && !(source is T))
    {
      source = VisualTreeHelper.GetParent(source);
    }

    return source as T;
  }
  #endregion
}
