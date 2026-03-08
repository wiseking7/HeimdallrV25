namespace Heimdallr.UI.Enums;

/// <summary>
/// HeimdallrMessageBox에서 표시할 버튼 조합을 정의합니다.
/// 
/// ⚠ 중요:
/// 버튼 타입은 단순히 버튼 개수가 아니라
/// "사용자의 의사결정 흐름(User Decision Flow)"을 의미합니다.
/// 
/// 올바른 버튼 타입 선택은 UX 일관성과 코드 가독성에 직접적인 영향을 줍니다.
/// </summary>
public enum HeimdallrMessageBoxButtonEnum
{
  None = 0,

  /// <summary> 단순 알림 메시지에 사용합니다.사용자의 선택 분기가 필요 없는 경우에 적합합니다. 예: - 저장이 완료되었습니다. - 오류가 발생했습니다. 
  /// 반환값: MessageBoxResult.OK </summary>
  OK = 1,

  /// <summary> 작업 실행 여부를 확인할 때 사용합니다. "실행 vs 취소" 구조입니다. 예: - 정말 삭제하시겠습니까? - 변경사항을 저장하시겠습니까?
  /// OK     → 작업 수행 Cancel → 작업 취소 </summary>
  OKCancel = 2,

  /// <summary> 질문형 선택 구조입니다. 두 선택지가 동등한 의미를 가질 때 사용합니다. 예: - 파일을 덮어쓰시겠습니까? - 계속 진행하시겠습니까?
  /// Yes → 동의 No  → 동의하지 않음 </summary>
  YesNo = 3,

  /// <summary> 결정 vs 취소 구조입니다. OKCancel과 유사하지만 질문형 의미를 가집니다. 
  /// 예: - 문서 화면으로 이동하시겠습니까? Yes    → 진행 Cancel → 취소 </summary>
  YesCancel = 4,

  /// <summary> 3지 선택 구조입니다. 저장 여부를 묻는 경우에 가장 많이 사용됩니다. 
  /// 예: - 변경 내용을 저장하시겠습니까? Yes    → 저장 후 진행 No     → 저장하지 않고 진행/ Cancel → 작업 자체 취소 </summary>
  YesNoCancel = 5,
}
