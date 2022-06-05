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
    private static Dictionary<Type, List<(object notificationType, Func<SerializedNotification, Task> handler)>> _handlers 
      = new Dictionary<Type, List<(object, Func<SerializedNotification, Task>)>>();

    public static void Register<T>(INotificationHandler<T> handler) 
      where T : SerializedNotification
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

        foreach (var handlerInterface in handlerInterfaces)
        {
          var notificationType = handlerInterface.GenericTypeArguments.First();

          if (!_handlers.TryGetValue(notificationType, out var handlers))
          {
            handlers = new List<(object, Func<SerializedNotification, Task>)>();
            _handlers.Add(notificationType, handlers);
          }

          try
          {
            handlers.Add(
            (
              handler,
              async (s) =>
              {
                if(s is T t)
                  await handler.Handle(t, default);
              }
            ));
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
        foreach (var _handler in _handlers)
        {
          _handler.Value.RemoveAll(h => h.handler.Equals(handler));
        }
      }
    }

    public static async Task Publish(SerializedNotification notification)
    {
      try
      {
        var notificationType = notification.GetType();

        if (_handlers.TryGetValue(notificationType, out var serializedNotoficationTypes))
        {
          foreach (var serializedNotificationType in serializedNotoficationTypes)
          {
            try
            {
              await serializedNotificationType.handler(notification);
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
