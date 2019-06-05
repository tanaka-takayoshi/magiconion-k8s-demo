using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.Server;

namespace MagicOnion.Client
{
    public class GamingHubClient : IGamingHubReceiver
    {
    
        IGamingHub client;
    
        public async Task<string> ConnectAsync(Channel grpcChannel, string roomName, string playerName)
        {
            client = StreamingHubClient.Connect<IGamingHub, IGamingHubReceiver>(grpcChannel, this);
    
            var roomPlayers = await client.JoinAsync(roomName, playerName, 0d, 0d);
            foreach (var player in roomPlayers)
            {
                (this as IGamingHubReceiver).OnJoin(player);
            }
    
            return playerName;
        }
    
        // methods send to server.
    
        public Task LeaveAsync()
        {
            return client.LeaveAsync();
        }
    
        public Task MoveAsync(double position, double rotation)
        {
            return client.MoveAsync(position, rotation);
        }
    
        // dispose client-connection before channel.ShutDownAsync is important!
        public Task DisposeAsync()
        {
            return client.DisposeAsync();
        }
    
        // You can watch connection state, use this for retry etc.
        public Task WaitForDisconnect()
        {
            return client.WaitForDisconnect();
        }
    
        // Receivers of message from server.
    
        void IGamingHubReceiver.OnJoin(Player player)
        {
            Console.WriteLine($"plaer:{player.Name} joined");
        }
    
        void IGamingHubReceiver.OnLeave(Player player)
        {
            Console.WriteLine($"plaer:{player.Name} left");
        }
    
        void IGamingHubReceiver.OnMove(Player player)
        {
            Console.WriteLine($"plaer:{player.Name} moved to {player.Position} {player.Rotation}");
        }
    }
}