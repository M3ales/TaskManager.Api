using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Domain.Entities.Auth
{
    public class Claim
    {
        public int Id { get; set; }
        public Account Account{ get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
