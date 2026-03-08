using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

public static class GeometryConverter
{
  /// <summary>
  /// 지정된 이름에 해당하는 PathGeometry를 반환합니다.
  /// 한 번 생성된 Geometry는 캐시에서 재사용됩니다.
  /// </summary>
  /// <param name="name">호출한 멤버 이름 (자동)</param>
  /// <returns>PathGeometry 객체</returns>
  public static Geometry GetGeometry([CallerMemberName] string? name = null)
  {
    if (string.IsNullOrEmpty(name))
      throw new ArgumentNullException(nameof(name), "CallerMemberName 는 null 일 수 없습니다.");

    return GeometryContainer.GetGeometry(name);
  }
}
