using Heimdallr.App.Themes.UI;
using Heimdallr.UI.App;
using System.Windows;

namespace Heimdallr.App;

public class App : HeimdallrApplication
{
  protected override Window CreateShell()
  {
    return new PrismMainView();
  }
}
