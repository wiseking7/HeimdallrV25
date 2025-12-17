using Heimdallr.App.Resources;
using Heimdallr.UI.Controls;
using System.Windows;
using System.Windows.Media;

namespace Heimdallr.App.Themes.UI;

public class PrismMainView : BaseThemeWindow
{
  static PrismMainView()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(PrismMainView), new FrameworkPropertyMetadata(typeof(PrismMainView)));
  }

  private readonly Lazy<ResourceManager> _resourceManager;

  public PrismMainView()
  {
    _resourceManager = new Lazy<ResourceManager>(() => new ResourceManager());

    // 초기 색상 설정
    _resourceManager.Value.SetWindowBackground((Color)ColorConverter.ConvertFromString("#FF2E323A"));
    _resourceManager.Value.SetTitleBarBackground((Color)ColorConverter.ConvertFromString("#FF2E323A"));

    // 컨텍스트 메뉴 예시
    //var menu = new HeimdallrContextMenu();
    //menu.Items.Add(CreateColorMenuItem("Red", Colors.Red));
    //menu.Items.Add(CreateColorMenuItem("Green", Colors.Green));
    //menu.Items.Add(CreateColorMenuItem("Blue", Colors.Blue));
    //this.ContextMenu = menu;
  }

  private HeimdallrMenuItem CreateColorMenuItem(string header, Color color)
  {
    var item = new HeimdallrMenuItem { Header = header };
    item.Click += (s, e) =>
    {
      _resourceManager.Value.SetWindowBackground(color);
      _resourceManager.Value.SetTitleBarBackground(color);
    };
    return item;
  }
}
