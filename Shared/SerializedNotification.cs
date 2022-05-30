using MediatR;

namespace RemiBou.BlogPost.SignalR.Shared
{
  public abstract class SerializedNotification : INotification
  {
    public string NotificationType => GetType().Name;
  }
}
