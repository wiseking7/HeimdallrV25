using Heimdallr.UI.Attributes;
using Heimdallr.UI.Base;

namespace Heimdallr.App.ViewModel;

[Dimming]
public class TestDialogViewModel : BaseDialogWithDimming
{
  public TestDialogViewModel(IContainerProvider containerProvider) : base(containerProvider)
  {
    Title = "TestDialogView";
  }

  public string Message { get; set; } = "테스트 다이얼로그";


  public override void OnDialogOpened(IDialogParameters parameters)
  {
    base.OnDialogOpened(parameters);

    if (parameters.ContainsKey("Message"))
    {
      Message = parameters.GetValue<string>("Message");
    }
  }

  public override void OnDialogClosed()
  {
    base.OnDialogClosed();

    RequestClose.Invoke(new DialogResult(ButtonResult.OK));
  }
}
