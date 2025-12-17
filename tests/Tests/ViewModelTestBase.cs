using Heimdallr.Utility;
using Moq;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

namespace Tests;

public abstract class ViewModelTestBase<TViewModel>
  where TViewModel : ViewModelBase
{
  protected Mock<IContainerProvider> ContainerMock { get; }
  protected Mock<IEventAggregator> EventAggregatorMock { get; }
  protected Mock<IRegionManager> RegionManagerMock { get; }

  protected TViewModel VM { get; }

  protected ViewModelTestBase()
  {
    ContainerMock = new Mock<IContainerProvider>();
    EventAggregatorMock = new Mock<IEventAggregator>();
    RegionManagerMock = new Mock<IRegionManager>();

    // Prism 필수 Resolve 설정
    ContainerMock
      .Setup(x => x.Resolve(typeof(IEventAggregator)))
      .Returns(EventAggregatorMock.Object);

    ContainerMock
      .Setup(x => x.Resolve(typeof(IRegionManager)))
      .Returns(RegionManagerMock.Object);

    VM = CreateViewModel(ContainerMock.Object);
  }

  protected abstract TViewModel CreateViewModel(IContainerProvider container);
}

