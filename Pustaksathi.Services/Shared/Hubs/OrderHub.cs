using Microsoft.AspNetCore.SignalR;

namespace Pustaksathi.Services.Shared.Hubs
{
    public class OrderHub: Hub
    {
        public async Task SendOrderUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
