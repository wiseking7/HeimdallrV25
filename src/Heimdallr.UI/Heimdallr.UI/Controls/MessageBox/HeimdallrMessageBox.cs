using Heimdallr.UI.Enums;
using Heimdallr.UI.Extensions;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public class HeimdallrMessageBox : Window
{
  #region 종속성 생성자
  static HeimdallrMessageBox()
  {
    DefaultStyleKeyProperty.OverrideMetadata(
        typeof(HeimdallrMessageBox),
        new FrameworkPropertyMetadata(typeof(HeimdallrMessageBox)));
  }
  #endregion

  #region Dependency Properties

  public string? Message
  {
    get => (string)GetValue(MessageProperty);
    set => SetValue(MessageProperty, value);
  }

  public static readonly DependencyProperty MessageProperty =
      DependencyProperty.Register(nameof(Message), typeof(string), typeof(HeimdallrMessageBox));


  public new IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }

  public new static readonly DependencyProperty IconProperty =
      DependencyProperty.Register(nameof(Icon), typeof(IconType), typeof(HeimdallrMessageBox));


  public Brush IconFill
  {
    get => (Brush)GetValue(IconFillProperty);
    set => SetValue(IconFillProperty, value);
  }

  public static readonly DependencyProperty IconFillProperty =
      DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(HeimdallrMessageBox),
          new PropertyMetadata(Brushes.DarkGray));


  public HeimdallrMessageBoxButtonEnum Buttons
  {
    get => (HeimdallrMessageBoxButtonEnum)GetValue(ButtonsProperty);
    set => SetValue(ButtonsProperty, value);
  }

  public static readonly DependencyProperty ButtonsProperty =
      DependencyProperty.Register(nameof(Buttons), typeof(HeimdallrMessageBoxButtonEnum), typeof(HeimdallrMessageBox));

  #endregion

  #region 내부 필드

  private StackPanel? _buttonPanel;

  public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

  public ICommand CloseCommand { get; }

  #endregion



  #region 생성자
  public HeimdallrMessageBox(
      string? message,
      string? caption,
      HeimdallrMessageBoxButtonEnum buttons = HeimdallrMessageBoxButtonEnum.OK,
      IconType icon = IconType.None,
      Brush? iconFill = null)
  {
    Title = caption;
    Message = message;
    Icon = icon;
    Buttons = buttons;

    if (iconFill != null)
      IconFill = iconFill;  // ← 여기!

    SizeToContent = SizeToContent.WidthAndHeight;
    AllowsTransparency = true;
    WindowStyle = WindowStyle.None;
    WindowStartupLocation = WindowStartupLocation.CenterScreen;

    CloseCommand = new DelegateCommand(() =>
    {
      Result = MessageBoxResult.Cancel;
      Close();
    });
  }
  #endregion



  #region Template 연결

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    // PART_ButtonPanel을 VisualTreeHelperExtensions를 사용해서 찾기
    _buttonPanel = this.FindChild<StackPanel>(sp => sp.Name == "PART_ButtonPanel")
        ?? throw new Exception("Template -> PART_ButtonPanel 을 찾을 수 없습니다.");

    var bar = this.FindChild<DraggableBar>(b => b.Name == "PART_DragBar");
    if (bar != null)
    {
      bar.MouseDown += WindowDragMove;
    }

    CreateButtons(Buttons);
  }

  private void WindowDragMove(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed)
    {
      GetWindow(this).DragMove();
    }
  }

  #endregion

  #region 버튼 생성 로직
  private void CreateButtons(HeimdallrMessageBoxButtonEnum buttons)
  {
    if (_buttonPanel == null)
    {
      return; // 안전하게 종료 (CS8602 방지)
    }

    _buttonPanel.Children.Clear();

    void AddButton(string text, MessageBoxResult result)
    {
      // BrushConverter 변환은 null 가능 → 검증 필요
      var hover = new BrushConverter().ConvertFromString("#FF9F73AB") as Brush;
      var pressed = new BrushConverter().ConvertFromString("#FFC060A1") as Brush;

      var btn = new HeimdallrFlatButton
      {
        Content = text,
        MinWidth = 100,
        MinHeight = 34,
        Margin = new Thickness(6),
        Cursor = Cursors.Hand,
        CornerRadius = new CornerRadius(6),
        MouseOverBackground = hover ?? Brushes.LightGray,
        PressedBackground = pressed ?? Brushes.Gray,
      };

      btn.Click += (_, __) =>
      {
        Result = result;
        Close();
      };

      _buttonPanel.Children.Add(btn);
    }

    switch (buttons)
    {
      case HeimdallrMessageBoxButtonEnum.OK:
        AddButton("OK", MessageBoxResult.OK);
        break;

      case HeimdallrMessageBoxButtonEnum.OKCancel:
        AddButton("OK", MessageBoxResult.OK);
        AddButton("Cancel", MessageBoxResult.Cancel);
        break;

      case HeimdallrMessageBoxButtonEnum.YesNo:
        AddButton("Yes", MessageBoxResult.Yes);
        AddButton("No", MessageBoxResult.No);
        break;

      case HeimdallrMessageBoxButtonEnum.YesNoCancel:
        AddButton("Yes", MessageBoxResult.Yes);
        AddButton("No", MessageBoxResult.No);
        AddButton("Cancel", MessageBoxResult.Cancel);
        break;
    }
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
    var dlg = new HeimdallrMessageBox(message, caption, buttons, icon, iconFill)
    {
      Owner = Application.Current.MainWindow
    };

    dlg.ShowDialog();
    return dlg.Result;
  }
  #endregion
}








