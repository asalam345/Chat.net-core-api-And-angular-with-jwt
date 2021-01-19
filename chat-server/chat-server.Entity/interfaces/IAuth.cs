using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Entity.interfaces
{
    public interface IAuth<T>
    {
        Task<Result> Login(T model);
        Task<Result> Register(T model);
    }
}
