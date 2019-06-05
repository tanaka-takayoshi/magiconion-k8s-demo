using Grpc.Core;
using MagicOnion.Server;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MagicOnion.Hosting;

namespace MagicOnion.Server
{
    class Program
    {   
        static async Task Main(string[] args)
        {
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
            
            await MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion(new[] {
                     new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure) })
                .RunConsoleAsync();
        }
    }
}
