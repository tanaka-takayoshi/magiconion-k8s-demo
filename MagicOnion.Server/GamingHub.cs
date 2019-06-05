// Server implementation
// implements : StreamingHubBase<THub, TReceiver>, THub
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using NewRelic.Api.Agent;

namespace MagicOnion.Server
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        // this class is instantiated per connected so fields are cache area of connection.
        IGroup room;
        Player self;
        IInMemoryStorage<Player> storage;

        public async Task<Player[]> JoinAsync(string roomName, string userName, double position, double rotation)
        {
            self = new Player() { Name = userName, Position = position, Rotation = rotation };

            // Group can bundle many connections and it has inmemory-storage so add any type per group. 
            (room, storage) = await Group.AddAsync(roomName, self);

            // Typed Server->Client broadcast.
            Broadcast(room).OnJoin(self);

            return storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            Broadcast(room).OnLeave(self);
        }

        [Transaction]
        public async Task MoveAsync(double position, double rotation)
        {
            if (position < 0)
            {
                var e = new ArgumentOutOfRangeException(nameof(position), position, "position should not be negative");
                throw e;
            }
            self.Position = position;
            self.Rotation = rotation;
            Broadcast(room).OnMove(self);
        }

        [Transaction]
        // You can hook OnConnecting/OnDisconnected by override.
        protected override ValueTask OnDisconnected()
        {
            // on disconnecting, if automatically removed this connection from group.
            return CompletedTask;
        }
    }
}