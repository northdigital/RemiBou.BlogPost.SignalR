using RemiBou.BlogPost.SignalR.Shared.Infrastructure;

namespace RemiBou.BlogPost.SignalR.Shared.Notifications
{
  public class CounterIncremented : SerializedNotification
  {
    public int Counter { get; set; }

    public CounterIncremented()
    {
    }

    public CounterIncremented(int val)
    {
      Counter = val;
    }
  }
}