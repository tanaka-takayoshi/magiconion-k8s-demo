using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using System;

namespace MagicOnion.Server
{

    // define interface as Server/Client IDL.
    // implements T : IService<T> and share this type between server and client.
    public interface IMyFirstService : IService<IMyFirstService>
    {
        // Return type must be `UnaryResult<T>` or `Task<UnaryResult<T>>`.
        // If you can use C# 7.0 or newer, recommend to use `UnaryResult<T>`.
        UnaryResult<int> SumAsync(int x, int y);

        UnaryResult<string> GetWebAsync(string url);
    }
}