using System.ComponentModel;

namespace Heimdallr.Domain.Enums;

public enum EmployeePosition
{
  /// <summary>
  /// 직급 없음 / 권한 없음 / 아르바이트/ 기타
  /// </summary>
  [Description("없음")]
  None = 0,

  /// <summary>
  /// 일반 팀원 - 실무 담당자
  /// </summary>
  [Description("팀원")]
  TeamMember = 1,

  /// <summary>
  /// 팀장 - 팀 단위 업무 관리 및 보고
  /// </summary>
  [Description("팀장")]
  TeamLead = 2,

  /// <summary>
  /// 부서장 - 부서 단위 관리 및 의사결정
  /// </summary>
  [Description("부서장")]
  DepartmentHead = 3,

  /// <summary>
  /// 임원 - 회사 주요 전략/결정 참여
  /// </summary>
  [Description("임원")]
  Executive = 4
}
