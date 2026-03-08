using Heimdallr.UI.Enums;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrMessageBox : Window
{
  #region 생성자
  // 종속성 생성자
  static HeimdallrMessageBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrMessageBox), new FrameworkPropertyMetadata(typeof(HeimdallrMessageBox)));
  }

  // 런타임 생성자
  public HeimdallrMessageBox(string? message, string? caption, HeimdallrMessageBoxButtonEnum buttons = HeimdallrMessageBoxButtonEnum.OK,
                             IconType icon = IconType.None, Brush? iconFill = null)
  {
    Title = caption;
    Message = message;
    MessageIcon = icon;
    Buttons = buttons;

    if (iconFill != null)
      IconFill = iconFill;  // ← 여기!

    AllowsTransparency = true;
    WindowStyle = WindowStyle.None;

    CloseCommand = new DelegateCommand(() =>
    {
      Result = MessageBoxResult.Cancel;

      if (IsLoaded)
      {
        DialogResult = false;
      }
      else
      {
        Close();
      }
    });

    PreviewKeyDown += OnPreviewKeyDown;
  }
  #endregion

  #region Dependency Properties Message
  public string? Message
  {
    get => (string?)GetValue(MessageProperty);
    set => SetValue(MessageProperty, value);
  }

  public static readonly DependencyProperty MessageProperty =
      DependencyProperty.Register(nameof(Message), typeof(string), typeof(HeimdallrMessageBox), new PropertyMetadata(string.Empty));
  #endregion

  #region Icon
  public IconType MessageIcon
  {
    get => (IconType)GetValue(MessageIconProperty);
    set => SetValue(MessageIconProperty, value);
  }

  public static readonly DependencyProperty MessageIconProperty =
      DependencyProperty.Register(nameof(MessageIcon), typeof(IconType), typeof(HeimdallrMessageBox), new PropertyMetadata(IconType.None));
  #endregion

  #region IconFill
  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }

  public static readonly DependencyProperty IconFillProperty =
    DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrMessageBox),
        new PropertyMetadata(null));
  #endregion

  #region IconSize
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HeimdallrMessageBox),
    new PropertyMetadata(64.0));
  #endregion

  #region IconMouseOverFill
  public Brush IconMouseOverFill
  {
    get => (Brush)GetValue(IconMouseOverFillProperty);
    set => SetValue(IconMouseOverFillProperty, value);
  }

  public static readonly DependencyProperty IconMouseOverFillProperty =
     DependencyProperty.Register(nameof(IconMouseOverFill), typeof(Brush), typeof(HeimdallrMessageBox),
         new PropertyMetadata(null));
  #endregion

  #region Buttons
  public HeimdallrMessageBoxButtonEnum Buttons
  {
    get => (HeimdallrMessageBoxButtonEnum)GetValue(ButtonsProperty);
    set => SetValue(ButtonsProperty, value);
  }
  public static readonly DependencyProperty ButtonsProperty =
      DependencyProperty.Register(nameof(Buttons), typeof(HeimdallrMessageBoxButtonEnum), typeof(HeimdallrMessageBox),
        new PropertyMetadata(HeimdallrMessageBoxButtonEnum.OK, OnButtonsChanged));

  private static void OnButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is HeimdallrMessageBox box && box._buttonPanel != null)
    {
      box.CreateButtons((HeimdallrMessageBoxButtonEnum)e.NewValue);
    }
  }
  #endregion

  #region HeaderBackground
  public Brush HeaderBackground
  {
    get => (Brush)GetValue(HeaderBackgroundProperty);
    set => SetValue(HeaderBackgroundProperty, value);
  }

  public static readonly DependencyProperty HeaderBackgroundProperty =
    DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(HeimdallrMessageBox),
        new PropertyMetadata(null));
  #endregion

  public Brush HeaderForeground
  {
    get => (Brush)GetValue(HeaderForegroundProperty);
    set => SetValue(HeaderForegroundProperty, value);
  }

  public static readonly DependencyProperty HeaderForegroundProperty =
    DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(HeimdallrMessageBox),
        new PropertyMetadata(Brushes.White));


  #region ButtonHoverBackground(마우스 오버시)
  public Brush ButtonHoverBackground
  {
    get => (Brush)GetValue(ButtonHoverBackgroundProperty);
    set => SetValue(ButtonHoverBackgroundProperty, value);
  }

  public static readonly DependencyProperty ButtonHoverBackgroundProperty =
    DependencyProperty.Register(nameof(ButtonHoverBackground), typeof(Brush), typeof(HeimdallrMessageBox),
        new PropertyMetadata(null));
  #endregion

  #region ButtonHoverBackground(마우스 클릭시)
  public Brush ButtonPressedBackground
  {
    get => (Brush)GetValue(ButtonPressedBackgroundProperty);
    set => SetValue(ButtonPressedBackgroundProperty, value);
  }

  public static readonly DependencyProperty ButtonPressedBackgroundProperty =
      DependencyProperty.Register(nameof(ButtonPressedBackground), typeof(Brush), typeof(HeimdallrMessageBox),
          new PropertyMetadata(null));
  #endregion

  #region Content 배경색 
  public Brush ContentBackground
  {
    get => (Brush)GetValue(ContentBackgroundProperty);
    set => SetValue(ContentBackgroundProperty, value);
  }

  public static readonly DependencyProperty ContentBackgroundProperty = DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(HeimdallrMessageBox),
    new PropertyMetadata(Brushes.Transparent));
  #endregion

  #region 
  public string YesText
  {
    get => (string)GetValue(YesTextProperty);
    set => SetValue(YesTextProperty, value);
  }
  public static readonly DependencyProperty YesTextProperty =
      DependencyProperty.Register(nameof(YesText), typeof(string), typeof(HeimdallrMessageBox),
          new PropertyMetadata(string.Empty));

  public string NoText
  {
    get => (string)GetValue(NoTextProperty);
    set => SetValue(NoTextProperty, value);
  }
  public static readonly DependencyProperty NoTextProperty =
      DependencyProperty.Register(nameof(NoText), typeof(string), typeof(HeimdallrMessageBox),
          new PropertyMetadata(string.Empty));

  public string CancelText
  {
    get => (string)GetValue(CancelTextProperty);
    set => SetValue(CancelTextProperty, value);
  }
  public static readonly DependencyProperty CancelTextProperty =
      DependencyProperty.Register(nameof(CancelText), typeof(string), typeof(HeimdallrMessageBox),
          new PropertyMetadata(string.Empty));
  #endregion

  #region 내부 필드
  private UniformGrid? _buttonPanel;
  private bool _buttonClicked = false;   // 버튼 중복 클릭 방지
  public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;
  public ICommand CloseCommand { get; }
  #endregion

  #region ESC 이벤트
  private void OnPreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
    {
      Result = MessageBoxResult.Cancel;
      DialogResult = false;
    }
    else if (e.Key == Key.Enter)
    {
      switch (Buttons)
      {
        case HeimdallrMessageBoxButtonEnum.OK:
        case HeimdallrMessageBoxButtonEnum.OKCancel:
          Result = MessageBoxResult.OK;
          DialogResult = true;
          break;

        case HeimdallrMessageBoxButtonEnum.YesNo:
        case HeimdallrMessageBoxButtonEnum.YesNoCancel:
          Result = MessageBoxResult.Yes;
          DialogResult = true;
          break;
      }
    }
  }
  #endregion

  #region OnApplyTemplate 연결
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    _buttonPanel = GetTemplateChild("PART_ButtonPanel") as UniformGrid;

    if (_buttonPanel == null)
    {
      Debug.WriteLine($"[{nameof(HeimdallrMessageBox)}.OnApplyTemplate] -> PART_ButtonPanel 템플릿 요소를 찾을 수 없습니다.");
      // 필요하면 여기서 return; 으로 버튼 생성 생략
      return;
    }

    // DragBar 이벤트 안전 연결
    try
    {
      var bar = GetTemplateChild("PART_DragBar") as DraggableBar;
      if (bar != null)
      {
        bar.MouseDown -= WindowDragMove;
        bar.MouseDown += WindowDragMove;
      }
      else
      {
        // 디버깅 용 메시지 출력
        Debug.WriteLine($"[{nameof(HeimdallrMessageBox)}.OnApplyTemplate] -> PART_DragBar 템플릿 요소를 찾을 수 없습니다");
      }
    }
    catch (Exception ex)
    {
      // 오류 로깅 (실제 배포 시 로그 파일 기록 가능)
      Debug.WriteLine($"[{nameof(HeimdallrMessageBox)}.OnApplyTemplate] -> DragBar 이벤트 연결 실패: {ex.Message}");
    }

    CreateButtons(Buttons);
  }
  #endregion

  #region WindowDragMove 연결
  private void WindowDragMove(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed)
    {
      DragMove();
    }
  }
  #endregion

  #region CreateButtons 버튼 생성 로직
  private void CreateButtons(HeimdallrMessageBoxButtonEnum buttons)
  {
    if (_buttonPanel == null)
    {
      return; // 안전하게 종료 (CS8602 방지)
    }

    _buttonPanel.Children.Clear();

    void AddButton(string text, MessageBoxResult result, bool isDefault = false, bool isCancel = false, int tabIndex = 0)
    {
      var btn = new HeimdallrFlatButton
      {
        Content = text,

        Width = 80,
        Height = 34,

        UseLayoutRounding = true,
        SnapsToDevicePixels = true,

        Margin = new Thickness(6),

        BorderThickness = new Thickness(0),

        Cursor = Cursors.Hand,
        Foreground = (Brush?)Application.Current.TryFindResource("MessageBoxButtonTextBrush") ?? Brushes.White,

        CornerRadius = new CornerRadius(6),

        MouseOverBackground = this.ButtonHoverBackground,
        PressedBackground = this.ButtonPressedBackground,

        VerticalContentAlignment = VerticalAlignment.Center, // 텍스트 중앙 정렬
        HorizontalContentAlignment = HorizontalAlignment.Center, // 가로 중앙 정렬

        Padding = new Thickness(12, 0, 12, 0),   // 내부 여백 제거

        IsCancel = isCancel,    // ← Esc 키 동작
        TabIndex = tabIndex
      };

      btn.Click += (_, __) =>
      {
        if (_buttonClicked) return; // 이미 클릭됨 

        _buttonClicked = true;      // 한 번만 실행

        Result = result;    // 클릭 결과 저장

        DialogResult = result switch
        {
          MessageBoxResult.OK => true,
          MessageBoxResult.Yes => true,
          MessageBoxResult.No => false,
          _ => null
        };

        // 버튼 비활성화 추가 안전 장치
        foreach (var child in _buttonPanel.Children.OfType<Button>())
          child.IsEnabled = false;
      };

      _buttonPanel.Children.Add(btn);
    }

    switch (buttons)
    {
      case HeimdallrMessageBoxButtonEnum.OK:
        AddButton((string)Application.Current.FindResource("MsgBox_OK"),
                  MessageBoxResult.OK, isDefault: true, tabIndex: 0);
        break;

      case HeimdallrMessageBoxButtonEnum.OKCancel:
        AddButton((string)Application.Current.FindResource("MsgBox_OK"),
                  MessageBoxResult.OK, isDefault: true, tabIndex: 0);
        AddButton((string)Application.Current.FindResource("MsgBox_Cancel"),
                  MessageBoxResult.Cancel, isCancel: true, tabIndex: 1);
        break;

      case HeimdallrMessageBoxButtonEnum.YesNo:
        AddButton((string)Application.Current.FindResource("MsgBox_Yes"),
                  MessageBoxResult.Yes, isDefault: true, tabIndex: 0);
        AddButton((string)Application.Current.FindResource("MsgBox_No"),
                  MessageBoxResult.No, tabIndex: 1);
        break;

      case HeimdallrMessageBoxButtonEnum.YesNoCancel:
        AddButton((string)Application.Current.FindResource("MsgBox_Yes"),
                  MessageBoxResult.Yes, isDefault: true, tabIndex: 0);
        AddButton((string)Application.Current.FindResource("MsgBox_No"),
                  MessageBoxResult.No, tabIndex: 1);
        AddButton((string)Application.Current.FindResource("MsgBox_Cancel"),
                  MessageBoxResult.Cancel, isCancel: true, tabIndex: 2);
        break;
    }

    // UniformGrid 열 수 설정 → 버튼 균등 배치
    _buttonPanel.Columns = _buttonPanel.Children.Count;
  }
  #endregion

  #region 정적 호출 함수 (커스텀 enum 적용)
  public static MessageBoxResult Show(
      string message,
      string caption = "Message",
      HeimdallrMessageBoxButtonEnum buttons = HeimdallrMessageBoxButtonEnum.OK,
      IconType icon = IconType.None,
      Brush? iconFill = null)
  {
    var dlg = new HeimdallrMessageBox(message, caption, buttons, icon, iconFill);

    dlg.Owner = Application.Current.Windows
        .OfType<Window>()
        .FirstOrDefault(w => w.IsActive) ?? Application.Current.MainWindow;

    dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

    dlg.ShowDialog();

    return dlg.Result;
  }
  #endregion
}








