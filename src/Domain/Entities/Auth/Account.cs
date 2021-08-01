using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Domain.Entities.Auth
{
    public class Account
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
