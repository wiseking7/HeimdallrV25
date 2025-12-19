namespace Heimdallr.UI.Attributes;

/// <summary>
/// 클래스에만 적용
/// 상속할수 없음
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DimmingAttribute : Attribute
{
  /// <summary>
  /// 디밍 효과를 사용할지 여부를 나타냅니다.
  /// true이면 디밍 효과가 활성화되고, false이면 사용되지 않습니다.
  /// </summary>
  public bool Dimming { get; private set; }

  /// <summary>
  /// 기본 생성자. 디밍 효과를 기본값으로 사용(true)하도록 설정합니다.
  /// 이 생성자를 사용하는 경우 [UseDimming] 과 같이 매개변수 없이 선언하면 UseDimming = true로 처리됩니다.
  /// </summary>
  public DimmingAttribute()
  {
    Dimming = true;
  }

  /// <summary>
  /// 디밍 사용 여부를 지정할 수 있는 생성자입니다.
  /// 예: [UseDimming(false)] 처럼 디밍 효과를 사용하지 않도록 지정할 수 있습니다.
  /// </summary>
  /// <param name="useDimming">디밍 효과를 사용할지 여부</param>
  public DimmingAttribute(bool useDimming)
  {
    Dimming = useDimming;
  }
}
