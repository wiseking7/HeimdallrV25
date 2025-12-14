namespace Heimdallr.UI.Helpers;

/// <summary>
/// WPF 또는 .NET 애플리케이션에서 테마 및 로케일(언어/문화 설정) 관련 리소스를 초기화하기 위한 추상 클래스.
/// ResourceManager와 바로 호환되도록 설계됨.
/// </summary>
public abstract class BaseResourceInitializer
{
  /// <summary>
  /// 테마 리소스 파일의 기본 경로
  /// 예: "Resources/Themes"
  /// </summary>
  public string ThemePath { get; protected set; }

  /// <summary>
  /// 애플리케이션의 기본 로케일 (언어-국가 코드)
  /// 예: "en-US", "ko-KR"
  /// </summary>
  public string DefaultLocale { get; protected set; }

  /// <summary>
  /// 기본 테마 이름
  /// 예: "Light", "Dark"
  /// </summary>
  public string DefaultThemeName { get; protected set; }

  /// <summary>
  /// 로케일 리소스 파일의 기본 경로
  /// 예: "Resources/Locales"
  /// </summary>
  public string LocalePath { get; protected set; }

  /// <summary>
  /// 생성자에서 속성을 초기화하고, Initialize() 호출
  /// </summary>
  protected BaseResourceInitializer()
  {
    ThemePath = FetchThemePath();
    DefaultThemeName = DetermineDefaultThemeName();
    LocalePath = FetchLocalePath();
    DefaultLocale = DetermineDefaultLocale();

    // 추가 초기화 로직
    Initialize();
  }

  #region 추상 메서드 (자식 클래스에서 반드시 구현)

  /// <summary>
  /// 테마 리소스 경로를 반환
  /// </summary>
  protected abstract string FetchThemePath();

  /// <summary>
  /// 기본 테마 이름 결정
  /// </summary>
  protected abstract string DetermineDefaultThemeName();

  /// <summary>
  /// 로케일 리소스 경로 반환
  /// </summary>
  protected abstract string FetchLocalePath();

  /// <summary>
  /// 기본 로케일 결정
  /// </summary>
  protected abstract string DetermineDefaultLocale();

  /// <summary>
  /// 추가 초기화 로직 (테마/로케일 적용, 커스텀 설정 등)
  /// </summary>
  public abstract void Initialize();

  #endregion
}

