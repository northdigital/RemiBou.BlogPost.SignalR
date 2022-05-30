using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace RemiBou.BlogPost.SignalR.Shared.Infrastructure
{
  public static class DynamicNotificationHandlers
  {
    private static Dictionary<Type, List<(object, Func<SerializedNotification, Task>)>> _handlers 
      = new Dictionary<Type, List<(object, Func<SerializedNotification, Task>)>>();

    public static void Register<T>(INotificationHandler<T> handler) where T : SerializedNotification
    {
      lock (_handlers)
      {
        var handlerInterfaces = handler
          .GetType()
          .GetInterfaces()
          .Where(x =>
              x.IsGenericType &&
              x.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
          .ToList();

        foreach (var item in handlerInterfaces)
        {
          var notificationType = item.GenericTypeArguments.First();

          if (!_handlers.TryGetValue(notificationType, out var handlers))
          {
            handlers = new List<(object, Func<SerializedNotification, Task>)>();
            _handlers.Add(notificationType, handlers);
          }

          try
          {
            handlers.Add((handler, async s => 
            await handler.Handle((T)s, default(CancellationToken))));
          }
          catch(Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
      }
    }

    public static void Unregister<T>(INotificationHandler<T> handler) where T : SerializedNotification
    {
      lock (_handlers)
      {
        foreach (var item in _handlers)
        {
          item.Value.RemoveAll(h => h.Item1.Equals(handler));
        }
      }
    }

    public static async Task Publish(SerializedNotification notification)
    {
      try
      {
        var notificationType = notification.GetType();

        if (_handlers.TryGetValue(notificationType, out var filtered))
        {
          foreach (var item in filtered)
          {
            try
            {
              await item.Item2(notification);
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
            }
          }
        }
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e + " " + e.StackTrace);

        throw;
      }
    }
  }
}
