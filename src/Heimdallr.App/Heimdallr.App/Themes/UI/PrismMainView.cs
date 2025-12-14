using Heimdallr.App.Resources;
using Heimdallr.UI.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Heimdallr.App.Themes.UI;

public class PrismMainView : DarkThemeWindow
{
  static PrismMainView()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(PrismMainView), new FrameworkPropertyMetadata(typeof(PrismMainView)));
  }

  private readonly ResourceManager _resourceManager;

  public PrismMainView(ResourceManager resourceManager)
  {
    _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));

    // 초기 색상 설정
    _resourceManager.SetWindowBackground((Color)ColorConverter.ConvertFromString("#FF9F73AB"));
    _resourceManager.SetTitleBarBackground(Colors.DarkSlateGray);

    // 컨텍스트 메뉴 예시
    var menu = new ContextMenu();
    menu.Items.Add(CreateColorMenuItem("Red", Colors.Red));
    menu.Items.Add(CreateColorMenuItem("Green", Colors.Green));
    menu.Items.Add(CreateColorMenuItem("Blue", Colors.Blue));
    this.ContextMenu = menu;
  }

  private MenuItem CreateColorMenuItem(string header, Color color)
  {
    var item = new MenuItem { Header = header };
    item.Click += (s, e) =>
    {
      _resourceManager.SetWindowBackground(color);
      _resourceManager.SetTitleBarBackground(color);
    };
    return item;
  }
}
