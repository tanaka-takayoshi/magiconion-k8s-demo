using Grpc.Core;
using MagicOnion;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicOnion.Server
{
    // implement RPC service to Server Project.
    // inehrit ServiceBase<interface>, interface
    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
    {
        public int Time { get; private set; }

        // You can use async syntax directly.
        public async UnaryResult<int> SumAsync(int x, int y)
        {   
            Logger.Debug($"Received:{x}, {y}");

            return x + y;
        }

        public async UnaryResult<string> GetWebAsync(string url)
        {
            Logger.Debug($"Received:{url}");
            var client = new HttpClient();
            var res = await client.GetAsync(url);
            var responseBody = await res.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}