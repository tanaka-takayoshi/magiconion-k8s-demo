// Server -> Client definition
using System.Threading.Tasks;
using MagicOnion;
using MessagePack;

namespace MagicOnion.Server
{
    public interface IGamingHubReceiver
    {
        // return type shuold be `void` or `Task`, parameters are free.
        void OnJoin(Player player);
        void OnLeave(Player player);
        void OnMove(Player player);
    }
    
    // Client -> Server definition
    // implements `IStreamingHub<TSelf, TReceiver>`  and share this type between server and client.
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        // return type shuold be `Task` or `Task<T>`, parameters are free.
        Task<Player[]> JoinAsync(string roomName, string userName, double position, double rotation);
        Task LeaveAsync();
        Task MoveAsync(double position, double rotation);
    }
    
    // for example, request object by MessagePack.
    [MessagePackObject]
    public class Player
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public double Position { get; set; }
        [Key(2)]
        public double Rotation { get; set; }
    }
}