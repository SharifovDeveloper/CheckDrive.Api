using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class DispetcherReview
    {
        public int Id { get; set; }
        public double FuelSpended {  get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }

        public int DispatcherId { get; set; }
        public Dispatcher? Dispatcher { get; set; }
        public int OperatorId { get; set; }
        public Operator? Operator { get; set; }
        public int MechanicId { get; set; }
        public Mechanic? Mechanic { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }
    }
}
