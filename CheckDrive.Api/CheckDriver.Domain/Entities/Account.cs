using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public DateTime Bithdate { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Dispatcher>? Dispatchers { get; set; }
        public ICollection<Operator>? Operators { get; set; }
        public ICollection<Mechanic>? Mechanics { get; set; }
        public ICollection<Driver>? Drivers { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }
        public ICollection<Technician>? Technicians { get; set; }
    }
}
