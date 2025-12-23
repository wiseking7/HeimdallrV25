using System.Globalization;

namespace Heimdallr.UI.Converters;

public class IconSizeConverter : BaseValueConverter<IconSizeConverter>
{
  // 부모 아이콘 크기에 따라 자식 아이콘 크기를 비례적으로 설정
  public double DecreaseBy { get; set; } = 5; // 자식 아이콘이 부모보다 작을 비율
  public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is double parentIconSize)
    {
      // 부모 아이콘 크기에서 `DecreaseBy`만큼 빼서 자식 아이콘 크기를 결정
      return parentIconSize - DecreaseBy;
    }
    return value;
  }

  // ConvertBack은 사용하지 않음
  public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    => throw new NotImplementedException();
}
