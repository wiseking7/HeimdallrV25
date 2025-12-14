using YamlDotNet.Serialization;

namespace Heimdallr.UI.Models;

/// <summary>
/// 폰트
/// </summary>
public class FontPack
{
  /// <summary>
  /// 폰트 Key
  /// </summary>
  [YamlMember(Alias = "key")]
  public string? Key { get; set; }

  /// <summary>
  /// 폰트
  /// </summary>
  [YamlMember(Alias = "fonts")]
  public FontFamilyUnit? Fonts { get; set; }
}
