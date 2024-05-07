using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriver.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public ICollection<DoctorReview>? DoctorReviews { get; set; }
    }
}
