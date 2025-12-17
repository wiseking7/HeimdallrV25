using System.Diagnostics;
using System.Reflection;

namespace Heimdallr.Utility;

// 검증 및 에러 관리 - 동기 및 비동기 검증 규칙을 관리하는 클래스
public class ValidationService
{
  private readonly Dictionary<string, List<Func<IEnumerable<string>>>> _syncRules = new Dictionary<string, List<Func<IEnumerable<string>>>>();
  private readonly Dictionary<string, List<Func<Task<IEnumerable<string>>>>> _asyncRules = new Dictionary<string, List<Func<Task<IEnumerable<string>>>>>();

  // 동기 검증 규칙 추가
  public void AddValidationRule(string propertyName, Func<IEnumerable<string>> rule)
  {
    if (!_syncRules.TryGetValue(propertyName, out var value))
    {
      value = new List<Func<IEnumerable<string>>>();
      _syncRules[propertyName] = value;
    }
    value.Add(rule);
  }

  // 비동기 검증 규칙 추가
  public void AddValidationRuleAsync(string propertyName, Func<Task<IEnumerable<string>>> asyncRule)
  {
    if (!_asyncRules.TryGetValue(propertyName, out var value))
    {
      value = new List<Func<Task<IEnumerable<string>>>>();
      _asyncRules[propertyName] = value;
    }
    value.Add(asyncRule);
  }

  // 특정 속성에 대한 검증 수행 (동기 및 비동기 규칙 실행)
  public async Task<List<string>> ValidatePropertyAsync(string propertyName)
  {
    List<string> errors = new List<string>();

    // 동기 검증 실행
    if (_syncRules.TryGetValue(propertyName, out var syncRules))
    {
      foreach (var rule in syncRules)
      {
        try
        {
          errors.AddRange(rule());
        }
        catch (Exception ex)
        {
          // 예외 로깅 또는 사용자 알림 처리
          Debug.WriteLine($"[{nameof(ValidationService)}.{MethodBase.GetCurrentMethod()?.Name}] {propertyName} 대한 동기화 검증 중 오류 발생: {ex.Message}");

          // 예외를 로깅하는 로직 추가 가능
        }
      }
    }

    // 비동기 검증 실행
    if (_asyncRules.TryGetValue(propertyName, out var asyncRules))
    {
      foreach (var asyncRule in asyncRules)
      {
        try
        {
          errors.AddRange(await asyncRule());
        }
        catch (Exception ex)
        {
          // 예외 처리
          Debug.WriteLine($"[{nameof(ValidationService)}.{MethodBase.GetCurrentMethod()?.Name}] {propertyName} 대한 비동기화 검증 중 오류 발생: {ex.Message}");
        }
      }
    }

    return errors;
  }
}

