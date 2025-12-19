using Moq;


namespace Tests.ViewModeTests;

public class NavigationViewModelTests : ViewModelTestBase<TestNavigationViewModel>
{
  protected override TestNavigationViewModel CreateViewModel(IContainerProvider container)
      => new TestNavigationViewModel(container);

  [Fact]
  public void OnNavigatedTo_Should_SetFlagAndContext()
  {
    // Arrange
    var vm = CreateViewModel(ContainerMock.Object);
    var context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("Test", UriKind.RelativeOrAbsolute));

    // Act
    vm.OnNavigatedTo(context);

    // Assert
    Assert.True(vm.OnNavigatedToCalled);
    Assert.Equal(context, vm.LastNavigationContext);
  }

  [Fact]
  public void OnNavigatedFrom_Should_SetFlagAndContext()
  {
    // Arrange
    var vm = CreateViewModel(ContainerMock.Object);
    var context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("Test", UriKind.RelativeOrAbsolute));

    // Act
    vm.OnNavigatedFrom(context);

    // Assert
    Assert.True(vm.OnNavigatedFromCalled);
    Assert.Equal(context, vm.LastNavigationContext);
  }

  [Fact]
  public void IsNavigationTarget_Should_AlwaysReturnTrue()
  {
    // Arrange
    var vm = CreateViewModel(ContainerMock.Object);
    var context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("Test", UriKind.RelativeOrAbsolute));

    // Act
    var result = vm.IsNavigationTarget(context);

    // Assert
    Assert.True(result);
  }
}

