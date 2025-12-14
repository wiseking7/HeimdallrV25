using YamlDotNet.Serialization;

namespace Heimdallr.UI.Models;

/// <summary>
/// 테마색상 키
/// </summary>
public class ThemePack
{
  /// <summary>
  /// 테마 Key
  /// </summary>
  [YamlMember(Alias = "key")]
  public string? Key { get; set; }

  /// <summary>
  /// 테마 색상
  /// </summary>
  [YamlMember(Alias = "colors")]
  public SolidColorBrushUnit? Colors { get; set; }
}
