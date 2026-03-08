namespace Heimdallr.App.Enums;

/// <summary>
/// Value -> SelectedValuePath="Value" 으로 바인딩, Description -> DisplayMemberPath="Description" 바인딩
/// </summary>
public class HeimdallrComboBoxItem
{
  public int Value { get; set; }                            // SelectedValue 로 바인딩 실제 바인딩 값
  public string? Description { get; set; } = string.Empty;  // UI 에 표시될 값 Dsecription 열거형
}

