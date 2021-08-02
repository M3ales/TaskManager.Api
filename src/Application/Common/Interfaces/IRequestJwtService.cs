using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Common.Interfaces
{
    public interface IRequestJwtService
    {
        string Token { get; }
    }
}
