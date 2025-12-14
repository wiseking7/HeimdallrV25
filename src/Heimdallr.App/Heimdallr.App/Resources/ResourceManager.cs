using Heimdallr.UI.Helpers;
using System.Windows;
using System.Windows.Media;

namespace Heimdallr.App.Resources;

public class ResourceManager : IResourceManager
{
  public void SetWindowBackground(Color color)
  {
    // DynamicResource Key를 사용
    Application.Current.Resources["WindowBackgroundColor"] = new SolidColorBrush(color);
  }

  public void SetTitleBarBackground(Color color)
  {
    Application.Current.Resources["TitleBarBackgroundColor"] = new SolidColorBrush(color);
  }
}
