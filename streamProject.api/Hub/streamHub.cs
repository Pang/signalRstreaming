using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace streamProject.API.Hub
{
    public class StreamHub : Microsoft.AspNetCore.SignalR.Hub
    {
        Queue<int> bank = new Queue<int>();

        public async Task Pump(ChannelReader<int> incoming)
        {
            while (await incoming.WaitToReadAsync())
            {
                while (incoming.TryRead(out var item))
                {
                    bank.Enqueue(item);
                    if (bank.Count > 15) bank.Dequeue();
                    System.Console.WriteLine(bank.Count);
                    await Clients.Others.SendAsync("Item", bank);
                }
            }
        }
    }
}