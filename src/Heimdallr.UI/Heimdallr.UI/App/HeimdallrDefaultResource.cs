using Heimdallr.UI.Helpers;
using System.Diagnostics;
using System.Windows;

namespace Heimdallr.UI.App;

/// 실제 Default 테마 초기화 클래스
public class HeimdallrDefaultResource : BaseResourceInitializer
{
  /// <summary>
  /// 재정의 추상화 메서드
  /// </summary>
  public override void Initialize()
  {
    try
    {
      // ThemePath 끝의 '/' 제거
      string themePath = ThemePath.TrimEnd('/');
      string themeXaml = $"{themePath}/{DefaultThemeName}"; // Themes/Generic.xaml

      // 테마 XAML 파일 등록 (Generic.xaml)
      Uri themeUri = new Uri($"pack://application:,,,/Heimdallr.UI;component/{themeXaml}", UriKind.Absolute);
      ResourceDictionary themeDict = new ResourceDictionary { Source = themeUri };
      Application.Current.Resources.MergedDictionaries.Add(themeDict);

      Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}] Themes({DefaultThemeName}) Load 완료");

      // Color.xaml 파일 등록
      string colorXaml = "Styles/Colors/Color.xaml";  // color.xaml 경로
      Uri colorUri = new Uri($"pack://application:,,,/Heimdallr.UI;component/{colorXaml}", UriKind.Absolute);
      ResourceDictionary colorDict = new ResourceDictionary { Source = colorUri };
      Application.Current.Resources.MergedDictionaries.Add(colorDict);

      Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}] Color.xaml Load 완료");

      // LocalePath 처리
      if (!string.IsNullOrEmpty(LocalePath) && !string.IsNullOrEmpty(DefaultLocale))
      {
        string localePath = LocalePath.TrimEnd('/');
        string localeXaml = $"{localePath}/{DefaultLocale}.xaml"; // Themes/locales/en-us.xaml
        Uri localeUri = new Uri($"pack://application:,,,/Heimdallr.UI;component/{localeXaml}", UriKind.Absolute);
        ResourceDictionary localeDict = new ResourceDictionary { Source = localeUri };
        Application.Current.Resources.MergedDictionaries.Add(localeDict);

        Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}] Locale({DefaultLocale}) Load 완료");
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}] 리소스 로드 실패: {ex.Message}");
      throw;
    }
  }

  #region BaseResourceInitializer 추상 메서드 구현
  protected override string DetermineDefaultLocale()
  {
    // 기본 로케일 파일 이름
    // 로케일 XAML 이름 (확장자 제외 가능)
    return string.Empty; // 나중에 실제 경로로 교체
  }

  protected override string DetermineDefaultThemeName()
  {
    // 로그할 기본 테마 파일 이름
    // 기본 테마 이름 지정 (테마이름)
    return "Generic.xaml";
  }

  protected override string FetchLocalePath()
  {
    // 로케일(yamlm, xaml, Json 등) 리소스가 들어 있는 폴더 경로를 반환합니다
    // 임시 경로 또는 빈 문자열 (로케일경로)
    return string.Empty; // 나중에 실제 경로로 교체
  }

  protected override string FetchThemePath()
  {
    // 테마 리소스가 들어 있는 폴더 경로를 반환합니다
    // 프로젝트 내 테마 리소스 폴더, 끝에 '/' 포함
    return "Themes/";
  }
  #endregion
}
