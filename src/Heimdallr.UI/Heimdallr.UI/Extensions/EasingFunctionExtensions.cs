using Heimdallr.UI.Animations;
using System.Windows.Media.Animation;

namespace Heimdallr.UI.Extensions;

public static class EasingFunctionExtensions
{
  public static IEasingFunction? GetEasingFunction(this EasingFunctionBaseMode mode)
  {
    return mode switch
    {
      EasingFunctionBaseMode.CubicEaseOut => new CubicEase { EasingMode = EasingMode.EaseOut },
      EasingFunctionBaseMode.CubicEaseInOut => new CubicEase { EasingMode = EasingMode.EaseInOut },
      EasingFunctionBaseMode.QuinticEaseInOut => new QuinticEase { EasingMode = EasingMode.EaseInOut },
      _ => null
    };
  }
}
