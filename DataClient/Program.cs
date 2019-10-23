using System;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR.Client;

namespace DataClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/streamHub").Build();

            await connection.StartAsync();

            var channel = Channel.CreateUnbounded<int>();
            _ = connection.InvokeAsync("Pump", channel.Reader);

            var rng = new Random();

            while (true)
            {
                await channel.Writer.WriteAsync(rng.Next(1, 100));
                await Task.Delay(1000);
            }
        }
    }
}
