using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Heimdallr.UI.Helpers;

public class HeimdallrDefaultResource : BaseResourceInitializer
{
  /// <summary>
  /// 리소스 초기화
  /// </summary>
  public override void Initialize()
  {
    try
    {
      // 테마 리소스 로드
      var themeDict = new ResourceDictionary
      {
        Source = new Uri($"pack://application:,,,/Heimdallr.UI;component/{ThemePath}", UriKind.Absolute)
      };
      Application.Current.Resources.MergedDictionaries.Add(themeDict);

      // 로케일(언어) 리소스 로드
      var localeDict = new ResourceDictionary
      {
        Source = new Uri($"pack://application:,,,/Heimdallr.UI;component/{LocalePath}", UriKind.Absolute)
      };
      Application.Current.Resources.MergedDictionaries.Add(localeDict);

      Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}.{MethodBase.GetCurrentMethod()?.Name}] Themes 및 Locale 로드 완료");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[{nameof(HeimdallrDefaultResource)}.{MethodBase.GetCurrentMethod()?.Name}] 리소스 로드 실패: {ex.Message}");
      throw;
    }
  }

  #region BaseResourceInitializer 구현

  protected override string FetchThemePath()
  {
    // Generic.xaml 경로
    return "Themes/Generic.xaml";
  }

  protected override string DetermineDefaultThemeName()
  {
    return "Default"; // 기본 테마 이름
  }

  protected override string FetchLocalePath()
  {
    // 기본 로케일 리소스 경로
    return "Locales/Default.xaml";
  }

  protected override string DetermineDefaultLocale()
  {
    return "ko-KR"; // 기본 로케일
  }

  #endregion
}

