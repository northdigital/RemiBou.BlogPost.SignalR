using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using RemiBou.BlogPost.SignalR.Shared.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RemiBou.BlogPost.SignalR.Client
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("app");
      builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      builder.Services.AddSingleton<IEventAggregator, EventAggregator>();

      var app = builder.Build();
      var navigationManager = app.Services.GetRequiredService<NavigationManager>();
      var hubConnection = new HubConnectionBuilder()
        .WithUrl(navigationManager.ToAbsoluteUri("/notifications"))
        .AddJsonProtocol(o => o.PayloadSerializerOptions.Converters.Add(new NotificationJsonConverter()))
        .Build();

      var eventAggregator = app.Services.GetRequiredService<IEventAggregator>();

      hubConnection.On<SerializedNotification>("Notification", async (notificationJson) =>
      {
        await eventAggregator.PublishAsync(notificationJson);
      });

      await hubConnection.StartAsync();
      await app.RunAsync();
    }
  }
}
