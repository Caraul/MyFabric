using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace MyStateless.Interfaces
{
    public interface IMyStateless : IService
    {
        Task<DateTimeOffset> Now();
    }
}
