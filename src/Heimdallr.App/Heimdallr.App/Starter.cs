using Heimdallr.App;
using Heimdallr.App.Themes.UI;
using Heimdallr.App.ViewModel;
using Heimdallr.UI.Extensions;
using Heimdallr.UI.MVVM;

namespace HeimdallrApp;

public class Starter
{
  [STAThread]
  public static void Main(string[] args)
  {
    _ = new App()
        .AddWireDataContext<WireDataContent>()
        .AddInversionModule<HelperModules>()
        //.AddInversionModule<ViewModules>()
        .Run();
  }
}

public class WireDataContent : ViewModelLocationScenario
{
  protected override void Match(ViewModelLocatorCollection items)
  {
    // View 와 ViewModel(등록) 이 100개 이상 있을시 명확하게 관리하게 위해서
    items.Register<PrismMainView, PrismMainViewModel>(); //매칭

  }
}

public class HelperModules : IModule
{
  public void OnInitialized(IContainerProvider containerProvider)
  {
    throw new NotImplementedException();
  }

  public void RegisterTypes(IContainerRegistry containerRegistry)
  {
    containerRegistry.RegisterViewsForNavigationAutomatically();

    // 또는, TestDialogView를 IDialogService에 직접 등록하려면
    containerRegistry.RegisterDialog<TestDialogView, TestDialogViewModel>();
  }
}