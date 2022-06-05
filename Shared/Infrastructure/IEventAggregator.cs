using System;
using System.Threading;
using System.Threading.Tasks;

namespace RemiBou.BlogPost.SignalR.Shared.Infrastructure
{
  /// <summary>
  /// Enables loosely-coupled publication of and subscription to events.
  /// </summary>
  public interface IEventAggregator
  {
    /// <summary>
    /// Subscribes an instance to all events declared through implementations of <see cref = "IHandle{T}" />
    /// </summary>
    /// <param name = "subscriber">The instance to subscribe for event publication.</param>
    void Subscribe(object subscriber);

    /// <summary>
    /// Unsubscribes the instance from all events.
    /// </summary>
    /// <param name = "subscriber">The instance to unsubscribe.</param>
    void Unsubscribe(object subscriber);

    /// <summary>
    /// Publishes a message.
    /// </summary>
    /// <param name = "message">The message instance.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync(object message, CancellationToken cancellationToken = default);
  }
}
