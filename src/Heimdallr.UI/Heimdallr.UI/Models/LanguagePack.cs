using YamlDotNet.Serialization;

namespace Heimdallr.UI.Models;

/// <summary>
/// 언어 
/// </summary>
public class LanguagePack
{
  /// <summary>
  /// 언어 Key
  /// </summary>
  [YamlMember(Alias = "key")]
  public string? Key { get; set; }

  /// <summary>
  /// 언어 폰트
  /// </summary>
  [YamlMember(Alias = "items")]
  public LanguageUnit? Fonts { get; set; }
}
