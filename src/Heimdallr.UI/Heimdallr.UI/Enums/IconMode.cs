using System.ComponentModel;

namespace Heimdallr.UI.Enums;

/// <summary>
/// IconMode 열거형 HeimdallrIcon 사용
/// </summary>
public enum IconMode
{
  /// <summary>없음</summary>
  [Description("없음")]
  None,

  /// <summary>아이콘</summary>
  [Description("Icon")]
  Icon,

  /// <summary>이미지</summary>
  [Description("Image")]
  Image,
}
