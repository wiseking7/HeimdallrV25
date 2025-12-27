using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,            //where theme specific resource dictionaries are located
                                                //(used if a resource is not found in the page,
                                                // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly   //where the generic resource dictionary is located
                                                //(used if a resource is not found in the page,
                                                // app, or any theme specific resource dictionaries)
)]

// 하나의 접두사로 묶기
[assembly: XmlnsPrefix("https://Heimdallr.WPF.UI", "ui")]

// 여러 네임스페이스를 하나의 URI로 묶어서 정의
// 전체 프로젝트 -> 프로젝트의 네임스페이스인 Heimdallr.UI를 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI")]

// Animations -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Animations")]

// App -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.App")]

// Controls -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Controls")]

// Converters -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Converters")]

// Enums -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Enums")]

// Extensions -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Extensions")]

// Helpers -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Helpers")]

// Models -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Models")]

// MVVM -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.MVVM")]

// Styles -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Styles")]

// Themes -> 폴더 내 네임스페이스를 해당 URI에 매핑
[assembly: XmlnsDefinition("https://Heimdallr.WPF.UI", "Heimdallr.UI.Themes")]
