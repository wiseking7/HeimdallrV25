using Prism.Ioc;
using System.Reflection;

namespace Heimdallr.Utility.Extensions;

/// <summary>
/// 
/// </summary>
public static class ContainerRegistryExtensions
{
  /// <summary>
  /// Prism 의 IContainerRegistry 명히적 확장메서드
  /// </summary>
  /// <param name="containerRegistry"></param>
  /// <param name="views"></param>
  public static void RegisterViewsForNavigationExplicitly(this IContainerRegistry containerRegistry,
      params (Type view, Type viewModel)[] views)
  {
    foreach (var (view, viewModel) in views)
    {
      // 문자열로 이름을 자동 생성하거나 명시해야 함
      var viewName = view.Name;

      // 이건 Prism에 정의된 유효한 메서드
      containerRegistry.RegisterForNavigation(view, viewName);
    }
  }

  /// <summary>
  /// Prism 의 IContainerRegistry 자동등록 확장메서드
  /// </summary>
  /// <param name="containerRegistry"></param>
  /// <param name="views"></param>
  public static void RegisterViewsForNavigationAutomatically(this IContainerRegistry containerRegistry)
  {
    var viewModelTypes = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.Name.EndsWith("ViewModel")); // 뷰모델 이름 필터링

    foreach (var viewModelType in viewModelTypes)
    {
      var viewType = Assembly.GetExecutingAssembly().GetTypes()
          .FirstOrDefault(t => t.Name == viewModelType.Name.Replace("ViewModel", "View"));

      if (viewType != null)
      {
        containerRegistry.RegisterForNavigation(viewType, viewType.Name);
      }
    }
  }
}

/*
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
  // 자동 등록: 모든 ViewModel과 View가 자동으로 등록됨
  containerRegistry.RegisterViewsForNavigationAutomatically();

  // Register Views and ViewModels with a single call
  containerRegistry.RegisterViewsForNavigation(
      (typeof(MainView), typeof(MainViewModel)),
      (typeof(AnotherView), typeof(AnotherViewModel))
  );
}
 */