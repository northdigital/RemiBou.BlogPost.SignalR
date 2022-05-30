namespace RemiBou.BlogPost.SignalR.Shared
{
  public class CounterIncremented : SerializedNotification
  {
    public int Counter { get; set; }

    public CounterIncremented(int val)
    {
      Counter = val;
    }

    public CounterIncremented()
    {
    }

    public override string ToString()
    {
      return $"Counter incremented ! new value {Counter}";
    }
  }
}