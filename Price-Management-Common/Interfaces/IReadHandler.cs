using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Price_Management_Common.Interfaces
{
    public interface IReadHandler<TRequest,TResponse> where TRequest:IQueryRequest where TResponse:IResponse
    {
        Task<TResponse> ReadOperationAsync(TRequest request, CancellationToken cancellationToken);
    }
}
