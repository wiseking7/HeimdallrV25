namespace Tests.ViewModeTests;

public sealed class TestSynchronizationContext : SynchronizationContext
{
  public int ThreadId { get; private set; }

  public override void Post(SendOrPostCallback d, object? state)
  {
    ThreadId = Environment.CurrentManagedThreadId;
    d(state);
  }
}
