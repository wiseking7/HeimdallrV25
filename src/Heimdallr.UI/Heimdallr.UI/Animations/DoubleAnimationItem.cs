using System.Windows;
using System.Windows.Media.Animation;

namespace Heimdallr.UI.Animations;

/// <summary>
/// DoubleAnimation을 상속받아 Storyboard에서 직접 사용할 수 있는
/// TargetName, TargetProperty, EasingFunction 설정 기능을 확장한 클래스입니다.
/// </summary>
public class DoubleAnimationItem : DoubleAnimation
{
  #region TargetName Property

  public static readonly DependencyProperty TargetNameProperty =
      DependencyProperty.Register(
          nameof(TargetName),
          typeof(string),
          typeof(DoubleAnimationItem),
          new PropertyMetadata(null, OnTargetNameChanged));

  /// <summary>애니메이션이 적용될 대상 요소의 이름.</summary>
  public string TargetName
  {
    get => (string)GetValue(TargetNameProperty);
    set => SetValue(TargetNameProperty, value);
  }

  private static void OnTargetNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var item = (DoubleAnimationItem)d;
    Storyboard.SetTargetName(item, (string)e.NewValue);
  }

  #endregion


  #region Property Property

  public static readonly DependencyProperty PropertyProperty =
      DependencyProperty.Register(
          nameof(Property),
          typeof(PropertyPath),
          typeof(DoubleAnimationItem),
          new PropertyMetadata(null, OnPropertyChanged));

  /// <summary>애니메이션이 적용될 DependencyProperty 경로.</summary>
  public PropertyPath Property
  {
    get => (PropertyPath)GetValue(PropertyProperty);
    set => SetValue(PropertyProperty, value);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var item = (DoubleAnimationItem)d;
    Storyboard.SetTargetProperty(item, (PropertyPath)e.NewValue);
  }

  #endregion


  #region Mode Property (Easing Function)

  public static readonly DependencyProperty ModeProperty =
      DependencyProperty.Register(
          nameof(Mode),
          typeof(EasingFunctionBaseMode),
          typeof(DoubleAnimationItem),
          new PropertyMetadata(EasingFunctionBaseMode.CubicEaseIn, OnModeChanged));

  /// <summary>애니메이션의 이징(속도 곡선) 함수 설정.</summary>
  public EasingFunctionBaseMode Mode
  {
    get => (EasingFunctionBaseMode)GetValue(ModeProperty);
    set => SetValue(ModeProperty, value);
  }

  private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var item = (DoubleAnimationItem)d;
    var newMode = (EasingFunctionBaseMode)e.NewValue;

    // 기존 easing 함수와 종류가 같으면 Mode만 교체
    if (item.EasingFunction is EasingFunctionBase existing)
    {
      var sameType = existing.GetType() == GetFunctionType(newMode);
      if (sameType)
      {
        existing.EasingMode = GetEasingMode(newMode);
        return;
      }
    }

    // 새로운 easing 함수 생성
    item.EasingFunction = CreateEasingFunction(newMode);
  }

  #endregion


  #region Easing Function Helpers

  /// <summary>Mode 값으로부터 EasingFunction 객체를 생성한다.</summary>
  private static EasingFunctionBase CreateEasingFunction(EasingFunctionBaseMode mode)
  {
    var function = (EasingFunctionBase)Activator.CreateInstance(GetFunctionType(mode))!;
    function.EasingMode = GetEasingMode(mode);
    return function;
  }

  /// <summary>해당 모드에서 EasingMode(EaseIn, EaseOut 등)만 추출.</summary>
  private static EasingMode GetEasingMode(EasingFunctionBaseMode mode)
  {
    string name = mode.ToString();

    if (name.EndsWith("InOut"))
      return EasingMode.EaseInOut;
    if (name.EndsWith("In"))
      return EasingMode.EaseIn;
    return EasingMode.EaseOut;
  }

  /// <summary>EasingFunctionBaseMode → 이징 함수 Type 매핑.</summary>
  private static Type GetFunctionType(EasingFunctionBaseMode mode)
  {
    switch (mode)
    {
      case EasingFunctionBaseMode.BackEaseIn:
      case EasingFunctionBaseMode.BackEaseOut:
      case EasingFunctionBaseMode.BackEaseInOut:
        return typeof(BackEase);

      case EasingFunctionBaseMode.BounceEaseIn:
      case EasingFunctionBaseMode.BounceEaseOut:
      case EasingFunctionBaseMode.BounceEaseInOut:
        return typeof(BounceEase);

      case EasingFunctionBaseMode.CircleEaseIn:
      case EasingFunctionBaseMode.CircleEaseOut:
      case EasingFunctionBaseMode.CircleEaseInOut:
        return typeof(CircleEase);

      case EasingFunctionBaseMode.CubicEaseIn:
      case EasingFunctionBaseMode.CubicEaseOut:
      case EasingFunctionBaseMode.CubicEaseInOut:
        return typeof(CubicEase);

      case EasingFunctionBaseMode.ElasticEaseIn:
      case EasingFunctionBaseMode.ElasticEaseOut:
      case EasingFunctionBaseMode.ElasticEaseInOut:
        return typeof(ElasticEase);

      case EasingFunctionBaseMode.ExponentialEaseIn:
      case EasingFunctionBaseMode.ExponentialEaseOut:
      case EasingFunctionBaseMode.ExponentialEaseInOut:
        return typeof(ExponentialEase);

      case EasingFunctionBaseMode.PowerEaseIn:
      case EasingFunctionBaseMode.PowerEaseOut:
      case EasingFunctionBaseMode.PowerEaseInOut:
        return typeof(PowerEase);

      case EasingFunctionBaseMode.QuadraticEaseIn:
      case EasingFunctionBaseMode.QuadraticEaseOut:
      case EasingFunctionBaseMode.QuadraticEaseInOut:
        return typeof(QuadraticEase);

      case EasingFunctionBaseMode.QuarticEaseIn:
      case EasingFunctionBaseMode.QuarticEaseOut:
      case EasingFunctionBaseMode.QuarticEaseInOut:
        return typeof(QuarticEase);

      case EasingFunctionBaseMode.QuinticEaseIn:
      case EasingFunctionBaseMode.QuinticEaseOut:
      case EasingFunctionBaseMode.QuinticEaseInOut:
        return typeof(QuinticEase);

      case EasingFunctionBaseMode.SineEaseIn:
      case EasingFunctionBaseMode.SineEaseOut:
      case EasingFunctionBaseMode.SineEaseInOut:
        return typeof(SineEase);

      default:
        throw new ArgumentOutOfRangeException(nameof(mode), mode,
            "지원되지 않는 EasingFunctionBaseMode 입니다.");
    }
  }

  #endregion
}


