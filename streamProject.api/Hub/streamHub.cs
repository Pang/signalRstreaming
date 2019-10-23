using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace streamProject.API.Hub
{
    public class StreamHub : Microsoft.AspNetCore.SignalR.Hub
    {

        public async Task Pump(ChannelReader<int> incoming)
        {
            while (await incoming.WaitToReadAsync())
            {
                while (incoming.TryRead(out var item))
                {
                    System.Console.WriteLine(item);
                    await Clients.Others.SendAsync("Item", item);
                }
            }
        }
    }
}

