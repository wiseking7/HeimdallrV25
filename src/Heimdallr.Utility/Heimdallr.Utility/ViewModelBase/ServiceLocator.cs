using Prism.Ioc;

namespace Heimdallr.Utility;

// DI 관리 - 의존성 주입을 처리하는 클래스
public class ServiceLocator
{
  private readonly IContainerProvider _containerProvider;

  public ServiceLocator(IContainerProvider containerProvider)
  {
    _containerProvider = containerProvider;
  }

  // DI 컨테이너에서 T 타입의 서비스 인스턴스를 반환
  public T Resolve<T>() => _containerProvider.Resolve<T>();
}

