using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace Heimdallr.UI.Controls;

/// <summary>
/// GeometryItem 개체들을 관리하는 정적 클래스입니다.
/// JSON 파일을 로드하고, GeometryItem을 Dictionary에 저장하며,
/// 필요할 때만 Geometry(PathGeometry)를 생성하고 캐싱하여 메모리 사용을 최소화합니다.
/// </summary>
public static class GeometryContainer
{
  // JSON에서 읽은 GeometryItem들을 저장하는 딕셔너리
  private static Dictionary<string, GeometryItem>? _items;

  // PathGeometry 캐시: 한 번 생성한 Geometry는 재사용
  private static readonly Dictionary<string, Geometry> _geometryCache = new();

  /// <summary>
  /// 지정된 이름의 Geometry(PathGeometry) 객체를 반환합니다.
  /// 한 번 생성된 Geometry는 캐시에서 재사용합니다.
  /// </summary>
  /// <param name="name">GeometryItem 이름</param>
  /// <returns>PathGeometry 객체</returns>
  public static Geometry GetGeometry(string name)
  {
    EnsureItemsLoaded();

    if (!_items!.ContainsKey(name))
      throw new KeyNotFoundException($"Geometry Item '{name}'을(를) 찾을 수 없습니다.");

    // 캐시 확인
    if (_geometryCache.TryGetValue(name, out var cached))
      return cached;

    // PathGeometry 생성 후 캐시 저장
    var geo = Geometry.Parse(_items[name].Data!);
    _geometryCache[name] = geo;
    return geo;
  }

  /// <summary>
  /// JSON 데이터를 로드하고 _items를 초기화합니다.
  /// 최초 접근 시 한 번만 실행 (Lazy Loading)
  /// </summary>
  private static void EnsureItemsLoaded()
  {
    if (_items != null) return;

    string jsonData = LoadJson();

    var geometryRoot = JsonConvert.DeserializeObject<GeometryRoot>(jsonData);
    _items = new Dictionary<string, GeometryItem>();

    if (geometryRoot?.Items != null)
    {
      foreach (var item in geometryRoot.Items)
      {
        if (!string.IsNullOrWhiteSpace(item.Name))
          _items[item.Name] = item;
      }
    }
  }

  /// <summary>
  /// Embedded Resource로 포함된 geometries.json 파일을 읽어 반환합니다.
  /// </summary>
  /// <returns>JSON 문자열</returns>
  private static string LoadJson()
  {
    Assembly assembly = Assembly.GetExecutingAssembly();
    string resourceName = "Heimdallr.UI.Controls.Assets.Geomerties.geometries.json";

    using Stream? stream = assembly.GetManifestResourceStream(resourceName)
        ?? throw new FileNotFoundException($"리소스 파일을 찾을 수 없습니다: {resourceName}");

    using StreamReader reader = new(stream);
    return reader.ReadToEnd();
  }

  /// <summary>
  /// 외부에서 GeometryItem 딕셔너리에 접근할 수 있도록 제공합니다.
  /// Items 자체는 Lazy Loading이 완료된 후 접근 가능
  /// </summary>
  public static IReadOnlyDictionary<string, GeometryItem> Items
  {
    get
    {
      EnsureItemsLoaded();
      return _items!;
    }
  }
}

