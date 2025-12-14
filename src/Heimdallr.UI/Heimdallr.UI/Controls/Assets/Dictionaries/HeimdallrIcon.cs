using Heimdallr.UI.Enums;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heimdallr.UI.Controls;

/// <summary>
///  Icon, Image 를 사용한 커스터마이징
/// </summary>
public class HeimdallrIcon : ContentControl
{
  #region IconMode
  public IconMode Mode
  {
    get => (IconMode)GetValue(ModeProperty);
    set => SetValue(ModeProperty, value);
  }
  /// <summary>
  /// 종속성 주입
  /// </summary>
  public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(IconMode),
        typeof(HeimdallrIcon), new PropertyMetadata(IconMode.None));
  #endregion

  #region Data
  /// <summary>
  /// 현재 표시되는 Geometry 데이터
  /// </summary>
  public Geometry Data
  {
    get => (Geometry)GetValue(DataProperty);
    set => SetValue(DataProperty, value);
  }
  /// <summary>
  /// 종속성 주입
  /// </summary>
  public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(Geometry),
        typeof(HeimdallrIcon), new PropertyMetadata(null));
  #endregion

  #region IconType
  /// <summary>
  /// 정해진 Enum 값으로 아이콘을 지정하는 방식 (예: Save, Delete, Edit 등)
  /// 내부적으로 Toolkit에 정의된 Path Geometry 문자열로 변환하여 표시
  /// </summary>
  public IconType Icon
  {
    get => (IconType)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
  }

  /// <summary>
  /// 종속성 주입
  /// </summary>
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconType),
        typeof(HeimdallrIcon), new PropertyMetadata(IconType.None, IconPropertyChanged));

  /// <summary>
  /// IconType 속성이 변경될 때 호출되는 콜백
  /// </summary>
  /// <param name="d"></param>
  /// <param name="e"></param>
  private static void IconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    HeimdallrIcon heimdallrIcon = (HeimdallrIcon)d;

    // ToolKit에서 아이콘 이름에 해당하는 Path 데이터 문자열 가져오기
    string geometryData = GeometryConverter.GetData(heimdallrIcon.Icon.ToString());

    if (!string.IsNullOrEmpty(geometryData))
    {
      // // 문자열을 Geometry로 파싱하여 Data 속성에 할당
      heimdallrIcon.Data = Geometry.Parse(geometryData);
      heimdallrIcon.Mode = IconMode.Icon;
    }
    else
    {
      heimdallrIcon.Data = Geometry.Parse("M0,0"); // 빈 PathFallback

      // 현재 모드를 Icon으로 설정
      heimdallrIcon.Mode = IconMode.None;
    }
  }
  #endregion

  #region Image
  /// <summary>
  /// ImageType Enum 값으로 지정된 아이콘 이미지를 표시 (Base64 인코딩된 이미지 사용)
  /// </summary>
  public ImageType Image
  {
    get => (ImageType)GetValue(ImageProperty);
    set => SetValue(ImageProperty, value);
  }

  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageType),
        typeof(HeimdallrIcon), new PropertyMetadata(ImageType.None, ImagePropertyChanged));

  /// <summary>
  /// Image 속성이 변경될 때 호출되는 콜백
  /// </summary>
  /// <param name="d"></param>
  /// <param name="e"></param>
  private static void ImagePropertyChanged(DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    HeimdallrIcon heimdallrIcon = (HeimdallrIcon)d;

    try
    {
      // 이미지 이름에 해당하는 Base64 문자열 가져오기
      string base64 = ImageConverter.GetData(heimdallrIcon.Image.ToString());

      // Base64 → byte[]
      byte[] binaryData = Convert.FromBase64String(base64);

      // byte[] → BitmapImage 생성
      BitmapImage bitmapImage = new();
      bitmapImage.BeginInit();
      bitmapImage.StreamSource = new MemoryStream(binaryData);
      bitmapImage.EndInit();

      // Source 속성에 할당
      heimdallrIcon.Source = bitmapImage;
      heimdallrIcon.Mode = IconMode.Image;
    }
    catch (KeyNotFoundException ex)
    {
      Debug.WriteLine($"[{nameof(HeimdallrIcon)}.{MethodBase.GetCurrentMethod()?.Name}] Image -> Item 을 찾을수 없습니다 : {ex.Message}");
      heimdallrIcon.Source = null!;
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[{nameof(HeimdallrIcon)}.{MethodBase.GetCurrentMethod()?.Name}] Image -> Loading 중 오류 발생 : {ex.Message}");
    }
  }
  #endregion

  #region Fill
  /// <summary>
  /// Path 아이콘에 색상을 적용할 때 사용되는 속성 (브러시)
  /// </summary>
  public Brush Fill
  {
    get => (Brush)GetValue(FillProperty);
    set => SetValue(FillProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush),
        typeof(HeimdallrIcon), new PropertyMetadata(Brushes.Silver));
  #endregion

  #region IconSize
  /// <summary>
  /// Icon, Image 사이즈 너비,높이
  /// </summary>
  public double IconSize
  {
    get => (double)GetValue(IconSizeProperty);
    set => SetValue(IconSizeProperty, value);
  }

  /// <summary>
  /// 아이콘사이즈 기본값
  /// </summary>
  public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double),
          typeof(HeimdallrIcon), new PropertyMetadata(25.0));
  #endregion

  #region Source
  /// <summary>
  /// 현재 표시되는 이미지 데이터 (BitmapImage 등)
  /// </summary>
  public ImageSource Source
  {
    get => (ImageSource)GetValue(SourceProperty);
    set => SetValue(SourceProperty, value);
  }
  /// <summary>
  /// 종속성주입
  /// </summary>
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource),
        typeof(HeimdallrIcon), new PropertyMetadata(null));
  #endregion

  #region Command
  /// <summary>
  /// Command 종속성 속성
  /// </summary>
  public ICommand? Command
  {
    get => (ICommand?)GetValue(CommandProperty);
    set => SetValue(CommandProperty, value);
  }

  /// <summary>
  /// Command 속성은 ICommand 인터페이스를 구현하는 명령을 나타냅니다.
  /// </summary>
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command),
    typeof(ICommand), typeof(HeimdallrIcon), new PropertyMetadata(null));
  #endregion

  #region CommandParameter
  /// <summary>
  /// CommandParameter 속성은 명령에 전달할 추가 매개변수를 나타냅니다.
  /// </summary>
  public object? CommandParameter
  {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }

  /// <summary>
  /// CommandParameter 속성은 ICommand에 전달할 매개변수를 나타냅니다.
  /// </summary>
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter),
    typeof(object), typeof(HeimdallrIcon), new PropertyMetadata(null));
  #endregion

  #region 생성자
  // 종속성 생성자
  static HeimdallrIcon()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(HeimdallrIcon), new FrameworkPropertyMetadata(typeof(HeimdallrIcon)));
  }

  #region Stretch
  /// <summary>
  /// Viewbox 또는 Path 렌더링에 사용할 Stretch 모드
  /// </summary>
  public Stretch Stretch
  {
    get => (Stretch)GetValue(StretchProperty);
    set => SetValue(StretchProperty, value);
  }

  /// <summary>
  /// 기본값 Uniform으로 설정된 Stretch 속성
  /// </summary>
  public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch),
    typeof(Stretch), typeof(HeimdallrIcon), new PropertyMetadata(Stretch.Uniform));
  #endregion

  #region 재정의 메서드
  /// <summary>
  /// OnApplyTemplate: 컨트롤 템플릿이 적용될 때 호출되는 메서드
  /// </summary>
  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();

    if (Data != null)
    {
      Debug.WriteLine($"[{nameof(HeimdallrIcon)}.{MethodBase.GetCurrentMethod()?.Name}] Data -> 속성(Property) 설정 입니다");
    }
    else
    {
      Debug.WriteLine($"[{nameof(HeimdallrIcon)}.{MethodBase.GetCurrentMethod()?.Name}] Data -> null 입니다");
    }
  }
  #endregion


  /// <summary>
  /// HeimdallrIcon 생성자: 기본 생성자
  /// </summary>
  public HeimdallrIcon()
  {
    MouseLeftButtonUp += (sender, e) =>
    {
      // 마우스 왼쪽 버튼 클릭 시 Command 실행
      if (Command != null && Command.CanExecute(CommandParameter))
      {
        Command.Execute(CommandParameter);
        e.Handled = true; // 이벤트 처리 완료 표시
      }
    };

    Cursor = Cursors.Hand; // 마우스 커서를 손 모양으로 변경
  }

  #endregion
}

