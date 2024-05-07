using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
