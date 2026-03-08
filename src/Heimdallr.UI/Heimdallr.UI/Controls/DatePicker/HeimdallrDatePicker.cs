using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrDatePicker : Control
{
  // ===== PART 컨트롤 참조 =====
  private Popup? _popup;
  private HeimdallrCalendarSwitch? _switch;
  private HeimdallrCalendarBox? _listbox;
  private HeimdallrChevronButton? _leftButton;
  private HeimdallrChevronButton? _rightButton;

  #region Dependency Properties

  public bool KeepPopupOpen
  {
    get => (bool)GetValue(KeepPopupOpenProperty);
    set => SetValue(KeepPopupOpenProperty, value);
  }
  public static readonly DependencyProperty KeepPopupOpenProperty =
      DependencyProperty.Register("KeepPopupOpen", typeof(bool), typeof(HeimdallrDatePicker), new PropertyMetadata(true));

  public DateTime CurrentMonth
  {
    get => (DateTime)GetValue(CurrentMonthProperty);
    set => SetValue(CurrentMonthProperty, value);
  }
  public static readonly DependencyProperty CurrentMonthProperty =
      DependencyProperty.Register("CurrentMonth", typeof(DateTime), typeof(HeimdallrDatePicker),
          new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

  public DateTime? SelectedDate
  {
    get => (DateTime?)GetValue(SelectedDateProperty);
    set => SetValue(SelectedDateProperty, value);
  }
  public static readonly DependencyProperty SelectedDateProperty =
      DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(HeimdallrDatePicker),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));

  private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var dp = (HeimdallrDatePicker)d;
    dp.OnSelectedDateChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
  }

  protected virtual void OnSelectedDateChanged(DateTime? oldValue, DateTime? newValue)
  {
    if (_isUpdatingTextBox) return; // 재귀 방지

    if (GetTemplateChild("PART_InputDate") is TextBox tb)
    {
      tb.Text = newValue?.ToString("yyyy-MM-dd") ?? "";
    }

    if (newValue.HasValue)
      GenerateCalendar(newValue.Value);
  }

  public string Watermark
  {
    get => (string)GetValue(WatermarkProperty);
    set => SetValue(WatermarkProperty, value);
  }
  public static readonly DependencyProperty WatermarkProperty =
    DependencyProperty.Register("Watermark", typeof(string), typeof(HeimdallrDatePicker), new PropertyMetadata(string.Empty));

  #region WatermarkForeground 워터마크 텍스트 색상
  public Brush WatermarkForeground
  {
    get => (Brush)GetValue(WatermarkForegroundProperty);
    set => SetValue(WatermarkForegroundProperty, value);
  }
  public static readonly DependencyProperty WatermarkForegroundProperty =
      DependencyProperty.Register(nameof(WatermarkForeground), typeof(Brush), typeof(HeimdallrDatePicker),
          new PropertyMetadata(Brushes.Gray));
  #endregion

  // 루트 Border 배경은 기존 Background 사용
  // 팝업 배경 따로 지정
  public Brush PopupBackground
  {
    get => (Brush)GetValue(PopupBackgroundProperty);
    set => SetValue(PopupBackgroundProperty, value);
  }
  public static readonly DependencyProperty PopupBackgroundProperty = DependencyProperty.Register("PopupBackground", typeof(Brush), typeof(HeimdallrDatePicker),
    new PropertyMetadata(new SolidColorBrush(Color.FromRgb(21, 21, 21))));    // 기본 팝업 색상
  #endregion

  #region PopupAnimation
  public PopupAnimation PopupAnimation
  {
    get => (PopupAnimation)GetValue(PopupAnimationProperty);
    set => SetValue(PopupAnimationProperty, value);
  }

  public static readonly DependencyProperty PopupAnimationProperty =
      DependencyProperty.Register(nameof(PopupAnimation),
          typeof(PopupAnimation),
          typeof(HeimdallrDatePicker),
          new PropertyMetadata(PopupAnimation.Fade));
  #endregion

  #region VerticalOffset
  public double VerticalOffset
  {
    get => (double)GetValue(VerticalOffsetProperty);
    set => SetValue(VerticalOffsetProperty, value);
  }

  public static readonly DependencyProperty VerticalOffsetProperty =
      DependencyProperty.Register(nameof(VerticalOffset),
          typeof(double),
          typeof(HeimdallrDatePicker),
          new PropertyMetadata(0.0));
  #endregion

  #region HorizontalOffset 팝업창 좌/우 이동
  /// <summary>
  /// Popup의 좌우 위치 보정
  /// </summary>
  public double HorizontalOffset
  {
    get => (double)GetValue(HorizontalOffsetProperty);
    set => SetValue(HorizontalOffsetProperty, value);
  }

  public static readonly DependencyProperty HorizontalOffsetProperty =
      DependencyProperty.Register(
          nameof(HorizontalOffset),
          typeof(double),
          typeof(HeimdallrDatePicker),
          new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

  private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrDatePicker menu && menu._popup != null)
    {
      menu._popup.HorizontalOffset = (double)e.NewValue;
    }
  }
  #endregion

  #region 생성자

  static HeimdallrDatePicker()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrDatePicker), new FrameworkPropertyMetadata(typeof(HeimdallrDatePicker)));
  }

  public HeimdallrDatePicker()
  {
    ToolTipOpening += HeimdallrSmartDate_ToolTipOpening;
  }

  private void HeimdallrSmartDate_ToolTipOpening(object sender, ToolTipEventArgs e)
  {
    if (ToolTip == null)
    {
      e.Handled = true;
      return;
    }

    if (ToolTip is HeimdallrToolTip) return;

    if (ToolTip is string text && !string.IsNullOrWhiteSpace(text))
      ToolTip = new HeimdallrToolTip { Content = text };
    else
      e.Handled = true;
  }
  #endregion

  #region OnApplyTemplate
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    _leftButton = GetTemplateChild("PART_Left") as HeimdallrChevronButton;
    _rightButton = GetTemplateChild("PART_Right") as HeimdallrChevronButton;
    _switch = GetTemplateChild("PART_Switch") as HeimdallrCalendarSwitch;
    _popup = GetTemplateChild("PART_Popup") as Popup;
    _listbox = GetTemplateChild("PART_ListBox") as HeimdallrCalendarBox;

    if (_leftButton != null)
      _leftButton.Click += (s, e) => MoveMonth(-1);

    if (_rightButton != null)
      _rightButton.Click += (s, e) => MoveMonth(1);

    if (_switch != null)
      _switch.Click += _switch_Click;

    if (_popup != null)
    {
      _popup.Closed += _popup_Closed;

      // Popup 위치 초기 적용
      _popup.HorizontalOffset = HorizontalOffset;
      _popup.VerticalOffset = VerticalOffset;
    }

    if (_listbox != null)
      _listbox.MouseLeftButtonUp += _listbox_MouseLeftButtonUp;

    if (GetTemplateChild("PART_InputDate") is TextBox inputDate)
      inputDate.TextChanged += PART_InputDate_TextChanged;

    UpdateWatermark();
  }
  #endregion

  #region 월 이동 처리
  private void MoveMonth(int months)
  {
    DateTime newMonth = CurrentMonth.AddMonths(months);

    // 선택 날짜 갱신: 같은 달이면 기존 날짜 유지, 아니면 첫째 날로
    DateTime newSelectedDate;
    if (SelectedDate.HasValue && SelectedDate.Value.Month == CurrentMonth.Month && SelectedDate.Value.Year == CurrentMonth.Year)
    {
      newSelectedDate = new DateTime(newMonth.Year, newMonth.Month, SelectedDate.Value.Day);
      int daysInMonth = DateTime.DaysInMonth(newMonth.Year, newMonth.Month);
      if (newSelectedDate.Day > daysInMonth)
        newSelectedDate = new DateTime(newMonth.Year, newMonth.Month, daysInMonth);
    }
    else
    {
      newSelectedDate = new DateTime(newMonth.Year, newMonth.Month, 1);
    }

    SelectedDate = newSelectedDate; // TextBox와 바인딩 갱신
    GenerateCalendar(newMonth);
  }
  #endregion

  #region TextBox 처리
  private bool _isUpdatingTextBox = false;
  private void PART_InputDate_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (sender is not TextBox inputDate) return;

    _isUpdatingTextBox = true;

    int selStart = inputDate.SelectionStart;
    string raw = inputDate.Text.Replace("-", "").Trim();

    // 입력 자동 포맷
    string formatted = raw;
    if (raw.Length > 4) formatted = formatted.Insert(4, "-");
    if (raw.Length > 6) formatted = formatted.Insert(7, "-");

    int diff = formatted.Length - inputDate.Text.Length;

    if (inputDate.Text != formatted)
    {
      inputDate.Text = formatted;
      inputDate.SelectionStart = Math.Max(0, selStart + diff);
    }

    DateTime? newDate = null;

    // 오직 yyyyMMdd 완전 입력일 때만 SelectedDate 갱신
    if (raw.Length == 8)
    {
      if (DateTime.TryParseExact(raw, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dtFull))
        newDate = dtFull;
    }

    if (newDate.HasValue && SelectedDate != newDate)
      SelectedDate = newDate;

    inputDate.BorderBrush = newDate.HasValue || string.IsNullOrWhiteSpace(inputDate.Text)
        ? Brushes.Transparent
        : Brushes.Red;

    UpdateWatermark();

    _isUpdatingTextBox = false;
  }

  private void UpdateWatermark()
  {
    if (GetTemplateChild("PART_InputDate") is TextBox tb &&
        GetTemplateChild("PART_InputDateWatermark") is TextBlock watermark)
    {
      watermark.Visibility = string.IsNullOrWhiteSpace(tb.Text)
          ? Visibility.Visible
          : Visibility.Collapsed;
    }
  }
  #endregion

  #region 달력 Popup & 선택 이벤트
  private void _switch_Click(object sender, RoutedEventArgs e)
  {
    if (_switch?.IsChecked == true)
    {
      if (_popup != null)
      {
        _popup.IsOpen = true;

        // DP 값 적용
        _popup.HorizontalOffset = HorizontalOffset;
        _popup.VerticalOffset = VerticalOffset;
      }

      GenerateCalendar(SelectedDate ?? DateTime.Now);
    }
  }

  private void _popup_Closed(object? sender, EventArgs e)
  {
    if (_switch != null)
      _switch.IsChecked = IsMouseOver;
  }

  private void _listbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (_listbox?.SelectedItem is HeimdallrCalendarBoxItem selected)
    {
      SelectedDate = selected.Date;
      GenerateCalendar(selected.Date);

      if (_popup != null)
        _popup.IsOpen = KeepPopupOpen;
    }
  }
  #endregion

  #region 달력 생성
  private void GenerateCalendar(DateTime current)
  {
    if (_listbox == null) return;

    CurrentMonth = current;
    _listbox.Items.Clear();

    DateTime fDayOfMonth = new(current.Year, current.Month, 1);
    DateTime lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

    // 월요일 시작 기준으로 첫날 요일 오프셋 계산
    int fOffset = ((int)fDayOfMonth.DayOfWeek + 6) % 7;
    int lOffset = (7 - ((int)lDayOfMonth.DayOfWeek + 6) % 7 - 1);

    DateTime fDay = fDayOfMonth.AddDays(-fOffset);
    DateTime lDay = lDayOfMonth.AddDays(lOffset);

    for (DateTime day = fDay; day <= lDay; day = day.AddDays(1))
    {
      bool isCurrentMonth = day.Month == current.Month;

      var boxItem = new HeimdallrCalendarBoxItem
      {
        Date = day,
        DateFormat = day.ToString("yyyyMMdd"),
        Content = day.Day,
        IsCurrentMonth = isCurrentMonth,
        Foreground = isCurrentMonth
              ? day.DayOfWeek == DayOfWeek.Saturday
                  ? (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9EC6F3"))
                  : day.DayOfWeek == DayOfWeek.Sunday
                      ? (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF08787"))
                      : Brushes.White
              : new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)) // 흐린 흰색
      };

      _listbox.Items.Add(boxItem);
    }
  }
  #endregion
}

