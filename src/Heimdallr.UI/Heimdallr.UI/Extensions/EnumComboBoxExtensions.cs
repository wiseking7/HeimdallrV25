using Heimdallr.UI.Helpers;
using System.Collections.ObjectModel;

namespace Heimdallr.UI.Extensions;

public class HeimdallrComboBoxItem
{
  public int Value { get; set; }           // 실제 바인딩 값
  public string? Description { get; set; } // UI 에 표시될 값 Dsecription 열거형
}

/// <summary>
/// 열거형 Enum을 HeimdallrComboBox에서 사용하기 위해
/// Description과 Value를 매핑하고, 
/// Placeholder 처리(-1)까지 포함한 초기화 확장 메서드 제공
/// </summary>
public static class EnumComboBoxExtensions
{
  /// <summary>
  /// 제네릭 Enum 기반으로 ComboBox용 컬렉션과 선택값을 초기화합니다.
  /// <para>
  /// - Enum의 Description과 Value를 HeimdallrComboBox에 바인딩할 수 있는
  ///   ObservableCollection&lt;HeimdallrComboBoxItem&gt;으로 변환
  /// </para>
  /// <para>
  /// - Placeholder 처리: Enum 값 0 (없음)을 제거하고 SelectedValue에 -1 할당
  ///   → ComboBox에서 PlaceholderText가 정상 표시됨
  /// </para>
  /// <para>
  /// - DB 저장 시에는 SelectedValue를 nullable int로 변환하여 null 처리 가능
  /// </para>
  /// </summary>
  /// <typeparam name="TEnum">Enum 타입</typeparam>
  /// <param name="comboBoxProperty">
  /// ObservableCollection&lt;HeimdallrComboBoxItem&gt; 프로퍼티 setter
  /// (ViewModel에서 ItemsSource 바인딩용)
  /// </param>
  /// <param name="selectedValueProperty">
  /// ComboBox SelectedValue(int) 프로퍼티 setter
  /// -1일 경우 Placeholder 표시용
  /// </param>
  /// <param name="defaultValue">
  /// Enum 기본 선택값
  /// null인 경우 SelectedValue는 -1로 초기화되어 Placeholder로 사용
  /// </param>
  public static void InitializeEnumComboBox<TEnum>(
      Action<ObservableCollection<HeimdallrComboBoxItem>> comboBoxProperty,
      Action<int> selectedValueProperty,
      TEnum? defaultValue = null) where TEnum : struct, Enum
  {
    // Enum 목록 생성: Key=Value, Value=Description
    var items = EnumHelper.GetEnumDescriptionCollection<TEnum>()
                          .Select(kv => new HeimdallrComboBoxItem
                          {
                            Value = kv.Key,
                            Description = kv.Value
                          })
                          .ToList();

    // Placeholder 처리: Enum 값 0 제거, SelectedValue는 -1 사용
    var placeholder = items.FirstOrDefault(x => x.Value == 0);
    if (placeholder != null)
      items.Remove(placeholder);

    // ComboBox ItemsSource 초기화
    comboBoxProperty(new ObservableCollection<HeimdallrComboBoxItem>(items));

    // SelectedValue 초기화: 기본값이 없으면 -1로 지정 (Placeholder)
    int selected = defaultValue != null ? Convert.ToInt32(defaultValue) : -1;
    selectedValueProperty(selected);
  }

  /// <summary>
  /// DB 저장용 변환 확장 메서드
  /// <para>-1 (Placeholder용 값)을 null로 변환</para>
  /// <para>DB 저장 시 nullable int 프로퍼티에 매핑 가능</para>
  /// </summary>
  /// <param name="selectedValue">ComboBox SelectedValue(int)</param>
  /// <returns>DB용 nullable int</returns>
  public static int? ToDbValue(this int selectedValue)
  {
    return selectedValue == -1 ? null : selectedValue;
  }
}
