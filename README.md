# Heimdallr.WPF

Heimdallr.WPF는 WPF 애플리케이션에서 사용할 수 있는 커스텀 UI 컨트롤 모음입니다.  
NuGet 패키지를 설치하면 다양한 커스텀 버튼, 리스트뷰, 트리뷰, 스위치, ProgressBar 등 컨트롤을 바로 사용 가능합니다.

## 사용 예제
사용 예제
# HeimdallrContextMenu 사용 예제

<ctrls:HeimdallrContextMenu x:Key="ContextMenu"
       DataContext="{Binding PlacementTarget.DataContext, RelativeSource={RelativeSource Self}}"
       BackgroundColor="AliceBlue">
<ctrls:HeimdallrMenuItem Header="비밀번호변경"
       Icon="Exchange"
       Command="{Binding MessageCommand}"
       CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"/>
<ctrls:HeimdallrMenuItem Header="복원"
      Icon="AccountA"
      Command="{Binding MessageCommand}"
      CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"/>
<ctrls:HeimdallrMenuItem Header="삭제"
       Icon="Delete"
       Command="{Binding MessageCommand}"
       CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"/>
</ctrls:HeimdallrContextMenu>
