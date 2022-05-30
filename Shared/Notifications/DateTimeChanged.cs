using RemiBou.BlogPost.SignalR.Shared.Infrastructure;
using System;

namespace RemiBou.BlogPost.SignalR.Shared.Notifications
{
  public class DateTimeChanged : SerializedNotification
  {
    public DateTime CurrentDate { get; set; }

    public DateTimeChanged()
    {
    }

    public DateTimeChanged(DateTime value)
    {
      CurrentDate = value;
    }
  }
}
