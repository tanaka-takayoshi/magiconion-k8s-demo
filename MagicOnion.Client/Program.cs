using Grpc.Core;
using MagicOnion.Server;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MagicOnion.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
            
        }

        static async Task MainAsync()
        {
            var host = Environment.GetEnvironmentVariable("GRPC_HOST") ?? "localhost";
            int.TryParse(Environment.GetEnvironmentVariable("GRPC_PORT") ?? "12345", out var port);
            var channel = new Channel(host, port, ChannelCredentials.Insecure);
            
            var client = MagicOnionClient.Create<IMyFirstService>(channel);

            // call method.
            var result = await client.SumAsync(100, 200);
            Console.WriteLine("Client Received:" + result);

            while (true)
            {
                var tasks = Enumerable.Range(1,100)
                .Select(id => Emulate(channel, id))
                .ToArray();

                await Task.WhenAll(tasks);
            }
        }

        static async Task Emulate(Channel channel, int userId)
        {
            var random = new Random();
            var name = $"user{userId}";
            var room = $"room{random.Next(10)}";

            var userClient = new GamingHubClient();
            var res = await userClient.ConnectAsync(channel, room, name);
            await Task.Delay(TimeSpan.FromMilliseconds(random.Next(5)*100));

            var term = 100 + random.Next(100);
            for (int i = 0; i < term; i++)
            {
                var p = (random.NextDouble() - 0.2) * 10.0;
                var r = (random.NextDouble() - 0.1) * 10.0;
                try
                {
                    await userClient.MoveAsync(p, r);
                }
                catch (System.Exception)
                {
                }
                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(5)*100));
            }
            await userClient.LeaveAsync();
        }
    }
}
