using Microsoft.AspNetCore.SignalR;

public class ExchangeRatesHub : Hub
{
    private static Random _random = new Random();
    private static Dictionary<string, double> _exchangeRates = new Dictionary<string, double>();
    private static DateTime _dateTimeLastUpdate;
    private static bool _isFirstUpdate = true;

    public async Task SendUpdateRates(IHubContext<ExchangeRatesHub> hubContext)
    {
        UpdateRates();
        await hubContext.Clients.All.SendAsync("Receive", _exchangeRates, _dateTimeLastUpdate);
    }

    public override async Task OnConnectedAsync()
    {
        if (_isFirstUpdate)
        {
            UpdateRates();
            _isFirstUpdate = false;
        }
        await Clients.Caller.SendAsync("Receive", _exchangeRates, _dateTimeLastUpdate);
    }

    private void UpdateRates()
    {
        _exchangeRates["UAH/USD"] = 40.0 + _random.NextDouble() * (43.0 - 40.0);
        _exchangeRates["UAH/AUD"] = 25.0 + _random.NextDouble() * (28.0 - 25.0);
        _dateTimeLastUpdate = DateTime.Now;
    }
}