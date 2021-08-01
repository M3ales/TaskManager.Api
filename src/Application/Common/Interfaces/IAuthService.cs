using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
