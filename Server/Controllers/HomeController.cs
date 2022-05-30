using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RemiBou.BlogPost.SignalR.Shared.Notifications;

namespace RemiBou.BlogPost.SignalR.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class HomeController : ControllerBase
  {
    private static int Counter = 0;
    private IMediator _mediator;

    public HomeController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("increment")]
    public async Task IncrementValue()
    {
      int val = Interlocked.Increment(ref Counter);
      await _mediator.Publish(new CounterIncremented(val));
    }

    [HttpPost("date")]
    public async Task SetDate()
    {
      await _mediator.Publish(new DateTimeChanged(DateTime.Now));
    }
  }
}
