using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

interface IDedicatedSMS
{    
    ValueTask<Order[]> GetOrders();
    ValueTask<List<NewMessageModel>> Compose(Order[] orders);
    ValueTask SendMessage(List<NewMessageModel> newMessages);
    ValueTask DoWork();
}

public class DedicatedSMS : IDedicatedSMS
{
    ILogger<DedicatedSMS> _logger;
    private readonly AppDbContext _context;
    private readonly HttpClient _http;
    public DedicatedSMS(AppDbContext context, HttpClient http, ILogger<DedicatedSMS> logger)
    {
        _context = context;
        _http = http;
        _logger = logger;
    }

    public async ValueTask<List<NewMessageModel>> Compose(Order[] orders)
    {
        _logger.LogInformation($"Composing {orders.Count()} messages...");
        List<NewMessageModel> ms =new();
        await Task.Run(() => 
        {
            foreach (Order order in orders)
            {
                ms.Add(new NewMessageModel
                {
                    OrderID = order.Id,
                    Recipient = order.Customer!.PhoneNo,
                    Message =$"Dear {order.Customer!.CustomerName}, Thank you for your order and hope we'll be seeing you again!"  
                });
            }
        });
        return ms;
    }

    public async ValueTask DoWork()
    {
        _logger.LogInformation("Dedicated SMS Dispatcher started...");
        var Orders  = await GetOrders();
        var ComposedMessages = await Compose(Orders);
        await SendMessage(ComposedMessages);
    }

    public async ValueTask<Order[]> GetOrders()
    {
        _logger.LogInformation("Fetching Orders...");
        return await _context.Orders.AsSplitQuery()
                                    .Include(x => x.Customer)
                                    .Where(x => x.IsMessageSent == false)
                                    .OrderByDescending(x => x.TransactionDate)
                                    .ToArrayAsync();
    }

    public async ValueTask SendMessage(List<NewMessageModel> newMessages)
    {
        _logger.LogInformation("Sending Message(s)...");
        //var settings = await _context.SMS_Settings.FirstOrDefaultAsync();
        foreach (var message in newMessages)
        {                                    
            //string url = $"api/?username={settings!.Username}&password={settings!.Password}&sender=AbujaInc&message={message.Message}&mobiles={message.Recipient}";
            string url = $"api/?username=aminualee02@hotmail.com&password=Musallim123&sender=AbujaInc&message={message.Message}&mobiles={message.Recipient}";
            var response = await _http.GetFromJsonAsync<object?>(url);            
            if (response!.ToString()!.Contains("OK", StringComparison.InvariantCultureIgnoreCase))        
                message.IsSent = true;                            
        }
        newMessages.ForEach(message => 
        {
            _context.Database.ExecuteSqlRaw("UPDATE Orders SET IsMessageSent=1 WHERE Id={0}", message.OrderID);
        });
        _logger.LogInformation($"SMS Dispatcher finished: Success -> {newMessages.Count(x => x.IsSent == true)} Failed -> {newMessages.Count(x => x.IsSent == false)}");
    }
}