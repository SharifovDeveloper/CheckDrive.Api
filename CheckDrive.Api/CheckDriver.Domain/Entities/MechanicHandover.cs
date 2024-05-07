using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class MechanicHandover
    {
        public int Id { get; set; }
        public bool IsHanded { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int MechanicId { get; set; }
        public Mechanic? Mechanic { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }

        public ICollection <MechanicAcceptance>? MechanicAcceptances { get; set; }
    }
}
