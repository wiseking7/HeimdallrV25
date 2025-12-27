using Heimdallr.UI.Attributes;


namespace Heimdallr.App.ViewModel;

[Dimming]
public class TestDialogViewModel : BaseDialogWithDimming
{
  public TestDialogViewModel(IContainerProvider containerProvider) : base(containerProvider)
  {
    Title = "TestDialogView";
  }

  private bool _isOpen;
  public bool IsOpen { get => _isOpen; set => SetProperty(ref _isOpen, value); }


  public DelegateCommand? NavigateCommand { get; }
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
