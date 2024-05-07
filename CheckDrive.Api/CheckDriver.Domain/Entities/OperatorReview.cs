using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class OperatorReview
    {
        public double OilAmount { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public Operator? Operator { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }
    }
}
