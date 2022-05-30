using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RemiBou.BlogPost.SignalR.Shared.Infrastructure;
using RemiBou.BlogPost.SignalR.Shared.Notifications;

namespace RemiBou.BlogPost.SignalR.Server.Hubs
{
  public class NotificationHub : Hub
  {
  }

  public class HubNotificationHandler : INotificationHandler<CounterIncremented>,
                                        INotificationHandler<DateTimeChanged>
  {
    private readonly IHubContext<NotificationHub> _hubContext;

    public HubNotificationHandler(IHubContext<NotificationHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task Handle(CounterIncremented notification, CancellationToken cancellationToken)
    {
      await SendCounterIncrementedNotification(notification);
    }

    public async Task Handle(DateTimeChanged notification, CancellationToken cancellationToken)
    {
      await SendDateTimeChangedNotification(notification);
    }

    private async Task SendCounterIncrementedNotification(SerializedNotification notification)
    {
      await _hubContext.Clients.All.SendAsync("Notification", notification);
    }

    private async Task SendDateTimeChangedNotification(SerializedNotification notification)
    {
      await _hubContext.Clients.All.SendAsync("Date", notification);
    }
  }
}
