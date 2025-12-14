using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;

namespace Heimdallr.UI.Controls;

/// <summary>
/// YAML 형식의 이미지 데이터를 JSON으로 변환하고 ImageItem 컬렉션으로 변환하는 정적 클래스입니다.
/// 이미지 데이터를 런타임 중 한 번만 로딩하며, 키 기반으로 빠르게 접근할 수 있게 딕셔너리 형태로 제공합니다.
/// </summary>
public static class ImageContainer
{
  // 전체 Image 데이터 모델
  private static ImageRoot? _data;

  // 이름을 키로 하는 ImageItem 항목 저장소
  private static Dictionary<string, ImageItem>? _items;

  /// <summary>
  /// 읽기 전용으로 외부에 노출되는 이미지 항목 딕셔너리입니다.
  /// </summary>
  public static IReadOnlyDictionary<string, ImageItem>? Items => _items;

  // 정적 생성자에서 자동 빌드 실행
  static ImageContainer()
  {
    Build();
  }

  /// <summary>
  /// 내장 리소스에서 YAML 파일을 읽고 JSON으로 변환한 후, Image 데이터를 메모리에 로드
  /// </summary>
  private static void Build()
  {
    try
    {
      // 현재 어셈블리 참조
      Assembly assembly = Assembly.GetExecutingAssembly();

      string resourceName = "Heimdallr.UI.Controls.Assets.Images.images.yaml";

      Debug.WriteLine($"[{nameof(ImageContainer)}.{MethodBase.GetCurrentMethod()?.Name}] Resource Loading...: {resourceName}");

      using Stream? stream = assembly.GetManifestResourceStream(resourceName);

      if (stream == null)
      {
        throw new FileNotFoundException($"Resources를 찾을 수 없습니다: {resourceName}");
      }

      using StreamReader reader = new StreamReader(stream);

      string yamlText = reader.ReadToEnd();

      // YAML -> JSON으로 변환
      var deserializer = new Deserializer();

      object yamlObject = deserializer.Deserialize<object>(new StringReader(yamlText));

      StringWriter jsonWriter = new StringWriter();

      new JsonSerializer().Serialize(jsonWriter, yamlObject);

      string jsonText = jsonWriter.ToString();

      // JSON -> ImageRoot 디시리얼라이즈
      _data = JsonConvert.DeserializeObject<ImageRoot>(jsonText);

      _items = new Dictionary<string, ImageItem>();

      if (_data?.Items != null)
      {
        foreach (var item in _data.Items)
        {
          if (!string.IsNullOrEmpty(item?.Name))
          {
            _items[item.Name] = item;
          }
        }
      }

      Debug.WriteLine($"[{nameof(ImageContainer)}.{MethodBase.GetCurrentMethod()?.Name}] {_items.Count} 개의 Image Item Load");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[{nameof(ImageContainer)}.{MethodBase.GetCurrentMethod()?.Name}] Error : {ex.Message}");

      throw;
    }
  }
}

