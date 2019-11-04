using System.Threading.Channels;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace streamProject.api.Hub
{
    public class StreamHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Itemz", NumberBank.bank);
            System.Console.WriteLine("connected with: " + NumberBank.bank.Count);
        }

        public async Task Pump(ChannelReader<int> incoming)
        {
            while (await incoming.WaitToReadAsync())
            {
                while (incoming.TryRead(out var item))
                {
                    NumberBank.bank.Enqueue(item);
                    if (NumberBank.bank.Count > 15) NumberBank.bank.Dequeue();
                    int[] bankArray = NumberBank.bank.ToArray();
                    System.Console.WriteLine(bankArray.Length);
                    await Clients.Others.SendAsync("Item", bankArray[0]);
                }
            }
        }
    }
}