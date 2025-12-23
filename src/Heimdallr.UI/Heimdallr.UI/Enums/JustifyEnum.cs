using System.ComponentModel;

namespace Heimdallr.UI.Enums;

/// <summary>
/// UI 요소의 정렬 방식을 정의하기 위한 열거형(enum)
/// 수평 레이아웃 배치에서 자식 요소들 사이의 간격을 어떻게 분배할지를 설정할 때 사용됩니다. 
/// 이 값들은 Flexbox, DockPanel, WrapPanel, StackPanel 같은 레이아웃 시스템에서 
/// 자주 사용되는 정렬 개념을 반영하고 있습니다.
/// </summary>
public enum JustifyEnum
{
  [Description("없음")]
  None,

  [Description("자식 요소들 간의 간격을 고르게 분배(첫 번째와 마지막 요소는 컨테이너 끝과 붙음)")]
  SpaceAround,

  [Description("자식 요소들 간의 간격만 균등하게 배치(각 요소 주변에 같은 간격)")]
  SpaceBetween,

  [Description("모든 간격이 동일(첫/마지막 요소와 컨테이너 끝 사이도 동일)")]
  SpaceEvenly
}
