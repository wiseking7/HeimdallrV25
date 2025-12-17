using Prism.Regions;

namespace Heimdallr.Utility;

// 내비게이션 관리 - View 간 내비게이션을 처리하는 클래스
public class NavigationService
{
  private readonly IRegionManager _regionManager;

  public NavigationService(IRegionManager regionManager)
  {
    _regionManager = regionManager;
  }

  // 특정 region에 view를 내비게이션
  public void Navigate(string regionName, string viewName)
  {
    _regionManager.RequestNavigate(regionName, viewName);
  }
}

