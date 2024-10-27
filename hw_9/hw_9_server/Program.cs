using Microsoft.AspNetCore.SignalR;
using Timer = System.Timers.Timer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<ExchangeRatesHub>("/rates");

StartCurrencyUpdates(app.Services);

void StartCurrencyUpdates(IServiceProvider services)
{
    var hubContext = services.GetRequiredService<IHubContext<ExchangeRatesHub>>();
    Timer timer = new Timer(5000);
    
    timer.Elapsed += async (sender, args) => 
    { 
        await SendUpdateRates(hubContext);
    };
    timer.Start();
}

async Task SendUpdateRates(IHubContext<ExchangeRatesHub> hubContext)
{
    var exchangeRatesHub = new ExchangeRatesHub();
    await exchangeRatesHub.SendUpdateRates(hubContext);
}

app.Run();