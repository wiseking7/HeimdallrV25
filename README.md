# Heimdallr.WPF 예제

이 문서는 Heimdallr.WPF 컨트롤을 사용한 XAML 예제입니다.  
아래 코드는 그대로 복사하여 WPF 프로젝트에서 사용 가능합니다.

---

## 1. ListView 컨텍스트 메뉴 및 기본 컨트롤 예제

```xml
<!--#region ListView 컨텍스트 메뉴(비밀번호 변경, 복원, 삭제) -->
<ctrls:HeimdallrContextMenu x:Key="ContextMenu" DataContext="{Binding PlacementTarget.DataContext, RelativeSource={RelativeSource Self}}"
                            BackgroundColor="AliceBlue">
  <!--#region 비밀번호 변경 컨텍스트 메뉴 -->
  <ctrls:HeimdallrMenuItem
  Margin="10,5"
  Command="{Binding MessageCommand}"
  CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"
  IconFill="#FFFDCFFA"
  FontSize="16"
  Header="비밀번호변경"
  IconSize="15"
  Icon="Exchange" />
  <!--#endregion-->

  <!--#region 복원 컨텍스트 메뉴 -->
  <ctrls:HeimdallrMenuItem
  Margin="10,5"
  Command="{Binding MessageCommand}"
  CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"
  IconFill="#FF739EC9"
  FontSize="16"
  Header="복원"
  IconSize="15"
  Icon="AccountA" 
  />
  <!--#endregion -->

  <!--#region 삭제 -->
  <ctrls:HeimdallrMenuItem
  Margin="10,5"
  Command="{Binding MessageCommand}"
  CommandParameter="{Binding PlacementTarget.SelectedItem, RelativeSource={RelativeSource AncestorType=ContextMenu}}"
  FontSize="16"
  Header="삭제"
  IconSize="15"
  Icon="Delete" 
   />
  <!--#endregion-->
</ctrls:HeimdallrContextMenu>
<!--#endregion-->

<Style TargetType="{x:Type ui:PrismMainView}"
       BasedOn="{StaticResource {x:Type ctrls:BaseThemeWindow}}">
  <Setter Property="Background" Value="{DynamicResource WindowBackgroundColor}" />
  <Setter Property="TitleHeaderBackground" Value="{DynamicResource TitleHeaderBackground}" />
  <Setter Property="TitleContent">
    <Setter.Value>
      <StackPanel Orientation="Horizontal">
        <!-- FlatButton, DimmingButton, ThemeButton 등 다양한 컨트롤 배치 -->
        ...
      </StackPanel>
    </Setter.Value>
  </Setter>
  
  <Setter Property="Content">
    <Setter.Value>
      <ctrls:HeimdallrScrollViewer  VerticalScrollBarVisibility="Auto" HorizontalScrollBarVisibility="Auto">
        <Grid>
          <!-- Grid.Row, Grid.Column, 다양한 Heimdallr 컨트롤 배치 -->
          ...
        </Grid>
      </ctrls:HeimdallrScrollViewer>
    </Setter.Value>
  </Setter>
</Style>
