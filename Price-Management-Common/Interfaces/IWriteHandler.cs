using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Price_Management_Common.Interfaces
{
    public interface IWriteHandler<TRequest,TResponse> where TRequest : ICommandRequest where TResponse : IResponse
    {
        Task<TResponse> WriteOperationAsync(TRequest request, CancellationToken cancellationToken);
    }
}
