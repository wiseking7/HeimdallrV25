using Heimdallr.App.Themes.UI;
using Heimdallr.App.ViewModel;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace Heimdallr.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
  protected override Window CreateShell()
  {
    return Container.Resolve<PrismMainView>();
  }

  protected override void RegisterTypes(IContainerRegistry containerRegistry)
  {
    containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
    containerRegistry.RegisterSingleton<IRegionManager, RegionManager>();

    containerRegistry.RegisterForNavigation<PrismMainView, PrismMainViewModel>();
  }
}
