using CheckDriver.Domain.Common;

namespace CheckDriver.Domain.Entities
{
    public class Doctor : EntityBase
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DoctorReview> DoctorReviews { get; set; }
    }
}
