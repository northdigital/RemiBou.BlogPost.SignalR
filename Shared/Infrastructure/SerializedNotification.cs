using MediatR;

namespace RemiBou.BlogPost.SignalR.Shared.Infrastructure
{
  public abstract class SerializedNotification : INotification
  {
    public string NotificationType => GetType().Name;
  }
}
