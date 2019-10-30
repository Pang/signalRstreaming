using System.Threading.Channels;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace streamProject.api.Hub
{
    public class StreamHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task Pump(ChannelReader<int> incoming)
        {
            while (await incoming.WaitToReadAsync())
            {
                while (incoming.TryRead(out var item))
                {
                    NumberBank.bank.Enqueue(item);
                    if (NumberBank.bank.Count > 15) NumberBank.bank.Dequeue();
                    System.Console.WriteLine(NumberBank.bank.Count);
                    await Clients.Others.SendAsync("Item", NumberBank.bank);
                }
            }
        }
    }
}