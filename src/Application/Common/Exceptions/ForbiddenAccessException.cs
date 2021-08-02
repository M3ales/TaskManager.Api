using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Common.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("You do not have permission to access this resource") { }
    }
}
