using Heimdallr.UI.Enums;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrTreeViewItem : TreeViewItem
{
  #region CornerRadius
  /// <summary>
  /// 트리뷰 아이템의 테두리 둥글기 정도를 지정하는 속성입니다.
  /// 예를 들어 "4,4,4,4"와 같이 지정하면 테두리가 둥글게 렌더링됩니다.
  /// </summary>
  public CornerRadius CornerRadius
  {
    get => (CornerRadius)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }
  /// <summary>
  /// 기본값 
  /// </summary>
  public static readonly DependencyProperty CornerRadiusProperty =
    DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
      typeof(HeimdallrTreeViewItem), new FrameworkPropertyMetadata());
  #endregion

  #region ExpandIcon
  /// <summary>
  /// 아이템이 확장될 때 표시할 아이콘을 지정합니다.
  /// 예: 화살표 아래쪽(ChevronDown), + 기호(Plus) 등
  /// </summary>
  public IconType ExpandIcon
  {
    get => (IconType)GetValue(ExpandIconProperty);
    set => SetValue(ExpandIconProperty, value);
  }
  /// <summary>
  /// 기본값 IconType 없음
  /// </summary>
  public static readonly DependencyProperty ExpandIconProperty =
      DependencyProperty.Register(nameof(ExpandIcon), typeof(IconType), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(IconType.None));
  #endregion

  #region ExpandIconFill
  /// <summary>
  /// 확장 아이콘(ExpandIcon)의 색상 지정용 Brush, 기본값 밝은 보라색(#B13BFF)
  /// </summary>
  public Brush ExpandIconFill
  {
    get { return (Brush)GetValue(ExpandIconFillProperty); }
    set { SetValue(ExpandIconFillProperty, value); }
  }
  /// <summary>
  /// 기본값 보라색
  /// </summary>
  public static readonly DependencyProperty ExpandIconFillProperty =
      DependencyProperty.Register(nameof(ExpandIconFill), typeof(Brush), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B13BFF"))));
  #endregion

  #region CollapseIcon
  /// <summary>
  /// 아이템이 축소될 때 표시할 아이콘을 지정합니다.
  /// 예: 화살표 위쪽(ChevronUp), - 기호(Minus) 등
  /// </summary>
  public IconType CollapseIcon
  {
    get => (IconType)GetValue(CollapseIconProperty);
    set => SetValue(CollapseIconProperty, value);
  }
  /// <summary>
  /// 기본값 IconType 없음
  /// </summary>
  public static readonly DependencyProperty CollapseIconProperty =
      DependencyProperty.Register(nameof(CollapseIcon), typeof(IconType), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(IconType.None));
  #endregion

  #region CollapseIconFill
  /// <summary>
  /// 축소 아이콘(CollapseIcon)의 색상 지정용 Brush, 기본값 연두색(#9BC09C)
  /// </summary>
  public Brush CollapseIconFill
  {
    get { return (Brush)GetValue(CollapseIconFillProperty); }
    set { SetValue(CollapseIconFillProperty, value); }
  }
  /// <summary>
  /// 기본값 연두색
  /// </summary>
  public static readonly DependencyProperty CollapseIconFillProperty =
      DependencyProperty.Register(nameof(CollapseIconFill), typeof(Brush), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9BC09C"))));
  #endregion

  #region Icon
  /// <summary>
  /// 타이틀 아이콘으로 표시될 Icon 타입 지정 (예: Barcode, Setting 등)
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }
  /// <summary>
  /// 기본값 IconType.None
  /// </summary>
  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(IconType.None));
  #endregion

  #region IconFill
  /// <summary>
  /// Icon (주 아이콘) 의 색상 지정용 Brush, 기본값 회색(#AAAAAA)
  /// </summary>
  public Brush IconFill
  {
    get { return (Brush)GetValue(IconFillProperty); }
    set { SetValue(IconFillProperty, value); }
  }
  /// <summary>
  /// 기본값 #AAAAAA 회색
  /// </summary>
  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"))));
  #endregion

  #region IconSize
  /// <summary>
  /// 이이콘 사이즈 너비,높이
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  /// <summary>
  /// 아이콘사이즈 기본값
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register(nameof(IconSize), typeof(double),
          typeof(HeimdallrTreeViewItem), new PropertyMetadata(null));
  #endregion

  #region Text
  /// <summary>
  /// TreeViewItem에 표시할 텍스트 (타이틀 문자열)
  /// </summary>
  public string Text
  {
    get => (string)GetValue(TextProperty);
    set => SetValue(TextProperty, value);
  }
  /// <summary>
  /// 기본값 string.Empty
  /// </summary>
  public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register(nameof(Text), typeof(string), typeof(HeimdallrTreeViewItem),
      new PropertyMetadata(string.Empty));
  #endregion

  #region SelectedCommand
  /// <summary>
  /// TreeViewItem이 선택되었을 때 실행할 ICommand 속성 (MVVM 명령 바인딩용)
  /// </summary>
  public ICommand SelectedCommand
  {
    get => (ICommand)GetValue(SelectedCommandProperty);
    set => SetValue(SelectedCommandProperty, value);
  }
  /// <summary>
  /// 기본값 null
  /// </summary>
  public static readonly DependencyProperty SelectedCommandProperty =
      DependencyProperty.Register(nameof(SelectedCommand), typeof(ICommand), typeof(HeimdallrTreeViewItem),
        new PropertyMetadata(null));
  #endregion

  #region SelectedBackground
  /// <summary>
  /// 항목이 선택되었을 때 배경 색상
  /// 기본값 #4B70F5 (파란색)
  /// </summary>
  public Brush SelectedBackground
  {
    get => (Brush)GetValue(SelectedBackgroundProperty);
    set => SetValue(SelectedBackgroundProperty, value);
  }
  /// <summary>
  /// 기본값 파란색
  /// </summary>
  public static readonly DependencyProperty SelectedBackgroundProperty =
      DependencyProperty.Register(nameof(SelectedBackground), typeof(Brush), typeof(HeimdallrTreeViewItem),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B70F5"))));
  #endregion

  #region HoverBackground
  /// <summary>
  /// 마우스 오버 시 배경 색상
  /// 기본값 #577B8D (회색톤)
  /// </summary>
  public Brush HoverBackground
  {
    get => (Brush)GetValue(HoverBackgroundProperty);
    set => SetValue(HoverBackgroundProperty, value);
  }
  /// <summary>
  /// 기본값 회색
  /// </summary>
  public static readonly DependencyProperty HoverBackgroundProperty =
      DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(HeimdallrTreeViewItem),
          new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#577B8D"))));
  #endregion

  #region ExpandToggleIconSize
  /// <summary>
  /// 확장/축소 아이콘의 너비 (픽셀 단위), 기본값 16.0
  /// </summary>
  public double ExpandToggleIconSize
  {
    get => (double)GetValue(ExpandToggleIconSizeProperty);
    set => SetValue(ExpandToggleIconSizeProperty, value);
  }
  /// <summary>
  /// 기본값 16
  /// </summary>
  public static readonly DependencyProperty ExpandToggleIconSizeProperty =
      DependencyProperty.Register(nameof(ExpandToggleIconSize), typeof(double), typeof(HeimdallrTreeViewItem),
          new PropertyMetadata(20.0));
  #endregion

  #region IconSpacingWhenTextIsEmpty
  /// <summary>
  /// 텍스트가 없을 때 Icon과 확장/축소 아이콘 사이 간격
  /// 기본값 10.0
  /// </summary>
  public double IconSpacingWhenTextIsEmpty
  {
    get => (double)GetValue(IconSpacingWhenTextIsEmptyProperty);
    set => SetValue(IconSpacingWhenTextIsEmptyProperty, value);
  }
  /// <summary>
  /// 기본값 간격 10
  /// </summary>
  public static readonly DependencyProperty IconSpacingWhenTextIsEmptyProperty =
      DependencyProperty.Register(nameof(IconSpacingWhenTextIsEmpty), typeof(double),
          typeof(HeimdallrTreeViewItem), new PropertyMetadata(10.0));
  #endregion

  #region CommandParameter
  /// <summary>
  /// SelectedCommand에 전달할 매개변수
  /// </summary>
  public object CommandParameter
  {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }
  /// <summary>
  /// 기본값 null
  /// </summary>
  public static readonly DependencyProperty CommandParameterProperty =
      DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(HeimdallrTreeViewItem),
          new PropertyMetadata(null));

  #endregion

  #region 종속성 생성자
  /// <summary>
  /// 기본 스타일 연결 (Themes/Generic.xaml에 정의된 스타일 적용)
  /// </summary>
  static HeimdallrTreeViewItem()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrTreeViewItem),
      new FrameworkPropertyMetadata(typeof(HeimdallrTreeViewItem)));
  }
  #endregion

  #region GetContainerForItemOverride 메서드 재정의
  /// <summary>
  /// 하위 아이템 컨테이너도 HeimdallrTreeViewItem으로 생성하여
  /// 트리뷰 전체가 일관된 사용자 정의 아이템 타입을 사용하게 함
  /// </summary>
  /// <returns>새로운 HeimdallrTreeViewItem 인스턴스</returns>
  protected override DependencyObject GetContainerForItemOverride()
  {
    return new HeimdallrTreeViewItem();
  }
  #endregion

  #region OnSelected 메서드 재정의
  /// <summary>
  /// TreeViewItem 선택 시 호출되는 이벤트 핸들러 오버라이드
  /// 기본 선택 동작 수행 후, SelectedCommand가 바인딩 되어 있으면 실행함
  /// </summary>
  /// <param name="e">이벤트 인자</param>
  protected override void OnSelected(RoutedEventArgs e)
  {
    base.OnSelected(e);

    // 1. 이미 선택된 경우 중복 실행 방지
    if (!IsSelected)
      return;

    // 2. CommandParameter 우선, 없으면 DataContext 사용
    var parameter = CommandParameter ?? DataContext;

    // 3. Command 실행 전 조건 검사 및 로그 출력
    if (SelectedCommand != null)
    {
      if (SelectedCommand.CanExecute(parameter))
      {
        Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] Command 실행됨 -> Parameter: {parameter}");
        SelectedCommand.Execute(parameter);
      }
      else
      {
        Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] CanExecute 반환 false -> Parameter: {parameter}");
      }
    }
    else
    {
      Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] SelectedCommand -> null입니다");
    }
  }
  #endregion

  #region OnUnselected 메서드 재정의
  /// <summary>
  /// 선택 해제 시 호출되는 이벤트 핸들러 오버라이드
  /// </summary>
  /// <param name="e"></param>
  protected override void OnUnselected(RoutedEventArgs e)
  {
    base.OnUnselected(e);

    Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] Item 선택 해제ㄷ -> {Text}");
  }
  #endregion

  #region OnKeyDown 메서드 재정의 
  /// <summary>
  /// 키보드 이벤트 처리 (Enter 키로 SelectedCommand 실행)
  /// </summary>
  /// <param name="e"></param>
  protected override void OnKeyDown(KeyEventArgs e)
  {
    base.OnKeyDown(e);

    if (e.Key == Key.Enter)
    {
      var parameter = CommandParameter ?? DataContext;

      if (SelectedCommand?.CanExecute(parameter) == true)
      {
        Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] (Enter Key) Command 실행됨 -> Parameter: {parameter}");
        SelectedCommand.Execute(parameter);
        e.Handled = true;
      }
      else
      {
        Debug.WriteLine($"[{nameof(HeimdallrTreeViewItem)}.{MethodBase.GetCurrentMethod()?.Name}] (Enter Key) Command 실행 불가 -> Parameter: {parameter}");
      }
    }
  }
  #endregion
}
/*
 #region OnApplyTemplate
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // 부모 아이템에서 IconSize 가져오기 -> FindParent 확장메서드
    var parentItem = this.FindParent<HeimdallrTreeViewItem>();
    if (parentItem != null)
    {
      // 부모 아이템의 IconSize 기반으로 자식 아이템의 IconSize를 계산
      double parentIconSize = parentItem.IconSize;

      // 최소 IconSize 30 설정 (20보다 작을 경우 30으로 설정)
      this.IconSize = Math.Max(parentIconSize * 0.8, 30);
    }

    // 자식 아이템들을 순회하면서 IconSize 설정
    SetIconSizeForAllItems();
  }
  #endregion

  #region SetIconSizeForAllItems
  private void SetIconSizeForAllItems()
  {
    // 시각적 트리를 통해 모든 자식 아이템을 순회
    foreach (var item in this.Items)
    {
      if (item is HeimdallrTreeViewItem childItem)
      {
        // 부모 아이템의 IconSize를 기반으로 자식 아이템의 IconSize를 설정
        childItem.IconSize = this.IconSize * 0.8;

        // 자식 아이템이 또 다른 하위 아이템들을 가지면 재귀적으로 적용
        if (childItem.HasItems)
        {
          childItem.SetIconSizeForAllItems();
        }
      }
    }
  }
  #endregion
 */